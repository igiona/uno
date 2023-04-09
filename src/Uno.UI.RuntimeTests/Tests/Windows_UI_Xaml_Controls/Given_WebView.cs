﻿using System;
using System.Threading.Tasks;
using Private.Infrastructure;
using Windows.UI.Xaml.Controls;

namespace Uno.UI.RuntimeTests.Tests.Windows_UI_Xaml_Controls;

#if !HAS_UNO || __ANDROID__ || __IOS__ || __MACOS__
[TestClass]
[RunsOnUIThread]
public class Given_WebView
{
	[TestMethod]
	public void When_Navigate()
	{
		var webView = new WebView();
		var uri = new Uri("https://bing.com");
		webView.Navigate(uri);
		Assert.IsNotNull(webView.Source);
		Assert.AreEqual("https://bing.com/", webView.Source.OriginalString);
		Assert.AreEqual("https://bing.com", uri.OriginalString);
	}

#if __ANDROID__ || __IOS__ || __MACOS__
	[TestMethod]
	public void When_NavigateWithHttpRequestMessage()
	{
		var webView = new WebView();
		var uri = new Uri("https://bing.com");
		webView.NavigateWithHttpRequestMessage(new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, uri));
		Assert.IsNotNull(webView.Source);
		Assert.AreEqual("https://bing.com/", webView.Source.OriginalString);
		Assert.AreEqual("https://bing.com", uri.OriginalString);
	}
#endif

	[TestMethod]
	public void When_NavigateToString()
	{
		var webView = new WebView();
		var uri = new Uri("https://bing.com");
		webView.Source = uri;

		Assert.AreEqual("https://bing.com/", webView.Source.OriginalString);
		Assert.AreEqual("https://bing.com", uri.OriginalString);

		webView.NavigateToString("<html></html>");
		Assert.IsNull(webView.Source);
	}

#if !HAS_UNO
	[TestMethod]
	public async Task When_InvokeScriptAsync()
	{
		var border = new Border();
		var webView = new WebView();
		webView.Width = 200;
		webView.Height = 200;
		border.Child = webView;
		TestServices.WindowHelper.WindowContent = border;
		bool navigated = false;
		await TestServices.WindowHelper.WaitForLoaded(border);
		webView.NavigationCompleted += (sender, e) => navigated = true;
		webView.NavigateToString("<html><body><div id='test' style='width: 100px; height: 100px; background-color: blue;' /></body></html>");
		await TestServices.WindowHelper.WaitFor(() => navigated);

		var color = await webView.InvokeScriptAsync("eval", new[] { "document.getElementById('test').style.backgroundColor.toString()" });
		Assert.AreEqual("blue", color);

		// Change color to red
		await webView.InvokeScriptAsync("eval", new[] { "document.getElementById('test').style.backgroundColor = 'red'" });
		color = await webView.InvokeScriptAsync("eval", new[] { "document.getElementById('test').style.backgroundColor.toString()" });

		Assert.AreEqual("red", color);
	}

	[TestMethod]
	public async Task When_InvokeScriptAsync_String()
	{
		var border = new Border();
		var webView = new WebView();
		webView.Width = 200;
		webView.Height = 200;
		border.Child = webView;
		TestServices.WindowHelper.WindowContent = border;
		bool navigated = false;
		await TestServices.WindowHelper.WaitForLoaded(border);
		webView.NavigationCompleted += (sender, e) => navigated = true;
		webView.NavigateToString("<html></html>");
		await TestServices.WindowHelper.WaitFor(() => navigated);
		var script = "(1 + 1).toString()";

		var result = await webView.InvokeScriptAsync("eval", new[] { script });
		Assert.AreEqual("2", result);
	}

	[TestMethod]
	public async Task When_InvokeScriptAsync_Non_String()
	{
		var border = new Border();
		var webView = new WebView();
		webView.Width = 200;
		webView.Height = 200;
		border.Child = webView;
		TestServices.WindowHelper.WindowContent = border;
		bool navigated = false;
		await TestServices.WindowHelper.WaitForLoaded(border);
		webView.NavigationCompleted += (sender, e) => navigated = true;
		webView.NavigateToString("<html></html>");
		await TestServices.WindowHelper.WaitFor(() => navigated);
		var script = "(1 + 1)";

		var result = await webView.InvokeScriptAsync("eval", new[] { script });
		Assert.AreEqual("", result);
	}
#endif
}
#endif
