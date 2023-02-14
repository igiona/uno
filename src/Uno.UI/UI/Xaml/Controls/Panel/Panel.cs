﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml.Media.Animation;
using Uno.Extensions;
using Uno.UI.DataBinding;
using Windows.UI.Core;
using Windows.UI.Xaml.Media;
using Microsoft.UI.Xaml.Controls;
using Uno.UI.Xaml;

namespace Windows.UI.Xaml.Controls
{
	[Markup.ContentProperty(Name = "Children")]
	public partial class Panel : FrameworkElement, ICustomClippingElement, IPanel
	{
		private readonly BorderLayerRenderer _borderRenderer;

#if NET461 || UNO_REFERENCE_API
		private new UIElementCollection _children;
#else
		private UIElementCollection _children;
#endif

		private PanelTransitionHelper _transitionHelper;


		public Panel()
		{
			_borderRenderer = new BorderLayerRenderer(this);

			_children = new UIElementCollection(this);
		}

		private void UpdateBorder() => _borderRenderer.Update();

		private void OnChildrenCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
			=> OnChildrenChanged();

		private protected override void OnLoaded()
		{
			base.OnLoaded();

			_children.CollectionChanged += OnChildrenCollectionChanged;

			OnLoadedPartial();
		}

		partial void OnLoadedPartial();

		private protected override void OnUnloaded()
		{
			base.OnUnloaded();

			_children.CollectionChanged -= OnChildrenCollectionChanged;

			OnUnloadedPartial();
		}

		partial void OnUnloadedPartial();

		private protected virtual void OnChildAdded(IFrameworkElement element)
		{
			UpdateTransitions(element);
		}

		private void UpdateTransitions(IFrameworkElement element)
		{
			if (_transitionHelper == null)
			{
				return;
			}

			_transitionHelper.AddElement(element);
		}

		public UIElementCollection Children => _children;

#region ChildrenTransitions Dependency Property

		public TransitionCollection ChildrenTransitions
		{
			get { return (TransitionCollection)this.GetValue(ChildrenTransitionsProperty); }
			set { this.SetValue(ChildrenTransitionsProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Transitions.  This enables animation, styling, binding, etc...
		public static DependencyProperty ChildrenTransitionsProperty { get; } =
			DependencyProperty.Register("ChildrenTransitions", typeof(TransitionCollection), typeof(Panel), new FrameworkPropertyMetadata(null, OnChildrenTransitionsChanged));

		private static void OnChildrenTransitionsChanged(object dependencyObject, DependencyPropertyChangedEventArgs args)
		{
			var panel = dependencyObject as Panel;

			if (panel == null)
			{
				return;
			}

			if (panel._transitionHelper == null)
			{
				panel._transitionHelper = new PanelTransitionHelper(panel);
			}
		}

#endregion

		internal Thickness PaddingInternal { get; set; }

		internal Thickness BorderThicknessInternal { get; set; }

		internal Brush BorderBrushInternal { get; set; }

		internal CornerRadius CornerRadiusInternal { get; set; }

#region IsItemsHost DependencyProperty
		public static DependencyProperty IsItemsHostProperty { get; } = DependencyProperty.Register(
			"IsItemsHost", typeof(bool), typeof(Panel), new FrameworkPropertyMetadata(default(bool)));

		public bool IsItemsHost
		{
			get { return (bool)this.GetValue(IsItemsHostProperty); }
			private set { this.SetValue(IsItemsHostProperty, value); }
		}
#endregion

		private ManagedWeakReference _itemsOwnerRef;

		internal ItemsControl ItemsOwner
		{
			get => _itemsOwnerRef.Target as ItemsControl;
			set
			{
				WeakReferencePool.ReturnWeakReference(this, _itemsOwnerRef);
				_itemsOwnerRef = WeakReferencePool.RentWeakReference(this, value);
			}
		}

		internal void SetItemsOwner(ItemsControl itemsOwner)
		{
			ItemsOwner = itemsOwner;
			IsItemsHost = itemsOwner != null;
		}

		protected virtual void OnCornerRadiusChanged(CornerRadius oldValue, CornerRadius newValue) =>
			UpdateBorder();

		protected virtual void OnPaddingChanged(Thickness oldValue, Thickness newValue)
		{
			OnPaddingChangedPartial(oldValue, newValue);
			UpdateBorder();
		}

		partial void OnPaddingChangedPartial(Thickness oldValue, Thickness newValue);

		protected virtual void OnBorderThicknessChanged(Thickness oldValue, Thickness newValue)
		{
			OnBorderThicknessChangedPartial(oldValue, newValue);
			UpdateBorder();
		}
		partial void OnBorderThicknessChangedPartial(Thickness oldValue, Thickness newValue);

		protected virtual void OnBorderBrushChanged(Brush oldValue, Brush newValue) =>
			UpdateBorder();

		private protected override Thickness GetBorderThickness() => BorderThicknessInternal;

		internal override bool CanHaveChildren() => true;

		private protected void OnBackgroundSizingChangedInnerPanel(DependencyPropertyChangedEventArgs e)
		{
			base.OnBackgroundSizingChangedInner(e);

			UpdateBorder();
		}

		protected override void OnBackgroundChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnBackgroundChanged(e);
			UpdateBorder();
			OnBackgroundChangedPartial(e);
		}

		partial void OnBackgroundChangedPartial(DependencyPropertyChangedEventArgs e);

		internal override bool IsViewHit() => Border.IsViewHitImpl(this);
	}
}
