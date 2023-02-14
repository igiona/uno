﻿#if !HAS_UI_TESTS
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Uno.Disposables;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media;
using CoreGraphics;
using Uno.UI.Xaml.Media;
#if __IOS__
using UIKit;
using _Image = UIKit.UIImage;
#elif __MACOS__
using AppKit;
using _Image = AppKit.NSImage;
#endif

namespace Windows.UI.Xaml.Controls
{
	public partial class Border
	{
		protected override void OnAfterArrange()
		{
			base.OnAfterArrange();
			UpdateBorderLayer();
		}

		// MZ:TODO!!!
		private void UpdateBorderLayer(_Image backgroundImage = null)
		{
			if (IsLoaded)
			{
				if (backgroundImage == null)
				{
					ImageData backgroundImageData = default;
					if ((Background as ImageBrush)?.ImageSource?.TryOpenSync(out backgroundImageData) == true &&
						backgroundImageData.Kind == Uno.UI.Xaml.Media.ImageDataKind.NativeImage)
					{
						backgroundImage = backgroundImageData.NativeImage;
					}
				}

				if (_borderRenderer.UpdateLayer(Background, BackgroundSizing, BorderThickness, BorderBrush, CornerRadius, backgroundImage)
					is CGPath updated) // UpdateLayer may return null if there is no update
				{
					BoundsPath = updated;
					BoundsPathUpdated?.Invoke(this, default);
				}
			}

			this.SetNeedsDisplay();
		}

		partial void OnBackgroundChangedPartial(DependencyPropertyChangedEventArgs args)
		{
			//TODO:MZ:
			// Don't call base <-- THIS IS CURRENTLY NOT RIGHT AFTER?, we need to keep UIView.BackgroundColor set to transparent
			// because we're overriding draw.

			var old = args.OldValue as ImageBrush;
			if (old != null)
			{
				old.ImageChanged -= OnBackgroundImageBrushChanged;
			}
			var imgBrush = args.NewValue as ImageBrush;
			if (imgBrush != null)
			{
				imgBrush.ImageChanged += OnBackgroundImageBrushChanged;
			}
			else
			{
				UpdateBorderLayer();
			}
		}

		private void OnBackgroundImageBrushChanged(_Image backgroundImage)
		{
			UpdateBorderLayer(backgroundImage);
		}

		partial void OnChildChangedPartial(UIElement previousValue, UIElement newValue)
		{
			previousValue?.RemoveFromSuperview();

			if (newValue != null)
			{
				AddSubview(newValue);
			}

			UpdateBorderLayer();
		}

		bool ICustomClippingElement.AllowClippingToLayoutSlot => CornerRadius == CornerRadius.None && (!(Child is UIElement ue) || ue.RenderTransform == null);
		bool ICustomClippingElement.ForceClippingToLayoutSlot => false;

		internal event EventHandler BoundsPathUpdated;
		internal CGPath BoundsPath { get; private set; }
	}
}
#endif
