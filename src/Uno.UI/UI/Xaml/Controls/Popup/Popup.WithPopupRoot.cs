﻿#if __ANDROID__ || __WASM__ || __SKIA__
using Uno.Extensions;
using Uno.Disposables;
using Uno.Foundation.Logging;
using Microsoft.UI.Xaml.Controls.Primitives;
using System;
using Microsoft.UI.Xaml.Media;
using Uno.UI;
using Uno.UI.Xaml.Core;

namespace Microsoft.UI.Xaml.Controls.Primitives;

public partial class Popup
{
	private readonly SerialDisposable _closePopup = new SerialDisposable();

#if __ANDROID__
	private bool _useNativePopup = FeatureConfiguration.Popup.UseNativePopup;
	internal bool UseNativePopup => _useNativePopup;
#endif

	partial void InitializePartial()
	{
#if __ANDROID__
		if (_useNativePopup)
		{
			InitializeNativePartial();
		}
#endif

		PopupPanel = new PopupPanel(this);
	}

#if __ANDROID__
	partial void InitializeNativePartial();
#endif

	partial void OnChildChangedPartialNative(UIElement oldChild, UIElement newChild)
	{
		PopupPanel.Children.Remove(oldChild);

		if (newChild != null)
		{
			PopupPanel.Children.Add(newChild);
		}
	}

	partial void OnIsLightDismissEnabledChangedPartialNative(bool oldIsLightDismissEnabled, bool newIsLightDismissEnabled)
	{
#if __ANDROID__
		if (_useNativePopup)
		{
			OnIsLightDismissEnabledChangedNative(oldIsLightDismissEnabled, newIsLightDismissEnabled);
		}
		else
#endif
		{
			if (PopupPanel != null)
			{
				PopupPanel.Background = GetPanelBackground();
			}
		}
	}

#if __ANDROID__
	partial void OnIsLightDismissEnabledChangedNative(bool oldIsLightDismissEnabled, bool newIsLightDismissEnabled);
#endif

	partial void OnIsOpenChangedPartialNative(bool oldIsOpen, bool newIsOpen)
	{
		if (this.Log().IsEnabled(Uno.Foundation.Logging.LogLevel.Debug))
		{
			this.Log().Debug($"Popup.IsOpenChanged({oldIsOpen}, {newIsOpen})");
		}

#if __ANDROID__
		if (_useNativePopup)
		{
			OnIsOpenChangedNative(oldIsOpen, newIsOpen);
		}
		else
#endif
		{
			if (newIsOpen)
			{
#if !HAS_UNO_WINUI
				// In UWP, XamlRoot is set automatically to CoreWindow XamlRoot if not set beforehand.
				if (XamlRoot is null)
				{
					XamlRoot = CoreServices.Instance.ContentRootCoordinator.CoreWindowContentRoot.XamlRoot;
				}
#endif

#if !__SKIA__ // The OpenPopup method should be moved out of Window in general https://github.com/unoplatform/uno/issues/8978
				_closePopup.Disposable = Window.Current.OpenPopup(this);
#else
				var currentXamlRoot = XamlRoot ?? CoreServices.Instance.ContentRootCoordinator.CoreWindowContentRoot.XamlRoot;
				_closePopup.Disposable = currentXamlRoot?.OpenPopup(this);
#endif
				PopupPanel.Visibility = Visibility.Visible;
			}
			else
			{
				_closePopup.Disposable = null;
				PopupPanel.Visibility = Visibility.Collapsed;
			}
		}
	}

#if __ANDROID__
	partial void OnIsOpenChangedNative(bool oldIsOpen, bool newIsOpen);
#endif

	partial void OnPopupPanelChangedPartial(PopupPanel previousPanel, PopupPanel newPanel)
	{
#if __ANDROID__
		if (_useNativePopup)
		{
			OnPopupPanelChangedPartialNative(previousPanel, newPanel);
		}
		else
#endif
		{
			previousPanel?.Children.Clear();

			if (newPanel != null)
			{
				if (Child != null)
				{
					newPanel.Children.Add(Child);
				}
				newPanel.Background = GetPanelBackground();
			}
		}
	}

#if __ANDROID__
	partial void OnPopupPanelChangedPartialNative(PopupPanel previousPanel, PopupPanel newPanel);
#endif
}
#endif
