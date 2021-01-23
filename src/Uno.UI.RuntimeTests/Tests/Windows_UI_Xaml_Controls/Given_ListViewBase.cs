﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Private.Infrastructure;
using Uno.UI.RuntimeTests.Tests.Windows_UI_Xaml_Controls.ListViewPages;
#if NETFX_CORE
using Uno.UI.Extensions;
#elif __IOS__
using UIKit;
#elif __MACOS__
using AppKit;
#else
using Uno.UI;
#endif
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using static Private.Infrastructure.TestServices;
using Windows.Foundation;

namespace Uno.UI.RuntimeTests.Tests.Windows_UI_Xaml_Controls
{
	[TestClass]
	[RunsOnUIThread]
	public partial class Given_ListViewBase
	{
		private ResourceDictionary _testsResources;

		private Style BasicContainerStyle => _testsResources["BasicListViewContainerStyle"] as Style;

		private Style ContainerMarginStyle => _testsResources["ListViewContainerMarginStyle"] as Style;

		private Style NoSpaceContainerStyle => _testsResources["NoExtraSpaceListViewContainerStyle"] as Style;

		private DataTemplate TextBlockItemTemplate => _testsResources["TextBlockItemTemplate"] as DataTemplate;

		private DataTemplate SelfHostingItemTemplate => _testsResources["SelfHostingItemTemplate"] as DataTemplate;

		private DataTemplate FixedSizeItemTemplate => _testsResources["FixedSizeItemTemplate"] as DataTemplate;

		private ItemsPanelTemplate NoCacheItemsStackPanel => _testsResources["NoCacheItemsStackPanel"] as ItemsPanelTemplate;

		[TestInitialize]
		public void Init()
		{
			_testsResources = new TestsResources();
		}

		[TestMethod]
		[RunsOnUIThread]
		public void ValidSelectionChange()
		{
			var source = Enumerable.Range(0, 10).ToArray();
			var list = new ListView { ItemsSource = source };
			list.SelectedItem = 3;
			Assert.AreEqual(list.SelectedItem, 3);
			list.SelectedItem = 5;
			Assert.AreEqual(list.SelectedItem, 5);
		}

		[TestMethod]
		[RunsOnUIThread]
		public void InvalidSelectionChangeValidPrevious()
		{
			var source = Enumerable.Range(0, 10).ToArray();
			var list = new ListView { ItemsSource = source };
			list.SelectedItem = 3;
			Assert.AreEqual(list.SelectedItem, 3);
			list.SelectedItem = 17;
			Assert.AreEqual(list.SelectedItem, 3);
		}

		[TestMethod]
		[RunsOnUIThread]
		public async Task ContainerIndicesAreUpdated()
		{
			var source = new ObservableCollection<string>();
			var SUT = new ListView { ItemsSource = source };
			WindowHelper.WindowContent = SUT;

			source.Add("test");

			await WindowHelper.WaitForIdle();

			source.Insert(0, "different");

			await WindowHelper.WaitForIdle();

#if HAS_UNO
			var containerIndices = SUT.MaterializedContainers
				.Select(container => container.GetValue(ItemsControl.IndexForItemContainerProperty))
				.OfType<int>()
				.OrderBy(index => index)
				.ToArray();

			CollectionAssert.AreEqual(new int[] { 0, 1 }, containerIndices);
#endif

			var container0 = SUT.ContainerFromIndex(0);
			var containerItem = SUT.ContainerFromItem("different");
			Assert.AreEqual(container0, containerItem);
		}

		[TestMethod]
		[RunsOnUIThread]
		public void InvalidSelectionChangeInvalidPrevious()
		{
			var source = Enumerable.Range(0, 10).ToArray();
			var list = new ListView { ItemsSource = source };
			list.SelectedItem = 3;
			Assert.AreEqual(list.SelectedItem, 3);
			source[3] = 13;
			list.SelectedItem = 17;
#if NETFX_CORE
			Assert.AreEqual(list.SelectedItem, 3);
#else
			Assert.IsNull(list.SelectedItem);
#endif
		}

		[TestMethod]
		[RunsOnUIThread]
		public async Task When_ContainerSet_Then_ContentShouldBeSet()
		{
			var resources = new TestsResources();

			var SUT = new ListView
			{
				ItemContainerStyle = BasicContainerStyle,
				ItemTemplate = TextBlockItemTemplate
			};

			WindowHelper.WindowContent = SUT;

			await WindowHelper.WaitForIdle();

			var source = new[] {
				"item 0",
			};

			SUT.ItemsSource = source;

			Assert.AreEqual(-1, SUT.SelectedIndex);

			SelectorItem si = null;
			await WindowHelper.WaitFor(() => (si = SUT.ContainerFromItem(source[0]) as SelectorItem) != null);

			var tb = si.FindFirstChild<TextBlock>();
			Assert.AreEqual("item 0", tb?.Text);
		}


		[TestMethod]
		[RunsOnUIThread]
		public async Task When_IsItsOwnItemContainer_FromSource()
		{
			var SUT = new ListView()
			{
				ItemContainerStyle = BasicContainerStyle,
				SelectionMode = ListViewSelectionMode.Single,
			};

			WindowHelper.WindowContent = SUT;
			await WindowHelper.WaitForIdle();

			var source = new[] {
				new ListViewItem(){ Content = "item 1" },
				new ListViewItem(){ Content = "item 2" },
				new ListViewItem(){ Content = "item 3" },
				new ListViewItem(){ Content = "item 4" },
			};

			SUT.ItemsSource = source;

			SelectorItem si = null;
			await WindowHelper.WaitFor(() => (si = SUT.ContainerFromItem(source[0]) as SelectorItem) != null);

			Assert.AreEqual("item 1", si.Content);
		}


		[TestMethod]
		[RunsOnUIThread]
		public async Task When_NoItemTemplate()
		{
			var SUT = new ListView()
			{
				ItemContainerStyle = BasicContainerStyle,
				ItemTemplate = null,
				ItemTemplateSelector = null,
			};

			WindowHelper.WindowContent = SUT;
			await WindowHelper.WaitForIdle();

			var source = new[] {
				"Item 1"
			};

			SUT.ItemsSource = source;

			SelectorItem si = null;
			await WindowHelper.WaitFor(() => (si = SUT.ContainerFromItem(source[0]) as SelectorItem) != null);

			Assert.AreEqual("Item 1", si.Content);
#if !NETFX_CORE // On iOS and Android (others not tested), ContentTemplateRoot is null, and TemplatedRoot is a ContentPresenter containing an ImplicitTextBlock
			return;
#endif

			Assert.IsInstanceOfType(si.ContentTemplateRoot, typeof(TextBlock));
			Assert.AreEqual("Item 1", (si.ContentTemplateRoot as TextBlock).Text);
		}

		[TestMethod]
		[RunsOnUIThread]
		public async Task When_IsItsOwnItemContainer_FromSource_With_DataTemplate()
		{
			var SUT = new ListView()
			{
				ItemContainerStyle = BasicContainerStyle,
				ItemTemplate = TextBlockItemTemplate,
				SelectionMode = ListViewSelectionMode.Single,
			};

			WindowHelper.WindowContent = SUT;
			await WindowHelper.WaitForIdle();

			var source = new object[] {
				new ListViewItem(){ Content = "item 1" },
				"item 2"
			};

			SUT.ItemsSource = source;

			SelectorItem si = null;
			await WindowHelper.WaitFor(() => (si = SUT.ContainerFromItem(source[0]) as SelectorItem) != null);
			Assert.AreEqual("item 1", si.Content);
			Assert.AreSame(si, source[0]);
#if !NETFX_CORE
			Assert.IsFalse(si.IsGeneratedContainer);
#endif

			var si2 = SUT.ContainerFromItem(source[1]) as ListViewItem;

			Assert.IsNotNull(si2);
			Assert.AreNotSame(si2, source[1]);
			Assert.AreEqual("item 2", si2.Content);
#if !NETFX_CORE
			Assert.AreEqual("item 2", si2.DataContext);
			Assert.IsTrue(si2.IsGeneratedContainer);
#endif
		}

		[TestMethod]
		[RunsOnUIThread]
		public async Task When_TemplateRoot_IsOwnContainer()
		{
			var SUT = new ListView()
			{
				ItemContainerStyle = BasicContainerStyle,
				ItemTemplate = SelfHostingItemTemplate,
				SelectionMode = ListViewSelectionMode.Single,
			};

			WindowHelper.WindowContent = SUT;
			await WindowHelper.WaitForIdle();

			var source = new object[]
			{
				"item 1",
				"item 2"
			};

			SUT.ItemsSource = source;

			ListViewItem lvi = null;
			await WindowHelper.WaitFor(() => (lvi = SUT.ContainerFromItem(source[0]) as ListViewItem) != null);

			Assert.IsNull(lvi.FindFirstChild<ListViewItem>(includeCurrent: false));
			Assert.IsNull(lvi.FindFirstParent<ListViewItem>(includeCurrent: false));
			Assert.AreEqual("SelfHostingListViewItem", lvi.Name);

			var content = lvi.Content as Border;
			Assert.IsNotNull(content);
			Assert.AreEqual("SelfHostingBorder", content.Name);
			Assert.AreEqual("item 1", (content.Child as TextBlock)?.Text);
		}

		[TestMethod]
		[RunsOnUIThread]
		public async Task SingleItemSelected()
		{
			var child1 = new ListViewItem
			{
				IsSelected = true,
				Content = "child 1"
			};

			var child2 = new ListViewItem
			{
				Content = "child 2"
			};

			var child3 = new ListViewItem
			{
				Content = "child 3"
			};

			var list = new ListView
			{
				SelectionMode = ListViewSelectionMode.Single
			};
			list.Items.Add(child1);
			list.Items.Add(child2);
			list.Items.Add(child3);

			var sut = new Grid
			{
				Children = { list }
			};

			TestServices.WindowHelper.WindowContent = sut;
			await TestServices.WindowHelper.WaitForIdle();


			Assert.AreEqual(list.SelectedIndex, 0);
			Assert.AreEqual(list.SelectedItem, child1);
		}

		[TestMethod]
		[RunsOnUIThread]
		public async Task MultipleItemsSelected()
		{
			var child1 = new ListViewItem
			{
				IsSelected = true,
				Content = "child 1"
			};

			var child2 = new ListViewItem
			{
				Content = "child 2"
			};

			var child3 = new ListViewItem
			{
				IsSelected = true,
				Content = "child 3"
			};

			var list = new ListView
			{
				SelectionMode = ListViewSelectionMode.Multiple
			};
			list.Items.Add(child1);
			list.Items.Add(child2);
			list.Items.Add(child3);

			var sut = new Grid
			{
				Children = { list }
			};

			TestServices.WindowHelper.WindowContent = sut;
			await TestServices.WindowHelper.WaitForIdle();


			Assert.AreEqual(list.SelectedItems[0], child1);
			Assert.AreEqual(list.SelectedItems[1], child3);
		}

		public async Task NoItemSelectedMultiple()
		{
			var child1 = new ListViewItem
			{
				Content = "child 1"
			};

			var child2 = new ListViewItem
			{
				Content = "child 2"
			};

			var child3 = new ListViewItem
			{
				Content = "child 3"
			};

			var list = new ListView
			{
				SelectionMode = ListViewSelectionMode.Multiple
			};
			list.Items.Add(child1);
			list.Items.Add(child2);
			list.Items.Add(child3);

			var sut = new Grid
			{
				Children = { list }
			};

			TestServices.WindowHelper.WindowContent = sut;
			await TestServices.WindowHelper.WaitForIdle();


			Assert.AreEqual(list.SelectedItems.Count, 0);
		}

		[TestMethod]
		[RunsOnUIThread]
		public async Task NoItemSelectedSingle()
		{
			var child1 = new ListViewItem
			{
				Content = "child 1"
			};

			var child2 = new ListViewItem
			{
				Content = "child 2"
			};

			var child3 = new ListViewItem
			{
				Content = "child 3"
			};

			var list = new ListView
			{
				SelectionMode = ListViewSelectionMode.Single
			};
			list.Items.Add(child1);
			list.Items.Add(child2);
			list.Items.Add(child3);

			var sut = new Grid
			{
				Children = { list }
			};

			TestServices.WindowHelper.WindowContent = sut;
			await TestServices.WindowHelper.WaitForIdle();


			Assert.AreEqual(list.SelectedIndex, -1);
		}

		[TestMethod]
		public async Task When_IsItsOwnItemContainer_Recycling()
		{
			var SUT = new ListView()
			{
				ItemContainerStyle = BasicContainerStyle,
				SelectionMode = ListViewSelectionMode.Single,
			};

			WindowHelper.WindowContent = SUT;
			await WindowHelper.WaitForIdle();

			var oldTwo = new ListViewItem() { Content = "item 2" };
			var source = new ObservableCollection<ListViewItem> {
				new ListViewItem(){ Content = "item 1" },
				oldTwo,
			};

			SUT.ItemsSource = source;

			SelectorItem si = null;
			await WindowHelper.WaitFor(() => (si = SUT.ContainerFromItem(source[0]) as SelectorItem) != null);

			Assert.AreEqual("item 1", si.Content);
			Assert.AreEqual(2, GetPanelChildren(SUT).Length);

			source.RemoveAt(1);

			await WindowHelper.WaitFor(() => GetPanelChildren(SUT).Length == 1);

			var newTwo = new ListViewItem { Content = "item 2" };
			Assert.AreNotEqual(oldTwo, newTwo);

			source.Add(newTwo);

			await WindowHelper.WaitFor(() => GetPanelChildren(SUT).Length == 2);
			Assert.AreEqual(newTwo, GetPanelChildren(SUT).Last());
		}

		[TestMethod]
		[RunsOnUIThread]
		public async Task When_Outer_ElementName_Binding()
		{
			var page = new ListViewPages.ListViewTemplateOuterBindingPage();

			for (int _ = 0; _ < 5; _++)
			{
				WindowHelper.WindowContent = page;
				await WindowHelper.WaitForIdle();

				var list = page.FindFirstChild<ListView>();
				Assert.IsNotNull(list);

				for (int i = 0; i < 3; i++)
				{
					ListViewItem lvi = null;
					await WindowHelper.WaitFor(() => (lvi = list.ContainerFromItem(i) as ListViewItem) != null);
					var sp = lvi.FindFirstChild<StackPanel>();
					var tb = sp?.FindFirstChild<TextBlock>();
					Assert.IsNotNull(tb);
					Assert.AreEqual("OuterContextText", tb.Text);
				}

				WindowHelper.WindowContent = null; // Unload page+list
				await WindowHelper.WaitForIdle();
				GC.Collect();
				GC.WaitForPendingFinalizers();
			}
		}

		[TestMethod]
		public async Task When_CollectionViewSource_In_Xaml()
		{
			var page = new ListViewCollectionViewSourcePage();

			Assert.AreEqual(0, page.SubjectListView.Items.Count);

			page.CVS.Source = new[] { "One", "Two", "Three" };

			WindowHelper.WindowContent = page;

			await WindowHelper.WaitForLoaded(page.SubjectListView);

			await WindowHelper.WaitForIdle();

#if NETFX_CORE // TODO: subscribe to changes to Source property
			Assert.AreEqual(3, page.SubjectListView.Items.Count);
#endif
			ListViewItem lvi = null;
			await WindowHelper.WaitFor(() => (lvi = page.SubjectListView.ContainerFromItem("One") as ListViewItem) != null);
		}

		private static ContentControl[] GetPanelChildren(ListViewBase list)
		{
#if __ANDROID__ || __IOS__
			return list.GetItemsPanelChildren().OfType<ContentControl>().ToArray();
#else
			return list.ItemsPanelRoot
				.Children
				.OfType<ContentControl>()
				.Where(c => c.Visibility == Visibility.Visible) // Managed ItemsStackPanel currently uses the dirty trick of leaving reyclable items attached to panel and collapsed
				.ToArray();
#endif
		}

		[TestMethod]
		public void When_Selection_SelectedValuePath_Set()
		{
			var SUT = new ListView();
			var source = new Dictionary<int, string>
			{
				{0, "Zero" },
				{1, "One" },
				{2, "Two" }
			};
			SUT.ItemsSource = source;
			SUT.SelectedValuePath = "Key";

			Assert.AreEqual(null, SUT.SelectedValue);
			Assert.AreEqual(null, SUT.SelectedItem);
			Assert.AreEqual(-1, SUT.SelectedIndex);

			SUT.SelectedValue = 1;

			var item1 = source.First(kvp => kvp.Key == 1);
			Assert.AreEqual(1, SUT.SelectedValue);
			Assert.AreEqual(item1, SUT.SelectedItem);
			Assert.AreEqual(1, SUT.SelectedIndex);

			// Set invalid
			SUT.SelectedValue = 4;

			Assert.AreEqual(null, SUT.SelectedValue);
			Assert.AreEqual(null, SUT.SelectedItem);
			Assert.AreEqual(-1, SUT.SelectedIndex);
		}

		[TestMethod]
		public void When_Selection_SelectedValue_Path_Not_Set()
		{
			var SUT = new ListView();
			var source = new List<string>
			{
				"Zero",
				"One",
				"Two",
			};
			SUT.ItemsSource = source;

			Assert.AreEqual(null, SUT.SelectedValue);
			Assert.AreEqual(null, SUT.SelectedItem);
			Assert.AreEqual(-1, SUT.SelectedIndex);

			SUT.SelectedValue = "Two";

			Assert.AreEqual("Two", SUT.SelectedValue);
			Assert.AreEqual("Two", SUT.SelectedItem);
			Assert.AreEqual(2, SUT.SelectedIndex);

			SUT.SelectedValue = "Eleventy";

			Assert.AreEqual(null, SUT.SelectedValue);
			Assert.AreEqual(null, SUT.SelectedItem);
			Assert.AreEqual(-1, SUT.SelectedIndex);
		}

		[TestMethod]
		public async Task When_Scrolled_To_End_And_Last_Item_Removed()
		{
			var container = new Grid { Height = 210 };

			var list = new ListView
			{
				ItemContainerStyle = NoSpaceContainerStyle,
				ItemTemplate = FixedSizeItemTemplate
			};
			container.Children.Add(list);

			var source = new ObservableCollection<int>(Enumerable.Range(0, 20));
			list.ItemsSource = source;

			WindowHelper.WindowContent = container;
			await WindowHelper.WaitForLoaded(list);

			ScrollBy(list, 10000); // Scroll to end

			ListViewItem lastItem = null;
			await WindowHelper.WaitFor(() => (lastItem = list.ContainerFromItem(19) as ListViewItem) != null);
			var secondLastItem = list.ContainerFromItem(18) as ListViewItem;

			await WindowHelper.WaitFor(() => ApproxEquals(181, GetTop(lastItem)), message: $"Expected 181 but got {GetTop(lastItem)}");
			await WindowHelper.WaitFor(() => ApproxEquals(152, GetTop(secondLastItem)), message: $"Expected 152 but got {GetTop(secondLastItem)}");

			source.Remove(19);

			await WindowHelper.WaitFor(() => list.Items.Count == 19);

			await WindowHelper.WaitFor(() => ApproxEquals(181, GetTop(secondLastItem)), message: $"Expected 181 but got {GetTop(secondLastItem)}");

			double GetTop(FrameworkElement element)
			{
				var transform = element.TransformToVisual(container);
				return transform.TransformPoint(new Point()).Y;
			}
		}

		[TestMethod]
		[RunsOnUIThread]
		public async Task When_Items_Their_Own_Container()
		{
			var list = new OnItemsChangedListView();
			var items = new ObservableCollection<ListViewItem>()
			{
				new ListViewItem(),
				new ListViewItem(),
				new ListViewItem(),
				new ListViewItem(),
			};

			list.ItemsSource = items;
			WindowHelper.WindowContent = list;
			await WindowHelper.WaitForLoaded(list);

			// Containers/indices/items can be retrieved
			Assert.AreEqual(items[1], list.ContainerFromItem(items[1]));
			Assert.AreEqual(items[2], list.ContainerFromIndex(2));
			Assert.AreEqual(3, list.IndexFromContainer(items[3]));
			Assert.AreEqual(items[1], list.ItemFromContainer(items[1]));
		}

		[TestMethod]
		[RunsOnUIThread]
		public async Task When_Items_Their_Own_Container_In_OnItemsChanged_Removal()
		{
			var list = new OnItemsChangedListView();
			var items = new ObservableCollection<ListViewItem>()
			{
				new ListViewItem(),
				new ListViewItem(),
				new ListViewItem(),
				new ListViewItem(),
			};

			list.ItemsSource = items;
			WindowHelper.WindowContent = list;
			await WindowHelper.WaitForLoaded(list);

			// Item removal

			var removedItem = items[1];
			list.ItemsChangedAction = () =>
			{
				// Test container/index/item before removed
				Assert.AreEqual(items[0], list.ContainerFromItem(items[0]));
				Assert.AreEqual(items[0], list.ContainerFromIndex(0));
				Assert.AreEqual(items[0], list.ItemFromContainer(items[0]));
				Assert.AreEqual(0, list.IndexFromContainer(items[0]));

				// Test removed container/index/item
				Assert.AreEqual(null, list.ContainerFromItem(removedItem));
				// In UWP, the Item is returned even though it is already removed
				// This is a weird behavior and doesn't seem too useful anyway, so we currently
				// ignore it
				// Assert.AreEqual(removedItem, list.ItemFromContainer(removedItem));
				Assert.AreEqual(-1, list.IndexFromContainer(removedItem));

				// Test container/index/item right after removed
				Assert.AreEqual(items[1], list.ContainerFromItem(items[1]));
				Assert.AreEqual(items[1], list.ContainerFromIndex(1));
				Assert.AreEqual(items[1], list.ItemFromContainer(items[1]));
				Assert.AreEqual(1, list.IndexFromContainer(items[1]));

				// Test container/index/item after removed
				Assert.AreEqual(items[2], list.ContainerFromItem(items[2]));
				Assert.AreEqual(items[2], list.ContainerFromIndex(2));
				Assert.AreEqual(items[2], list.ItemFromContainer(items[2]));
				Assert.AreEqual(2, list.IndexFromContainer(items[2]));
			};

			items.Remove(removedItem);
		}

		[TestMethod]
		[RunsOnUIThread]
		public async Task When_Items_Their_Own_Container_In_OnItemsChanged_Addition()
		{
			var list = new OnItemsChangedListView();
			var items = new ObservableCollection<ListViewItem>()
			{
				new ListViewItem(),
				new ListViewItem(),
				new ListViewItem(),
				new ListViewItem(),
			};

			list.ItemsSource = items;
			WindowHelper.WindowContent = list;
			await WindowHelper.WaitForLoaded(list);

			// Item removal

			var addedItem = new ListViewItem();
			list.ItemsChangedAction = () =>
			{
				// Test container/index/item before added
				Assert.AreEqual(items[0], list.ContainerFromItem(items[0]));
				Assert.AreEqual(items[0], list.ContainerFromIndex(0));
				Assert.AreEqual(items[0], list.ItemFromContainer(items[0]));
				Assert.AreEqual(0, list.IndexFromContainer(items[0]));

				// Test added container/index/item
#if HAS_UNO
				// UWP returns null/-1 here, which differs from "the same"
				// situation in case of collection change. For simplicity
				// we return the correct values here too. It should not have
				// any adverse impact.
				Assert.AreEqual(addedItem, list.ContainerFromItem(addedItem));
				Assert.AreEqual(addedItem, list.ContainerFromIndex(1));
				Assert.AreEqual(addedItem, list.ItemFromContainer(addedItem));
				Assert.AreEqual(1, list.IndexFromContainer(addedItem));
#endif

				// Test container/index/item right after added
				Assert.AreEqual(items[2], list.ContainerFromItem(items[2]));
				Assert.AreEqual(items[2], list.ContainerFromIndex(2));
				Assert.AreEqual(items[2], list.ItemFromContainer(items[2]));
				Assert.AreEqual(2, list.IndexFromContainer(items[2]));

				// Test container/index/item after removed
				Assert.AreEqual(items[3], list.ContainerFromItem(items[3]));
				Assert.AreEqual(items[3], list.ContainerFromIndex(3));
				Assert.AreEqual(items[3], list.ItemFromContainer(items[3]));
				Assert.AreEqual(3, list.IndexFromContainer(items[3]));
			};

			items.Insert(1, addedItem);
		}

		[TestMethod]
		[RunsOnUIThread]
		public async Task When_Items_Their_Own_Container_In_OnItemsChanged_Change()
		{
			var list = new OnItemsChangedListView();
			var items = new ObservableCollection<ListViewItem>()
			{
				new ListViewItem(),
				new ListViewItem(),
				new ListViewItem(),
				new ListViewItem(),
			};

			list.ItemsSource = items;
			WindowHelper.WindowContent = list;
			await WindowHelper.WaitForLoaded(list);

			// Item change
			var oldItem = items[1];
			var newItem = new ListViewItem();

			list.ItemsChangedAction = () =>
			{
				// Test container/index/item before removed
				Assert.AreEqual(items[0], list.ContainerFromItem(items[0]));
				Assert.AreEqual(items[0], list.ContainerFromIndex(0));
				Assert.AreEqual(items[0], list.ItemFromContainer(items[0]));
				Assert.AreEqual(0, list.IndexFromContainer(items[0]));

				// Test old container/index/item
				Assert.AreEqual(null, list.ContainerFromItem(oldItem));
				Assert.AreEqual(null, list.ItemFromContainer(oldItem));
				Assert.AreEqual(-1, list.IndexFromContainer(oldItem));

				// Test new container/index/item
				Assert.AreEqual(newItem, list.ContainerFromItem(newItem));
				Assert.AreEqual(newItem, list.ContainerFromIndex(1));
				Assert.AreEqual(newItem, list.ItemFromContainer(newItem));
				Assert.AreEqual(1, list.IndexFromContainer(newItem));

				// Test container/index/item right after changed
				Assert.AreEqual(items[2], list.ContainerFromItem(items[2]));
				Assert.AreEqual(items[2], list.ContainerFromIndex(2));
				Assert.AreEqual(items[2], list.ItemFromContainer(items[2]));
				Assert.AreEqual(2, list.IndexFromContainer(items[2]));

				// Test container/index/item after changed
				Assert.AreEqual(items[3], list.ContainerFromItem(items[3]));
				Assert.AreEqual(items[3], list.ContainerFromIndex(3));
				Assert.AreEqual(items[3], list.ItemFromContainer(items[3]));
				Assert.AreEqual(3, list.IndexFromContainer(items[3]));
			};

			items[1] = newItem;
		}

		[TestMethod]
		[RunsOnUIThread]
		public async Task When_Items_Their_Own_Container_In_OnItemsChanged_Reset()
		{
			var list = new OnItemsChangedListView();
			var items = new ObservableCollection<ListViewItem>()
			{
				new ListViewItem(),
				new ListViewItem(),
				new ListViewItem(),
				new ListViewItem(),
			};

			list.ItemsSource = items;
			WindowHelper.WindowContent = list;
			await WindowHelper.WaitForLoaded(list);

			// Item change
			var newItems = new ObservableCollection<ListViewItem>()
			{
				new ListViewItem(),
				new ListViewItem(),
				new ListViewItem(),
				new ListViewItem(),
			};

			list.ItemsChangedAction = () =>
			{
				// Test container/index/item from old source
				Assert.AreEqual(null, list.ContainerFromItem(items[1]));
				Assert.AreEqual(null, list.ItemFromContainer(items[1]));
				Assert.AreEqual(-1, list.IndexFromContainer(items[1]));

				// Test container/index/item from new source
#if HAS_UNO
				// UWP returns null/-1 here, which differs from "the same"
				// situation in case of collection change. For simplicity
				// we return the correct values here too. It should not have
				// any adverse impact.
				Assert.AreEqual(newItems[1], list.ContainerFromItem(newItems[1]));
				Assert.AreEqual(newItems[1], list.ContainerFromIndex(1));
				Assert.AreEqual(newItems[1], list.ItemFromContainer(newItems[1]));
				Assert.AreEqual(1, list.IndexFromContainer(newItems[1]));
#endif
			};

			list.ItemsSource = newItems;
		}

		public partial class OnItemsChangedListView : ListView
		{
			public Action ItemsChangedAction = null;

			protected override void OnItemsChanged(object e)
			{
				base.OnItemsChanged(e);
				ItemsChangedAction?.Invoke();
			}
		}

		private bool ApproxEquals(double value1, double value2) => Math.Abs(value1 - value2) <= 2;
	}
}
