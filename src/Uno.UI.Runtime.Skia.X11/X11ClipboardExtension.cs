// This implementation has a lot of the structure of Avalonia's implementation
// with the actual logic ported from xsel

// The MIT License (MIT)
//
// Copyright (c) .NET Foundation and Contributors
// All Rights Reserved
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

// Copyright (C) 2001 Conrad Parker <conrad@vergenet.net>
//
// Permission to use, copy, modify, distribute, and sell this software and its
// documentation for any purpose is hereby granted without fee, provided that
// the above copyright notice appear in all copies and that both that copyright
// notice and this permission notice appear in supporting documentation. No
// representations are made about the suitability of this software for any
// purpose. It is provided "as is" without express or implied warranty.

// https://github.com/kfish/xsel/blob/0ee15bfc8139d8eb595004ea093c6a57b7134470/xsel.c
// https://github.com/AvaloniaUI/Avalonia/blob/5fa3ffaeab7e5cd2662ef02d03e34b9d4cb1a489/src/Avalonia.X11/X11Clipboard.cs

#nullable disable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using SkiaSharp;
using Uno.ApplicationModel.DataTransfer;
using Uno.Disposables;
using Uno.Foundation.Logging;

namespace Uno.WinUI.Runtime.Skia.X11;

// https://specifications.freedesktop.org/clipboards-spec/clipboards-latest.txt
// https://www.jwz.org/doc/x-cut-and-paste.html
// https://jameshunt.us/writings/managing-the-x11-clipboard/
internal class X11ClipboardExtension : IClipboardExtension
{
	private readonly ImmutableList<IntPtr> _supportedAtoms;

	// TODO: fill these as we encounter new formats
	// Order from most to least preferable as the first available format will be used.
	private readonly ImmutableList<string> _imageFormats = ImmutableList.Create(
		"image/png",
		"image/jpeg",
		"image/bmp",
		"image/tiff",
		"image/avif",
		"image/ico");

	private readonly ImmutableDictionary<string, Encoding> _textFormats = new Dictionary<string, Encoding>
	{
		{ "UTF8_STRING", Encoding.UTF8 },
		{ "text/plain;charset=utf-8", Encoding.UTF8 },
		{ "UTF16_STRING", Encoding.Unicode },
		{ "text/plain;charset=utf-16", Encoding.Unicode },
		{ "XA_STRING", Encoding.ASCII },
		{ "OEMTEXT", Encoding.ASCII },
	}.ToImmutableDictionary();

	// We can mostly get away with having a single x11 window for selection events because Clipboard methods are only
	// allowed to be called when on the UI Thread, so no synchronization issues should be raised. Some questions
	// remain about having 2 different threads call DataPackageView.GetXXXAsync on the return value of GetContent().
	// In that case, we will get a data race (or probably a deadlock).
	// This will need to be reevaluated if we decide to give each window its own UI thread.
	private readonly X11Window _x11Window;

	private volatile bool _currentlyOwningClipboard;
	// these are only valid if CurrentlyOwningClipboard. However, reading outdated values is benign, so no lock needed for now
	private IntPtr _ownershipTimestamp;
	private DataPackageView _clipboardData;

#pragma warning disable CS0067 // Event is never used
	public event EventHandler<object> ContentChanged;
#pragma warning restore CS0067 // Event is never used

	public static X11ClipboardExtension Instance { get; } = new X11ClipboardExtension();

	private X11ClipboardExtension()
	{
		IntPtr display = XLib.XOpenDisplay(IntPtr.Zero);

		using var _1 = X11Helper.XLock(display);

		if (display == IntPtr.Zero)
		{
			if (this.Log().IsEnabled(LogLevel.Error))
			{
				this.Log().Error("XLIB ERROR: Cannot connect to X server");
			}
		}

		int screen = XLib.XDefaultScreen(display);

		IntPtr window = XLib.XCreateSimpleWindow(
			display,
			XLib.XRootWindow(display, screen),
			0,
			0,
			1,
			1,
			0,
			XLib.XBlackPixel(display, screen),
			XLib.XWhitePixel(display, screen));

		var _2 = XLib.XFlush(display); // unnecessary on most Xlib implementations

		if (this.Log().IsEnabled(LogLevel.Trace))
		{
			this.Log().Trace("XCLIP: Created event window.");
		}

		// Note how we do NOT map the window, since we're only using it for events, not actually showing anything
		_x11Window = new X11Window(display, window);

		_supportedAtoms = ImmutableList.Create(
			X11Helper.GetAtom(_x11Window.Display, X11Helper.TIMESTAMP),
			X11Helper.GetAtom(_x11Window.Display, X11Helper.MULTIPLE),
			X11Helper.GetAtom(_x11Window.Display, X11Helper.TARGETS)
		);

		var selThread = new Thread(ClipboardOwnerLoop)
		{
			Name = "X11 Selection Ownership",
			IsBackground = true
		};

		selThread.Start();
	}

	public void StartContentChanged() => throw new NotImplementedException();
	public void StopContentChanged() => throw new NotImplementedException();

	public void Clear() => SetContent(new DataPackage());

	// TODO: do we need to do anything specific here?
	// X11 has the bizarre concept of a clipboard owner, where if the clipboard owner goes away,
	// the clipboard vanishes. In practice, the DE persists the clipboard even if the x11 client
	// that set it goes away. Usually, the DE has a clipboard mananger, where it continuously
	// polls the owner of the clipboard for new values.
	public void Flush() { }

	public void SetContent(DataPackage content)
	{
		using var _1 = X11Helper.XLock(_x11Window.Display);

		_ownershipTimestamp = GetTimestamp();
		// Similar open issue for Gtk: https://github.com/unoplatform/uno/issues/14945
		var _2 = XLib.XSetSelectionOwner(
			_x11Window.Display,
			X11Helper.GetAtom(_x11Window.Display, X11Helper.CLIPBOARD),
			_x11Window.Window,
			_ownershipTimestamp);

		if (XLib.XGetSelectionOwner(_x11Window.Display, X11Helper.GetAtom(_x11Window.Display, X11Helper.CLIPBOARD)) == _x11Window.Window)
		{
			if (this.Log().IsEnabled(LogLevel.Information))
			{
				this.Log().LogInfo($"Successfully acquired x11 selection ownership");
			}

			_clipboardData = content.GetView();
			_currentlyOwningClipboard = true;
		}
		else
		{
			if (this.Log().IsEnabled(LogLevel.Error))
			{
				this.Log().Error($"Failed to acquire x11 selection");
			}

			_currentlyOwningClipboard = false;
		}
	}

	/// <returns>true if a supported property matches, otherwise false</returns>
	/// <remarks>We need an additional target parameter to support the recursive MULTIPLE</remarks>
	private bool SwitchTargets(IntPtr requestor, IntPtr target, IntPtr property)
	{
		using var _1 = X11Helper.XLock(_x11Window.Display);

		var targetName = XLib.GetAtomName(_x11Window.Display, target);
		if (target == X11Helper.GetAtom(_x11Window.Display, X11Helper.TIMESTAMP))
		{
			/* Return timestamp used to acquire ownership if target is TIMESTAMP */
			var _2 = XLib.XChangeProperty(
				_x11Window.Display,
				requestor,
				property,
				X11Helper.GetAtom(_x11Window.Display, X11Helper.XA_INTEGER),
				32,
				PropertyMode.Replace,
				_ownershipTimestamp,
				1);
		}
		else if (target == X11Helper.GetAtom(_x11Window.Display, X11Helper.TARGETS))
		{
			var atoms = _supportedAtoms.Concat(_textFormats.Keys.Select(name => X11Helper.GetAtom(_x11Window.Display, name))).ToArray();
			var _2 = XLib.XChangeProperty(
				_x11Window.Display,
				requestor,
				property,
				X11Helper.GetAtom(_x11Window.Display, X11Helper.XA_ATOM),
				32,
				PropertyMode.Replace,
				atoms,
				atoms.Length);
		}
		else if (_textFormats.TryGetValue(targetName, out var encoding) &&
			_clipboardData.AvailableFormats.Contains(targetName))
		{
			if (this.Log().IsEnabled(LogLevel.Trace))
			{
				this.Log().Trace($"XCLIP: directly matched requested text format {targetName}");
			}
			if (_clipboardData.FindRawData(StandardDataFormats.Text) is string s)
			{
				if (this.Log().IsEnabled(LogLevel.Trace))
				{
					this.Log().Trace("XCLIP: sending string to requestor.");
				}
				var bytes = encoding.GetBytes(s);
				var _2 = XLib.XChangeProperty(
					_x11Window.Display,
					requestor,
					property,
					target,
					8,
					PropertyMode.Replace,
					bytes,
					bytes.Length);
			}
			else if (_clipboardData.FindRawData(StandardDataFormats.Text) is byte[] bytes)
			{
				if (this.Log().IsEnabled(LogLevel.Trace))
				{
					this.Log().Trace("XCLIP: couldn't sending byte[] to requestor.");
				}
				var _2 = XLib.XChangeProperty(
					_x11Window.Display,
					requestor,
					property,
					target,
					8,
					PropertyMode.Replace,
					bytes,
					bytes.Length);
			}
			else
			{
				if (this.Log().IsEnabled(LogLevel.Error))
				{
					this.Log().Error("XCLIP: couldn't send text data to requestor.");
				}
			}
		}
		else if (_textFormats.TryGetValue(targetName, out var encoding2) &&
			(_clipboardData.AvailableFormats.Contains(StandardDataFormats.Text) || _clipboardData.AvailableFormats.Contains(StandardDataFormats.Uri)))
		{
			if (this.Log().IsEnabled(LogLevel.Trace))
			{
				this.Log().Trace($"XCLIP: didn't directly match requested text format {encoding}, but can send text anyway.");
			}
			var s = _clipboardData.AvailableFormats.Contains(StandardDataFormats.Text) ?
				_clipboardData.GetTextAsync().GetResults() :
				_clipboardData.GetUriAsync().GetResults().ToString(); // TODO: is this deadlock-proof?
			var bytes = encoding2.GetBytes(s);
			var _2 = XLib.XChangeProperty(
				_x11Window.Display,
				requestor,
				property,
				target,
				8,
				PropertyMode.Replace,
				bytes,
				bytes.Length);
		}
		else if (targetName == "text/html" &&
			_clipboardData.AvailableFormats.Contains(StandardDataFormats.Html))
		{
			if (this.Log().IsEnabled(LogLevel.Trace))
			{
				this.Log().Trace($"XCLIP: sending html data.");
			}
			var s = _clipboardData.GetHtmlFormatAsync().GetResults(); // TODO: is this deadlock-proof?
			var bytes = Encoding.GetEncoding(s).GetBytes(s);
			var _2 = XLib.XChangeProperty(
				_x11Window.Display,
				requestor,
				property,
				target,
				8,
				PropertyMode.Replace,
				bytes,
				bytes.Length);
		}
		else if (target == X11Helper.GetAtom(_x11Window.Display, X11Helper.MULTIPLE))
		{
			if (property == X11Helper.None)
			{
				if (this.Log().IsEnabled(LogLevel.Trace))
				{
					this.Log().Trace($"XCLIP: Invalid MULTIPLE request.");
				}
				return false;
			}

			var _2 = XLib.XGetWindowProperty(
				_x11Window.Display,
				requestor,
				property,
				0,
				new IntPtr(0x7fffffff),
				false,
				X11Helper.GetAtom(_x11Window.Display, X11Helper.ATOM_PAIR),
				out IntPtr _,
				out int format,
				out var length,
				out IntPtr _,
				out IntPtr atomsArray);
			using var _3 = Disposable.Create(() =>
			{
				var _ = XLib.XFree(atomsArray);
			});

			/* Make sure we got the Atom list we want */
			if (format != 32)
			{
				return true;
			}

			unsafe
			{
				var atomsSpan = new Span<IntPtr>(atomsArray.ToPointer(), (int)length);
				for (var i = 0; i < length; i += 2)
				{
					var target_ = atomsSpan[i];
					var prop = atomsSpan[i + 1];

					if (this.Log().IsEnabled(LogLevel.Trace))
					{
						this.Log().Trace($"XCLIP: Recursively calling {nameof(SwitchTargets)} on {XLib.GetAtomName(_x11Window.Display, prop)}");
					}
					SwitchTargets(requestor, target_, prop);
				}
			}
		}
		// TODO: support image copying.
		// Image conversion is broken in SkiaSharp on Linux as of 29/12/2023, so no need to
		// implement this right now.
		else if (_clipboardData.AvailableFormats.Contains(targetName))
		{
			// last-ditch effort
			if (_clipboardData.GetDataAsync(targetName).GetResults() is byte[] bytes)
			{
				if (this.Log().IsEnabled(LogLevel.Trace))
				{
					this.Log().Trace($"XCLIP: sending {targetName} data to requestor.");
				}
				var _2 = XLib.XChangeProperty(
					_x11Window.Display,
					requestor,
					property,
					target,
					8,
					PropertyMode.Replace,
					bytes,
					bytes.Length);
			}
		}
		else
		{
			if (this.Log().IsEnabled(LogLevel.Trace))
			{
				this.Log().Trace($"XCLIP: failed to match requestor target to available clipboard data formats.");
			}
			return false;
		}

		return true;
	}

	// We rely on the main thread to die in order to kill this background thread. This is fine
	// since the clipboard loop is shared between all windows, so we only need it to terminate after
	// all windows are closed.
	[DoesNotReturn]
	private unsafe void ClipboardOwnerLoop()
	{
		var fds = stackalloc X11Helper.Pollfd[1];
		fds[0].fd = XLib.XConnectionNumber(_x11Window.Display);
		fds[0].events = X11Helper.POLLIN;

		while (true)
		{
			if (this.Log().IsEnabled(LogLevel.Trace))
			{
				this.Log().Trace("XCLIP: Clipboard owner loop, waiting for events on {fds[0].fd}.");
			}

			var ret = X11Helper.poll(fds, 1, -1); // infinite waiting

			if (ret < 0)
			{
				if (this.Log().IsEnabled(LogLevel.Debug))
				{
					this.Log().Debug("Polling for X11 events failed, defaulting to SpinWait");
				}

				SpinWait.SpinUntil(() =>
				{
					using (X11Helper.XLock(_x11Window.Display))
					{
						return X11Helper.XPending(_x11Window.Display) > 0;
					}
				});
			}
			else if ((fds[0].revents & X11Helper.POLLIN) == 0)
			{
				continue;
			}

			using (X11Helper.XLock(_x11Window.Display))
			{
				while (X11Helper.XPending(_x11Window.Display) > 0)
				{
					XLib.XNextEvent(_x11Window.Display, out var event_);

					if (this.Log().IsEnabled(LogLevel.Trace))
					{
						this.Log().Trace($"XSEL EVENT: {event_.type}");
					}

					if (!_currentlyOwningClipboard)
					{
						if (this.Log().IsEnabled(LogLevel.Debug))
						{
							this.Log().Debug($"Not currently owning clipboard, skipping event: {event_.type}");
						}
						continue;
					}

					// TODO: implement INCR
					switch (event_.type)
					{
						case XEventName.SelectionClear:
							if (event_.SelectionEvent.selection == X11Helper.GetAtom(_x11Window.Display, X11Helper.CLIPBOARD))
							{
								if (this.Log().IsEnabled(LogLevel.Information))
								{
									this.Log().LogInfo($"Lost X11 selection ownership");
								}

								_currentlyOwningClipboard = false;
							}
							else
							{
								if (this.Log().IsEnabled(LogLevel.Trace))
								{
									this.Log().Trace($"Somehow losing X11 selection {XLib.GetAtomName(_x11Window.Display, event_.SelectionEvent.selection)}, even though we don't currently own it");
								}
							}
							break;
						case XEventName.SelectionRequest:
							if (event_.SelectionRequestEvent.selection != X11Helper.GetAtom(_x11Window.Display, X11Helper.CLIPBOARD))
							{
								if (this.Log().IsEnabled(LogLevel.Trace))
								{
									this.Log().Trace($"Somehow getting a SelectionRequest for {XLib.GetAtomName(_x11Window.Display, event_.SelectionEvent.selection)}, even though we don't currently own it");
								}
								break;
							}

							var xsr = event_.SelectionRequestEvent;
							XSelectionEvent ev = default; // reply

							ev.type = XEventName.SelectionNotify;
							ev.display = _x11Window.Display;
							ev.requestor = xsr.requestor;
							ev.selection = xsr.selection;
							ev.time = xsr.time;
							ev.send_event = 1;
							ev.target = xsr.target;

							if (this.Log().IsEnabled(LogLevel.Information))
							{
								this.Log().LogInfo($"Received SelectionRequest with target {XLib.GetAtomName(_x11Window.Display, ev.target)} and requestor {ev.requestor.ToString("X", CultureInfo.InvariantCulture)}");
							}

							if (xsr.property == X11Helper.None && ev.target != X11Helper.GetAtom(_x11Window.Display, X11Helper.MULTIPLE))
							{
								/* Obsolete requestor */
								xsr.property = xsr.target;
							}

							// To be ICCCM-compliant, we need to support TARGETS and MULTIPLE at the very least.

							if (ev.time != X11Helper.CurrentTime && ev.time < _ownershipTimestamp)
							{
								/* If the time is outside the period we have owned the selection,
								 * which is any time later than timestamp, or if the requested target
								 * is not a string, then refuse the SelectionRequest. NB. Some broken
								 * clients don't set a valid timestamp, so we have to check against
								 * CurrentTime here. */
								if (this.Log().IsEnabled(LogLevel.Trace))
								{
									this.Log().Trace($"XCLIP: Bad timestamp {ev.time} in clipboard request.");
								}
								ev.property = X11Helper.None;
							}
							else if (SwitchTargets(xsr.requestor, xsr.target, xsr.property))
							{
								ev.property = xsr.property;
							}

							if (ev.property != X11Helper.None)
							{
								if (this.Log().IsEnabled(LogLevel.Trace))
								{
									this.Log().Trace($"XCLIP: Sending reply to requestor {ev.requestor}.");
								}

								XEvent xev = default;
								xev.SelectionEvent = ev;
								var _ = XLib.XSendEvent(_x11Window.Display, ev.requestor, false,
									IntPtr.Zero, ref xev);
							}
							break;
					}
				}
			}
		}
		// ReSharper disable once FunctionNeverReturns
	}

	public DataPackageView GetContent()
	{
		if (_currentlyOwningClipboard)
		{
			return _clipboardData;
		}

		var dataPackage = new DataPackage();

		if (!HasOwner)
		{
			return dataPackage.GetView();
		}

		using var _ = X11Helper.XLock(_x11Window.Display);

		var formats = (WaitForFormats() ?? Array.Empty<IntPtr>())
			.Select(a => (name: XLib.GetAtomName(_x11Window.Display, a), atom: a))
			.ToList();

		// Supported formats will use a StandardDataFormats string, so we filter them out here.
		foreach (var format in formats)
		{
			dataPackage.SetDataProvider(format.name, async ct => await Task.Run(() => WaitForBytes(format.atom), ct));
		}

		if (formats.FirstOrDefault(f => _imageFormats.Contains(f.name)) is var f1 && f1.atom != IntPtr.Zero)
		{
			dataPackage.SetDataProvider(StandardDataFormats.Bitmap,
				async _ => await Task.FromResult(WaitForBmp(f1.atom)));
		}

		if (formats.FirstOrDefault(f => _textFormats.ContainsKey(f.name)) is var f2 && f2.atom != IntPtr.Zero)
		{
			dataPackage.SetDataProvider(StandardDataFormats.Text,
				async ct => await Task.Run(() => WaitForText(f2.atom), ct));
		}

		return dataPackage.GetView();

		// For easier deadlock debugging, replace Task.Run with
		// return await Task.Factory.StartNew(() =>
		// {
		//    Thread.CurrentThread.Name = $"XSEL {DateTime.Now}";
		//    return <your method>();
		// }, TaskCreationOptions.LongRunning);
	}

	/// <summary>checks if a clipboard owner exists (i.e. this is a clipboard)</summary>
	private bool HasOwner
	{
		get
		{
			using var _ = X11Helper.XLock(_x11Window.Display);
			return XLib.XGetSelectionOwner(_x11Window.Display, X11Helper.GetAtom(_x11Window.Display, X11Helper.CLIPBOARD)) != IntPtr.Zero;
		}
	}

	private IntPtr[] WaitForFormats()
	{
		using var _1 = X11Helper.XLock(_x11Window.Display);

		if (!HasOwner)
		{
			if (this.Log().IsEnabled(LogLevel.Error))
			{
				this.Log().Error($"{nameof(WaitForFormats)}: Found the X11 clipboard to be owner-less.");
			}

			return null;
		}

		var clipboardAtom = X11Helper.GetAtom(_x11Window.Display, X11Helper.CLIPBOARD);
		var targetsAtom = X11Helper.GetAtom(_x11Window.Display, X11Helper.TARGETS);
		var _2 = XLib.XConvertSelection(
			_x11Window.Display,
			clipboardAtom,
			targetsAtom,
			targetsAtom,
			_x11Window.Window,
			X11Helper.CurrentTime);

		// We don't need an event mask here as Selection events aren't maskable.

		var _3 = XLib.XNextEvent(_x11Window.Display, out var event_);

		if (event_.type != XEventName.SelectionNotify)
		{
			if (this.Log().IsEnabled(LogLevel.Error))
			{
				this.Log().Error($"{nameof(WaitForFormats)}: Unexpected X {event_.type} event while waiting for clipboard formats.");
			}

			return null;
		}

		var sel = event_.SelectionEvent;
		if (sel.property != targetsAtom)
		{
			if (this.Log().IsEnabled(LogLevel.Error))
			{
				this.Log().Error($"{nameof(WaitForFormats)}: Expected property {XLib.GetAtomName(_x11Window.Display, targetsAtom)}, instead found {XLib.GetAtomName(_x11Window.Display, sel.property)}");
			}

			return null;
		}

		if (sel.selection != clipboardAtom)
		{
			if (this.Log().IsEnabled(LogLevel.Error))
			{
				this.Log().Error($"{nameof(WaitForFormats)}: Expected SelectionEvent.selection to be {XLib.GetAtomName(_x11Window.Display, clipboardAtom)}, instead found {XLib.GetAtomName(_x11Window.Display, event_.SelectionEvent.selection)}");
			}
		}

		if (this.Log().IsEnabled(LogLevel.Trace))
		{
			this.Log().Trace($"{nameof(WaitForFormats)}: XSELELECTION EVENT: {event_.type} {event_.SelectionEvent}");
		}

		var _4 = XLib.XGetWindowProperty(
			_x11Window.Display,
			_x11Window.Window,
			sel.property,
			IntPtr.Zero,
			new IntPtr(0x7fffffff),
			true,
			X11Helper.AnyPropertyType,
			out var _,
			out var actualFormat,
			out var nitems,
			out var _,
			out var prop);
		using var _5 = Disposable.Create(() =>
		{
			var _ = XLib.XFree(prop);
		});

		// TODO: do we need this check?
		// if (nitems == IntPtr.Zero)
		// {
		// 	return null;
		// }

		if (actualFormat != 32)
		{
			if (this.Log().IsEnabled(LogLevel.Error))
			{
				this.Log().Error($"{nameof(WaitForFormats)}: Expected actual_format to be 32 (IntPtr), INSTEAD FOUND {actualFormat}");
			}

			return null;
		}

		var formats = new IntPtr[nitems.ToInt32()];
		Marshal.Copy(prop, formats, 0, formats.Length);
		return formats;
	}

	private byte[] WaitForBytes(IntPtr format)
	{
		using var _1 = X11Helper.XLock(_x11Window.Display);

		if (!HasOwner)
		{
			if (this.Log().IsEnabled(LogLevel.Error))
			{
				this.Log().Error($"{nameof(WaitForBytes)}: Found the X11 clipboard to be owner-less.");
			}

			return null;
		}

		var clipboardAtom = X11Helper.GetAtom(_x11Window.Display, X11Helper.CLIPBOARD);
		var _2 = XLib.XConvertSelection(_x11Window.Display, clipboardAtom, format, format,
			_x11Window.Window, X11Helper.CurrentTime);

		// We don't need an event mask here as Selection events aren't maskable.

		var _3 = XLib.XNextEvent(_x11Window.Display, out var event_);

		if (event_.type != XEventName.SelectionNotify)
		{
			if (this.Log().IsEnabled(LogLevel.Error))
			{
				this.Log().Error($"{nameof(WaitForBytes)}: Unexpected {event_.type} event while waiting for clipboard data.");
			}

			return null;
		}

		var sel = event_.SelectionEvent;
		if (sel.property != format)
		{
			if (this.Log().IsEnabled(LogLevel.Error))
			{
				this.Log().Error($"{nameof(WaitForBytes)}: Expected property {XLib.GetAtomName(_x11Window.Display, format)}, instead found {XLib.GetAtomName(_x11Window.Display, sel.property)}");
			}

			return null;
		}

		if (sel.selection != clipboardAtom)
		{
			if (this.Log().IsEnabled(LogLevel.Error))
			{
				this.Log().Error($"{nameof(WaitForBytes)}: Expected SelectionEvent.selection to be {XLib.GetAtomName(_x11Window.Display, clipboardAtom)}, INSTEAD FOUND {XLib.GetAtomName(_x11Window.Display, event_.SelectionEvent.selection)}");
			}
		}

		if (this.Log().IsEnabled(LogLevel.Trace))
		{
			this.Log().Trace($"{nameof(WaitForBytes)}: XSELELECTION EVENT: {event_.type} {event_.SelectionEvent}");
		}

		var _4 = XLib.XGetWindowProperty(
			_x11Window.Display,
			_x11Window.Window,
			sel.property,
			IntPtr.Zero,
			new IntPtr(0x7fffffff),
			true,
			X11Helper.AnyPropertyType,
			out var actualTypeAtom,
			out var actualFormat,
			out var nitems,
			out var bytes_after,
			out var prop);
		var readonlyProp = prop;
		using var propDisposable = Disposable.Create(() =>
		{
			var _ = XLib.XFree(readonlyProp);
		});

		var incrAtom = X11Helper.GetAtom(_x11Window.Display, X11Helper.INCR);
		if (actualTypeAtom == incrAtom)
		{
			// INCR state as of 28/12/2023: xsel implements INCR with an MIT License
			// xclip implements INCR with nice comments but is GPL2. Avalonia doesn't implement it.

			// This is particularly important for copying and pasting images.
			// We follow https://tronche.com/gui/x/icccm/sec-2.html#s-2.7.2 and xsel

			// prop contains a lower bound on the number of bytes, maybe we should use that to set the initial capacity somehow?
			var stream = new MemoryStream();

			var _5 = XLib.XDeleteProperty(_x11Window.Display, _x11Window.Window, sel.property);
			var _6 = XLib.XFlush(_x11Window.Display); // just a precaution

			XWindowAttributes attributes = default;
			var _7 = XLib.XGetWindowAttributes(_x11Window.Display, _x11Window.Window, ref attributes);

			using var maskDisposable = Disposable.Create(() =>
			{
				var _ = XLib.XSelectInput(_x11Window.Display, _x11Window.Window, attributes.your_event_mask);
			});
			var _8 = XLib.XSelectInput(_x11Window.Display, _x11Window.Window, (IntPtr)EventMask.PropertyChangeMask);

			while (true)
			{
				var _9 = XLib.XNextEvent(_x11Window.Display, out event_);

				if (event_.type != XEventName.PropertyNotify || event_.PropertyEvent.state != X11Helper.PropertyNewValue)
				{
					continue;
				}

				var _10 = XLib.XGetWindowProperty(
					_x11Window.Display,
					_x11Window.Window,
					sel.property,
					IntPtr.Zero,
					new IntPtr(0x7fffffff),
					true,
					X11Helper.AnyPropertyType,
					out actualTypeAtom,
					out actualFormat,
					out nitems,
					out bytes_after,
					out prop);
				var readonlyProp2 = prop;
				using var propDisposable2 = Disposable.Create(() =>
				{
					var _ = XLib.XFree(readonlyProp2);
				});

				if (this.Log().IsEnabled(LogLevel.Trace))
				{
					this.Log().Trace($"{nameof(WaitForBytes)}: received INCR chunk, {bytes_after} remaining");
				}

				if (bytes_after == 0)
				{
					// end of transfer
					break;
				}

				unsafe
				{
					var span = new ReadOnlySpan<byte>(prop.ToPointer(), (int)nitems * (actualFormat / 8));
					stream.Write(span);
				}
			}

			return stream.ToArray();
		}

		if (actualTypeAtom != format)
		{
			if (this.Log().IsEnabled(LogLevel.Error))
			{
				this.Log().Error($"{nameof(WaitForBytes)}: expected format {XLib.GetAtomName(_x11Window.Display, format)}, instead found {XLib.GetAtomName(_x11Window.Display, actualTypeAtom)}");
			}

			return null;
		}

		// TODO: do we need this check?
		// if (nitems == IntPtr.Zero)
		// {
		// 	return null;
		// }

		var data = new byte[(int)nitems * (actualFormat / 8)];
		Marshal.Copy(prop, data, 0, data.Length);
		return data;
	}

	private string WaitForText(IntPtr format)
	{
		using var _ = X11Helper.XLock(_x11Window.Display);

		if (_textFormats.TryGetValue(XLib.GetAtomName(_x11Window.Display, format), out var enc))
		{
			if (this.Log().IsEnabled(LogLevel.Trace))
			{
				this.Log().Trace($"{nameof(WaitForText)}: found acceptable text format, using encoding {enc}");
			}
			return enc.GetString(WaitForBytes(format));
		}

		return null;
	}

	private byte[] WaitForBmp(IntPtr format)
	{
		// Note: this will most likely crash on Linux due to a bug in skia/skiasharp.
		// Either Decode or Encode will return null
		var bytes = WaitForBytes(format);
		var bitmap = SKBitmap.Decode(bytes);
		return bitmap.Encode(SKEncodedImageFormat.Bmp, 100).ToArray();
	}

	private IntPtr GetTimestamp()
	{
		XWindowAttributes attributes = default;
		var _1 = XLib.XGetWindowAttributes(_x11Window.Display, _x11Window.Window, ref attributes);

		using var maskDisposable = Disposable.Create(() =>
		{
			var _ = XLib.XSelectInput(_x11Window.Display, _x11Window.Window, attributes.your_event_mask);
		});
		var _2 = XLib.XSelectInput(_x11Window.Display, _x11Window.Window, (IntPtr)EventMask.PropertyChangeMask);

		if (this.Log().IsEnabled(LogLevel.Trace))
		{
			this.Log().Trace("XCLIP: Setting dummy prop on window to get timestamp");
		}

		var _3 = XLib.XChangeProperty(
			_x11Window.Display,
			_x11Window.Window,
			X11Helper.GetAtom(_x11Window.Display, "DUMMY_PROP_TO_GET_TIMESTAMP"),
			X11Helper.GetAtom(_x11Window.Display, X11Helper.XA_INTEGER),
			32,
			PropertyMode.Replace,
			new IntPtr[] { 0 },
			1);

		while (true)
		{
			if (this.Log().IsEnabled(LogLevel.Trace))
			{
				this.Log().Trace("XCLIP: Waiting for PropertyNotify to get server timestamp.");
			}

			var _4 = XLib.XNextEvent(_x11Window.Display, out var event_);

			if (event_.type == XEventName.PropertyNotify)
			{
				return event_.PropertyEvent.time;
			}
			else
			{
				if (this.Log().IsEnabled(LogLevel.Trace))
				{
					this.Log().Trace($"XCLIP: unexpected event {event_.type} while waiting for PropertyNotify to get server timestamp.");
				}
			}
		}
	}
}
