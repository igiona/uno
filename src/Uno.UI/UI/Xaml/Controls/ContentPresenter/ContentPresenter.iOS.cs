﻿using System;
using System.Drawing;
using Uno.Extensions;
using Uno.UI;
using Uno.UI.Views.Controls;
using Uno.UI.DataBinding;
using UIKit;
using Windows.UI.Xaml.Shapes;

namespace Windows.UI.Xaml.Controls
{
	public partial class ContentPresenter
	{
		partial void SetUpdateTemplatePartial() => SetNeedsLayout();

		partial void RegisterContentTemplateRoot()
		{
			if (Subviews.Length != 0)
			{
				throw new Exception("A Xaml control may not contain more than one child.");
			}

			ContentTemplateRoot.Frame = Bounds;
			ContentTemplateRoot.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
			AddSubview(ContentTemplateRoot);
		}

		partial void UnregisterContentTemplateRoot()
		{
			// If Content is a view it may have already been set as Content somewhere else in certain scenarios, eg virtualizing collections
			if (ContentTemplateRoot.Superview == this)
			{
				ContentTemplateRoot?.RemoveFromSuperview();
			}
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			UpdateBorder();
		}

		bool ICustomClippingElement.AllowClippingToLayoutSlot => CornerRadius == CornerRadius.None;

		bool ICustomClippingElement.ForceClippingToLayoutSlot => false;
	}
}
