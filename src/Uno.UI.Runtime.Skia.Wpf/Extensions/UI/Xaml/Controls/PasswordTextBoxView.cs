﻿using System;
using System.Windows;
using Windows.UI.Xaml.Controls;

namespace Uno.UI.Runtime.Skia.Wpf.Extensions.UI.Xaml.Controls;

internal class PasswordTextBoxView : WpfTextBoxView
{
	//public override string Text { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

	//protected override Widget RootWidget => throw new NotImplementedException();

	//protected override Widget InputWidget => throw new NotImplementedException();

	//public override (int start, int end) GetSelectionBounds() => throw new NotImplementedException();
	//public override bool IsCompatible(TextBox textBox) => throw new NotImplementedException();
	//public override IDisposable ObserveTextChanges(EventHandler onChanged)
	//{

	//	_textChangedDisposable.Disposable = null;
	//	CompositeDisposable disposable = new();
	//	if (_currentTextBoxInputWidget is not null)
	//	{
	//		_currentTextBoxInputWidget.TextChanged += WpfTextViewTextChanged;
	//		disposable.Add(Disposable.Create(() => _currentTextBoxInputWidget.TextChanged -= WpfTextViewTextChanged));
	//	}

	//	if (_currentPasswordBoxInputWidget is not null)
	//	{
	//		_currentPasswordBoxInputWidget.PasswordChanged += PasswordBoxViewPasswordChanged;
	//		disposable.Add(Disposable.Create(() => _currentPasswordBoxInputWidget.PasswordChanged -= PasswordBoxViewPasswordChanged));
	//	}
	//	_textChangedDisposable.Disposable = disposable;
	//}

	//public override void SetSelectionBounds(int start, int end) => throw new NotImplementedException();

	//private System.Windows.Controls.PasswordBox CreatePasswordControl()
	//{
	//	var passwordBox = new System.Windows.Controls.PasswordBox();
	//	passwordBox.BorderBrush = System.Windows.Media.Brushes.Transparent;
	//	passwordBox.Background = System.Windows.Media.Brushes.Transparent;
	//	passwordBox.BorderThickness = new Thickness(0);
	//	return passwordBox;
	//}




	//public void SetIsPassword(bool isPassword)
	//{
	//	_isPasswordRevealed = !isPassword;
	//	if (_owner.TextBox is { } textBox)
	//	{
	//		if (_currentTextBoxInputWidget is not null)
	//		{
	//			_currentTextBoxInputWidget.Visibility = isPassword ? Visibility.Collapsed : Visibility.Visible;
	//			if (_currentPasswordBoxInputWidget is not null)
	//			{
	//				_currentPasswordBoxInputWidget.Visibility = isPassword ? Visibility.Visible : Visibility.Collapsed;
	//			}

	//		}
	//	}



	//	// Display

	//	if (_isPasswordBox)
	//	{
	//		textInputLayer.Children.Add(_currentPasswordBoxInputWidget!);
	//		_currentPasswordBoxInputWidget!.Visibility = _isPasswordRevealed ? Visibility.Collapsed : Visibility.Visible;
	//		_currentTextBoxInputWidget!.Visibility = _isPasswordRevealed ? Visibility.Visible : Visibility.Collapsed;
	//	}
	//}
	public override string Text { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

	protected override FrameworkElement RootElement => throw new NotImplementedException();

	protected override System.Windows.Controls.Control[] InputControls => throw new NotImplementedException();

	public override (int start, int end) GetSelectionBounds() => throw new NotImplementedException();
	public override bool IsCompatible(Windows.UI.Xaml.Controls.TextBox textBox) => throw new NotImplementedException();
	public override IDisposable ObserveTextChanges(EventHandler onChanged) => throw new NotImplementedException();
	public override void SetSelectionBounds(int start, int end) => throw new NotImplementedException();
}
