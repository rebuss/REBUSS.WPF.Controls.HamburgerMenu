using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace REBUSS.WPF.Controls.HamburgerMenu
{
    internal class ItemController
    {
        private readonly IList<HamburgerMenuItem> items;

        internal Action<HamburgerMenuItem> SelectedItemChanged;

        internal ItemController()
        {
            items = new List<HamburgerMenuItem>();
        }

        internal IReadOnlyCollection<HamburgerMenuItem> HamburgerMenuItems
            => (IReadOnlyCollection<HamburgerMenuItem>) items;

        internal void AddItem(HamburgerMenuItem item)
        {
            if (item != null)
            {
                items.Add(item);
                item.Checked += OnItemChecked;
            }
        }

        internal void InjectData(IList<ItemFeed> feeds)
        {
            if (feeds?.Any() == true)
            {
                foreach (var item in items)
                {
                    var feed = feeds.FirstOrDefault(f => f.Key.Equals(item.DataContext) || f.Key.Equals(item.GetHashCode()));
                    item.UpdateWith(feed);
                }
            }
        }

        // TODO remove handler
        private void OnItemChecked(object sender, RoutedEventArgs e)
        {
            var selectedItem = sender as HamburgerMenuItem;
            if (selectedItem != null)
            {
                SelectedItemChanged(sender as HamburgerMenuItem);
                foreach (var item in items)
                {
                    if (!item.Equals(selectedItem))
                    {
                        item.SetCurrentValue(ToggleButton.IsCheckedProperty, false);
                    } 
                }
            }
        }
    }
}