#pragma warning disable 108 // new keyword hiding
#pragma warning disable 114 // new keyword hiding
namespace Windows.UI.Core
{
	#if false || false || false || false || false || false || false
	[global::Uno.NotImplemented]
	#endif
	public  partial class CoreWindow : global::Windows.UI.Core.ICoreWindow,global::Windows.UI.Core.ICorePointerRedirector
	{
		// Skipping already declared property PointerPosition
		// Skipping already declared property PointerCursor
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public  global::Windows.UI.Core.CoreWindowFlowDirection FlowDirection
		{
			get
			{
				throw new global::System.NotImplementedException("The member CoreWindowFlowDirection CoreWindow.FlowDirection is not implemented in Uno.");
			}
			set
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "CoreWindowFlowDirection CoreWindow.FlowDirection");
			}
		}
		#endif
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public  bool IsInputEnabled
		{
			get
			{
				throw new global::System.NotImplementedException("The member bool CoreWindow.IsInputEnabled is not implemented in Uno.");
			}
			set
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "bool CoreWindow.IsInputEnabled");
			}
		}
		#endif
		// Skipping already declared property Dispatcher
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public  object AutomationHostProvider
		{
			get
			{
				throw new global::System.NotImplementedException("The member object CoreWindow.AutomationHostProvider is not implemented in Uno.");
			}
		}
		#endif
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public  global::Windows.Foundation.Collections.IPropertySet CustomProperties
		{
			get
			{
				throw new global::System.NotImplementedException("The member IPropertySet CoreWindow.CustomProperties is not implemented in Uno.");
			}
		}
		#endif
		// Skipping already declared property Visible
		// Skipping already declared property ActivationMode
		// Skipping already declared property DispatcherQueue
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public  global::Windows.UI.UIContext UIContext
		{
			get
			{
				throw new global::System.NotImplementedException("The member UIContext CoreWindow.UIContext is not implemented in Uno.");
			}
		}
		#endif
		// Forced skipping of method Windows.UI.Core.CoreWindow.AutomationHostProvider.get
		// Forced skipping of method Windows.UI.Core.CoreWindow.Bounds.get
		// Forced skipping of method Windows.UI.Core.CoreWindow.CustomProperties.get
		// Forced skipping of method Windows.UI.Core.CoreWindow.Dispatcher.get
		// Forced skipping of method Windows.UI.Core.CoreWindow.FlowDirection.get
		// Forced skipping of method Windows.UI.Core.CoreWindow.FlowDirection.set
		// Forced skipping of method Windows.UI.Core.CoreWindow.IsInputEnabled.get
		// Forced skipping of method Windows.UI.Core.CoreWindow.IsInputEnabled.set
		// Forced skipping of method Windows.UI.Core.CoreWindow.PointerCursor.get
		// Forced skipping of method Windows.UI.Core.CoreWindow.PointerCursor.set
		// Forced skipping of method Windows.UI.Core.CoreWindow.PointerPosition.get
		// Forced skipping of method Windows.UI.Core.CoreWindow.Visible.get
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public  void Activate()
		{
			global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "void CoreWindow.Activate()");
		}
		#endif
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public  void Close()
		{
			global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "void CoreWindow.Close()");
		}
		#endif
		// Skipping already declared method Windows.UI.Core.CoreWindow.GetAsyncKeyState(Windows.System.VirtualKey)
		// Skipping already declared method Windows.UI.Core.CoreWindow.GetKeyState(Windows.System.VirtualKey)
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || false || __NETSTD_REFERENCE__ || false
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__NETSTD_REFERENCE__")]
		public  void ReleasePointerCapture()
		{
			global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "void CoreWindow.ReleasePointerCapture()");
		}
		#endif
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || false || __NETSTD_REFERENCE__ || false
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__NETSTD_REFERENCE__")]
		public  void SetPointerCapture()
		{
			global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "void CoreWindow.SetPointerCapture()");
		}
		#endif
		// Forced skipping of method Windows.UI.Core.CoreWindow.Activated.add
		// Forced skipping of method Windows.UI.Core.CoreWindow.Activated.remove
		// Forced skipping of method Windows.UI.Core.CoreWindow.AutomationProviderRequested.add
		// Forced skipping of method Windows.UI.Core.CoreWindow.AutomationProviderRequested.remove
		// Forced skipping of method Windows.UI.Core.CoreWindow.CharacterReceived.add
		// Forced skipping of method Windows.UI.Core.CoreWindow.CharacterReceived.remove
		// Forced skipping of method Windows.UI.Core.CoreWindow.Closed.add
		// Forced skipping of method Windows.UI.Core.CoreWindow.Closed.remove
		// Forced skipping of method Windows.UI.Core.CoreWindow.InputEnabled.add
		// Forced skipping of method Windows.UI.Core.CoreWindow.InputEnabled.remove
		// Forced skipping of method Windows.UI.Core.CoreWindow.KeyDown.add
		// Forced skipping of method Windows.UI.Core.CoreWindow.KeyDown.remove
		// Forced skipping of method Windows.UI.Core.CoreWindow.KeyUp.add
		// Forced skipping of method Windows.UI.Core.CoreWindow.KeyUp.remove
		// Forced skipping of method Windows.UI.Core.CoreWindow.PointerCaptureLost.add
		// Forced skipping of method Windows.UI.Core.CoreWindow.PointerCaptureLost.remove
		// Forced skipping of method Windows.UI.Core.CoreWindow.PointerEntered.add
		// Forced skipping of method Windows.UI.Core.CoreWindow.PointerEntered.remove
		// Forced skipping of method Windows.UI.Core.CoreWindow.PointerExited.add
		// Forced skipping of method Windows.UI.Core.CoreWindow.PointerExited.remove
		// Forced skipping of method Windows.UI.Core.CoreWindow.PointerMoved.add
		// Forced skipping of method Windows.UI.Core.CoreWindow.PointerMoved.remove
		// Forced skipping of method Windows.UI.Core.CoreWindow.PointerPressed.add
		// Forced skipping of method Windows.UI.Core.CoreWindow.PointerPressed.remove
		// Forced skipping of method Windows.UI.Core.CoreWindow.PointerReleased.add
		// Forced skipping of method Windows.UI.Core.CoreWindow.PointerReleased.remove
		// Forced skipping of method Windows.UI.Core.CoreWindow.TouchHitTesting.add
		// Forced skipping of method Windows.UI.Core.CoreWindow.TouchHitTesting.remove
		// Forced skipping of method Windows.UI.Core.CoreWindow.PointerWheelChanged.add
		// Forced skipping of method Windows.UI.Core.CoreWindow.PointerWheelChanged.remove
		// Forced skipping of method Windows.UI.Core.CoreWindow.SizeChanged.add
		// Forced skipping of method Windows.UI.Core.CoreWindow.SizeChanged.remove
		// Forced skipping of method Windows.UI.Core.CoreWindow.VisibilityChanged.add
		// Forced skipping of method Windows.UI.Core.CoreWindow.VisibilityChanged.remove
		// Forced skipping of method Windows.UI.Core.CoreWindow.PointerPosition.set
		// Forced skipping of method Windows.UI.Core.CoreWindow.PointerRoutedAway.add
		// Forced skipping of method Windows.UI.Core.CoreWindow.PointerRoutedAway.remove
		// Forced skipping of method Windows.UI.Core.CoreWindow.PointerRoutedTo.add
		// Forced skipping of method Windows.UI.Core.CoreWindow.PointerRoutedTo.remove
		// Forced skipping of method Windows.UI.Core.CoreWindow.PointerRoutedReleased.add
		// Forced skipping of method Windows.UI.Core.CoreWindow.PointerRoutedReleased.remove
		// Forced skipping of method Windows.UI.Core.CoreWindow.ClosestInteractiveBoundsRequested.add
		// Forced skipping of method Windows.UI.Core.CoreWindow.ClosestInteractiveBoundsRequested.remove
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public  string GetCurrentKeyEventDeviceId()
		{
			throw new global::System.NotImplementedException("The member string CoreWindow.GetCurrentKeyEventDeviceId() is not implemented in Uno.");
		}
		#endif
		// Forced skipping of method Windows.UI.Core.CoreWindow.ResizeStarted.add
		// Forced skipping of method Windows.UI.Core.CoreWindow.ResizeStarted.remove
		// Forced skipping of method Windows.UI.Core.CoreWindow.ResizeCompleted.add
		// Forced skipping of method Windows.UI.Core.CoreWindow.ResizeCompleted.remove
		// Forced skipping of method Windows.UI.Core.CoreWindow.DispatcherQueue.get
		// Forced skipping of method Windows.UI.Core.CoreWindow.ActivationMode.get
		// Forced skipping of method Windows.UI.Core.CoreWindow.UIContext.get
		// Skipping already declared method Windows.UI.Core.CoreWindow.GetForCurrentThread()
		// Skipping already declared event Windows.UI.Core.CoreWindow.Activated
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public  event global::Windows.Foundation.TypedEventHandler<global::Windows.UI.Core.CoreWindow, global::Windows.UI.Core.AutomationProviderRequestedEventArgs> AutomationProviderRequested
		{
			[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
			add
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "event TypedEventHandler<CoreWindow, AutomationProviderRequestedEventArgs> CoreWindow.AutomationProviderRequested");
			}
			[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
			remove
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "event TypedEventHandler<CoreWindow, AutomationProviderRequestedEventArgs> CoreWindow.AutomationProviderRequested");
			}
		}
		#endif
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public  event global::Windows.Foundation.TypedEventHandler<global::Windows.UI.Core.CoreWindow, global::Windows.UI.Core.CharacterReceivedEventArgs> CharacterReceived
		{
			[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
			add
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "event TypedEventHandler<CoreWindow, CharacterReceivedEventArgs> CoreWindow.CharacterReceived");
			}
			[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
			remove
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "event TypedEventHandler<CoreWindow, CharacterReceivedEventArgs> CoreWindow.CharacterReceived");
			}
		}
		#endif
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public  event global::Windows.Foundation.TypedEventHandler<global::Windows.UI.Core.CoreWindow, global::Windows.UI.Core.CoreWindowEventArgs> Closed
		{
			[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
			add
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "event TypedEventHandler<CoreWindow, CoreWindowEventArgs> CoreWindow.Closed");
			}
			[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
			remove
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "event TypedEventHandler<CoreWindow, CoreWindowEventArgs> CoreWindow.Closed");
			}
		}
		#endif
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public  event global::Windows.Foundation.TypedEventHandler<global::Windows.UI.Core.CoreWindow, global::Windows.UI.Core.InputEnabledEventArgs> InputEnabled
		{
			[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
			add
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "event TypedEventHandler<CoreWindow, InputEnabledEventArgs> CoreWindow.InputEnabled");
			}
			[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
			remove
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "event TypedEventHandler<CoreWindow, InputEnabledEventArgs> CoreWindow.InputEnabled");
			}
		}
		#endif
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || false || __NETSTD_REFERENCE__ || false
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__NETSTD_REFERENCE__")]
		public  event global::Windows.Foundation.TypedEventHandler<global::Windows.UI.Core.CoreWindow, global::Windows.UI.Core.KeyEventArgs> KeyDown
		{
			[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__NETSTD_REFERENCE__")]
			add
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "event TypedEventHandler<CoreWindow, KeyEventArgs> CoreWindow.KeyDown");
			}
			[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__NETSTD_REFERENCE__")]
			remove
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "event TypedEventHandler<CoreWindow, KeyEventArgs> CoreWindow.KeyDown");
			}
		}
		#endif
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || false || __NETSTD_REFERENCE__ || false
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__NETSTD_REFERENCE__")]
		public  event global::Windows.Foundation.TypedEventHandler<global::Windows.UI.Core.CoreWindow, global::Windows.UI.Core.KeyEventArgs> KeyUp
		{
			[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__NETSTD_REFERENCE__")]
			add
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "event TypedEventHandler<CoreWindow, KeyEventArgs> CoreWindow.KeyUp");
			}
			[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__NETSTD_REFERENCE__")]
			remove
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "event TypedEventHandler<CoreWindow, KeyEventArgs> CoreWindow.KeyUp");
			}
		}
		#endif
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public  event global::Windows.Foundation.TypedEventHandler<global::Windows.UI.Core.CoreWindow, global::Windows.UI.Core.PointerEventArgs> PointerCaptureLost
		{
			[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
			add
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "event TypedEventHandler<CoreWindow, PointerEventArgs> CoreWindow.PointerCaptureLost");
			}
			[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
			remove
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "event TypedEventHandler<CoreWindow, PointerEventArgs> CoreWindow.PointerCaptureLost");
			}
		}
		#endif
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || false || __NETSTD_REFERENCE__ || false
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__NETSTD_REFERENCE__")]
		public  event global::Windows.Foundation.TypedEventHandler<global::Windows.UI.Core.CoreWindow, global::Windows.UI.Core.PointerEventArgs> PointerEntered
		{
			[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__NETSTD_REFERENCE__")]
			add
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "event TypedEventHandler<CoreWindow, PointerEventArgs> CoreWindow.PointerEntered");
			}
			[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__NETSTD_REFERENCE__")]
			remove
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "event TypedEventHandler<CoreWindow, PointerEventArgs> CoreWindow.PointerEntered");
			}
		}
		#endif
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || false || __NETSTD_REFERENCE__ || false
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__NETSTD_REFERENCE__")]
		public  event global::Windows.Foundation.TypedEventHandler<global::Windows.UI.Core.CoreWindow, global::Windows.UI.Core.PointerEventArgs> PointerExited
		{
			[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__NETSTD_REFERENCE__")]
			add
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "event TypedEventHandler<CoreWindow, PointerEventArgs> CoreWindow.PointerExited");
			}
			[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__NETSTD_REFERENCE__")]
			remove
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "event TypedEventHandler<CoreWindow, PointerEventArgs> CoreWindow.PointerExited");
			}
		}
		#endif
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || false || __NETSTD_REFERENCE__ || false
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__NETSTD_REFERENCE__")]
		public  event global::Windows.Foundation.TypedEventHandler<global::Windows.UI.Core.CoreWindow, global::Windows.UI.Core.PointerEventArgs> PointerMoved
		{
			[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__NETSTD_REFERENCE__")]
			add
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "event TypedEventHandler<CoreWindow, PointerEventArgs> CoreWindow.PointerMoved");
			}
			[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__NETSTD_REFERENCE__")]
			remove
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "event TypedEventHandler<CoreWindow, PointerEventArgs> CoreWindow.PointerMoved");
			}
		}
		#endif
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || false || __NETSTD_REFERENCE__ || false
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__NETSTD_REFERENCE__")]
		public  event global::Windows.Foundation.TypedEventHandler<global::Windows.UI.Core.CoreWindow, global::Windows.UI.Core.PointerEventArgs> PointerPressed
		{
			[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__NETSTD_REFERENCE__")]
			add
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "event TypedEventHandler<CoreWindow, PointerEventArgs> CoreWindow.PointerPressed");
			}
			[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__NETSTD_REFERENCE__")]
			remove
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "event TypedEventHandler<CoreWindow, PointerEventArgs> CoreWindow.PointerPressed");
			}
		}
		#endif
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || false || __NETSTD_REFERENCE__ || false
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__NETSTD_REFERENCE__")]
		public  event global::Windows.Foundation.TypedEventHandler<global::Windows.UI.Core.CoreWindow, global::Windows.UI.Core.PointerEventArgs> PointerReleased
		{
			[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__NETSTD_REFERENCE__")]
			add
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "event TypedEventHandler<CoreWindow, PointerEventArgs> CoreWindow.PointerReleased");
			}
			[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__NETSTD_REFERENCE__")]
			remove
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "event TypedEventHandler<CoreWindow, PointerEventArgs> CoreWindow.PointerReleased");
			}
		}
		#endif
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || false || __NETSTD_REFERENCE__ || false
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__NETSTD_REFERENCE__")]
		public  event global::Windows.Foundation.TypedEventHandler<global::Windows.UI.Core.CoreWindow, global::Windows.UI.Core.PointerEventArgs> PointerWheelChanged
		{
			[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__NETSTD_REFERENCE__")]
			add
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "event TypedEventHandler<CoreWindow, PointerEventArgs> CoreWindow.PointerWheelChanged");
			}
			[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__NETSTD_REFERENCE__")]
			remove
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "event TypedEventHandler<CoreWindow, PointerEventArgs> CoreWindow.PointerWheelChanged");
			}
		}
		#endif
		// Skipping already declared event Windows.UI.Core.CoreWindow.SizeChanged
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public  event global::Windows.Foundation.TypedEventHandler<global::Windows.UI.Core.CoreWindow, global::Windows.UI.Core.TouchHitTestingEventArgs> TouchHitTesting
		{
			[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
			add
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "event TypedEventHandler<CoreWindow, TouchHitTestingEventArgs> CoreWindow.TouchHitTesting");
			}
			[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
			remove
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "event TypedEventHandler<CoreWindow, TouchHitTestingEventArgs> CoreWindow.TouchHitTesting");
			}
		}
		#endif
		// Skipping already declared event Windows.UI.Core.CoreWindow.VisibilityChanged
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public  event global::Windows.Foundation.TypedEventHandler<global::Windows.UI.Core.ICorePointerRedirector, global::Windows.UI.Core.PointerEventArgs> PointerRoutedAway
		{
			[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
			add
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "event TypedEventHandler<ICorePointerRedirector, PointerEventArgs> CoreWindow.PointerRoutedAway");
			}
			[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
			remove
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "event TypedEventHandler<ICorePointerRedirector, PointerEventArgs> CoreWindow.PointerRoutedAway");
			}
		}
		#endif
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public  event global::Windows.Foundation.TypedEventHandler<global::Windows.UI.Core.ICorePointerRedirector, global::Windows.UI.Core.PointerEventArgs> PointerRoutedReleased
		{
			[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
			add
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "event TypedEventHandler<ICorePointerRedirector, PointerEventArgs> CoreWindow.PointerRoutedReleased");
			}
			[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
			remove
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "event TypedEventHandler<ICorePointerRedirector, PointerEventArgs> CoreWindow.PointerRoutedReleased");
			}
		}
		#endif
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public  event global::Windows.Foundation.TypedEventHandler<global::Windows.UI.Core.ICorePointerRedirector, global::Windows.UI.Core.PointerEventArgs> PointerRoutedTo
		{
			[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
			add
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "event TypedEventHandler<ICorePointerRedirector, PointerEventArgs> CoreWindow.PointerRoutedTo");
			}
			[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
			remove
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "event TypedEventHandler<ICorePointerRedirector, PointerEventArgs> CoreWindow.PointerRoutedTo");
			}
		}
		#endif
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public  event global::Windows.Foundation.TypedEventHandler<global::Windows.UI.Core.CoreWindow, global::Windows.UI.Core.ClosestInteractiveBoundsRequestedEventArgs> ClosestInteractiveBoundsRequested
		{
			[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
			add
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "event TypedEventHandler<CoreWindow, ClosestInteractiveBoundsRequestedEventArgs> CoreWindow.ClosestInteractiveBoundsRequested");
			}
			[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
			remove
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "event TypedEventHandler<CoreWindow, ClosestInteractiveBoundsRequestedEventArgs> CoreWindow.ClosestInteractiveBoundsRequested");
			}
		}
		#endif
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public  event global::Windows.Foundation.TypedEventHandler<global::Windows.UI.Core.CoreWindow, object> ResizeCompleted
		{
			[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
			add
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "event TypedEventHandler<CoreWindow, object> CoreWindow.ResizeCompleted");
			}
			[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
			remove
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "event TypedEventHandler<CoreWindow, object> CoreWindow.ResizeCompleted");
			}
		}
		#endif
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public  event global::Windows.Foundation.TypedEventHandler<global::Windows.UI.Core.CoreWindow, object> ResizeStarted
		{
			[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
			add
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "event TypedEventHandler<CoreWindow, object> CoreWindow.ResizeStarted");
			}
			[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
			remove
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "event TypedEventHandler<CoreWindow, object> CoreWindow.ResizeStarted");
			}
		}
		#endif
		// Processing: Windows.UI.Core.ICoreWindow
		// Processing: Windows.UI.Core.ICorePointerRedirector
	}
}
