using System;
using System.IO;
using Gtk;
using Uno.ApplicationModel.DataTransfer;
using Uno.Extensions;
using Uno.Extensions.Storage.Pickers;
using Uno.Extensions.System;
using Uno.Extensions.UI.Core.Preview;
using Uno.Foundation.Extensibility;
using Uno.Foundation.Logging;
using Uno.Helpers.Theming;
using Uno.UI.Core.Preview;
using Uno.UI.Runtime.Skia.GTK.Extensions.ApplicationModel.DataTransfer;
using Uno.UI.Runtime.Skia.GTK.Extensions.Helpers;
using Uno.UI.Runtime.Skia.GTK.Extensions.Helpers.Theming;
using Uno.UI.Runtime.Skia.GTK.Extensions.System;
using Uno.UI.Runtime.Skia.GTK.Extensions.UI.Xaml.Controls;
using Uno.UI.Runtime.Skia.GTK.System.Profile;
using Uno.UI.Runtime.Skia.GTK.UI.Core;
using Uno.UI.Xaml.Controls.Extensions;
using Windows.Foundation;
using Windows.Storage.Pickers;
using Windows.System.Profile.Internal;
using Windows.UI.Core.Preview;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using UnoApplication = Windows.UI.Xaml.Application;
using WUX = Windows.UI.Xaml;
using Uno.UI.Xaml.Core;
using System.ComponentModel;
using Uno.Disposables;
using System.Collections.Generic;
using Uno.Extensions.ApplicationModel.Core;
using Windows.ApplicationModel;
using System.Runtime.InteropServices;
using System.Reflection;

namespace Uno.UI.Runtime.Skia
{
	public partial class GtkHost : ISkiaHost
	{
		private void PreloadHarfBuzz()
		{
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			{
				// Here, we preload libHarfBuzzSharp.so using the RTLD_DEEPBIND flag, so that
				// is loaded with its static dependencies, and not the ones from the system, particularly
				// libharfbuzz.so.0, which is not compatible with the one used by SkiaSharp.
				//
				// We use the runtime's native search directories (https://learn.microsoft.com/en-us/dotnet/core/dependency-loading/default-probing#unmanaged-native-library-probing)
				// to look for the runtime-provided binaries.
				//
				// See https://github.com/unoplatform/uno/issues/9793 for research on the topic.

				var search = AppContext.GetData("NATIVE_DLL_SEARCH_DIRECTORIES")?.ToString() ?? "";

				foreach (var path in search.Split(Path.PathSeparator))
				{
					var libPath = Path.Combine(path, "libHarfBuzzSharp.so");

					if (File.Exists(libPath))
					{
						if (Linux.dlopen(libPath, true) != IntPtr.Zero)
						{
							if (this.Log().IsEnabled(LogLevel.Trace))
							{
								this.Log().Trace($"Loaded eagerly: {libPath}");
							}
							break;
						}
					}
				}

				if (this.Log().IsEnabled(LogLevel.Trace))
				{
					this.Log().Trace($"Could not load eagerly: libHarfBuzzSharp");
				}
			}
		}

		// Imported from https://github.com/mono/SkiaSharp/blob/482e6ee2913a08a7cad76520ccf5fbce97c7c23b/binding/Binding.Shared/LibraryLoader.cs
		private static class Linux
		{
			private const string SystemLibrary = "libdl.so";
			private const string SystemLibrary2 = "libdl.so.2"; // newer Linux distros use this

			private const int RTLD_LAZY = 1;
			private const int RTLD_NOW = 2;
			private const int RTLD_DEEPBIND = 8;

			private static bool UseSystemLibrary2 = true;

			public static IntPtr dlopen(string path, bool lazy = true)
			{
				try
				{
					return dlopen2(path, (lazy ? RTLD_LAZY : RTLD_NOW) | RTLD_DEEPBIND);
				}
				catch (DllNotFoundException)
				{
					UseSystemLibrary2 = false;
					return dlopen1(path, (lazy ? RTLD_LAZY : RTLD_NOW) | RTLD_DEEPBIND);
				}
			}

			public static IntPtr dlsym(IntPtr handle, string symbol)
			{
				return UseSystemLibrary2 ? dlsym2(handle, symbol) : dlsym1(handle, symbol);
			}
			public static void dlclose(IntPtr handle)
			{
				if (UseSystemLibrary2)
					dlclose2(handle);
				else
					dlclose1(handle);
			}
			[DllImport(SystemLibrary, EntryPoint = "dlopen")]
			private static extern IntPtr dlopen1(string path, int mode);
			[DllImport(SystemLibrary, EntryPoint = "dlsym")]
			private static extern IntPtr dlsym1(IntPtr handle, string symbol);
			[DllImport(SystemLibrary, EntryPoint = "dlclose")]
			private static extern void dlclose1(IntPtr handle);
			[DllImport(SystemLibrary2, EntryPoint = "dlopen")]
			private static extern IntPtr dlopen2(string path, int mode);
			[DllImport(SystemLibrary2, EntryPoint = "dlsym")]
			private static extern IntPtr dlsym2(IntPtr handle, string symbol);
			[DllImport(SystemLibrary2, EntryPoint = "dlclose")]
			private static extern void dlclose2(IntPtr handle);
		}

	}
}
