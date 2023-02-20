#pragma warning disable 108 // new keyword hiding
#pragma warning disable 114 // new keyword hiding
namespace Microsoft.UI.Xaml.Input
{
	#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
	[global::Uno.NotImplemented]
	#endif
	public  partial class AccessKeyManager 
	{
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public static bool AreKeyTipsEnabled
		{
			get
			{
				throw new global::System.NotImplementedException("The member bool AccessKeyManager.AreKeyTipsEnabled is not implemented. For more information, visit https://aka.platform.uno/notimplemented?m=bool%20AccessKeyManager.AreKeyTipsEnabled");
			}
			set
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Microsoft.UI.Xaml.Input.AccessKeyManager", "bool AccessKeyManager.AreKeyTipsEnabled");
			}
		}
		#endif
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public static bool IsDisplayModeEnabled
		{
			get
			{
				throw new global::System.NotImplementedException("The member bool AccessKeyManager.IsDisplayModeEnabled is not implemented. For more information, visit https://aka.platform.uno/notimplemented?m=bool%20AccessKeyManager.IsDisplayModeEnabled");
			}
		}
		#endif
		// Forced skipping of method Microsoft.UI.Xaml.Input.AccessKeyManager.IsDisplayModeEnabled.get
		// Forced skipping of method Microsoft.UI.Xaml.Input.AccessKeyManager.AreKeyTipsEnabled.get
		// Forced skipping of method Microsoft.UI.Xaml.Input.AccessKeyManager.AreKeyTipsEnabled.set
		// Forced skipping of method Microsoft.UI.Xaml.Input.AccessKeyManager.IsDisplayModeEnabledChanged.add
		// Forced skipping of method Microsoft.UI.Xaml.Input.AccessKeyManager.IsDisplayModeEnabledChanged.remove
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public static void ExitDisplayMode()
		{
			global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Microsoft.UI.Xaml.Input.AccessKeyManager", "void AccessKeyManager.ExitDisplayMode()");
		}
		#endif
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public static event global::Windows.Foundation.TypedEventHandler<object, object> IsDisplayModeEnabledChanged
		{
			[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
			add
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Microsoft.UI.Xaml.Input.AccessKeyManager", "event TypedEventHandler<object, object> AccessKeyManager.IsDisplayModeEnabledChanged");
			}
			[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
			remove
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Microsoft.UI.Xaml.Input.AccessKeyManager", "event TypedEventHandler<object, object> AccessKeyManager.IsDisplayModeEnabledChanged");
			}
		}
		#endif
	}
}
