using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace GeekayApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 
    public class MyImageData
    {
        public string Image
        {
            get;
            set;
        }
        public string Title
        {
            get;
            set;
        }
        public string Subtitle
        {
            get;
            set;
        }
        public string Description
        {
            get;
            set;
        }
    }

    public sealed partial class MenuPage : Page
    {


        public MenuPage()
        {
            this.InitializeComponent();
            ObservableCollection<MyImageData> ds = new ObservableCollection<MyImageData>();
            ds.Add(new MyImageData { Image = "ms-appx:///Assets/magnify.png", Title = "Scan a Game", Subtitle = "Subtitle 1", Description = "Description 1" });
            ds.Add(new MyImageData { Image = "ms-appx:///Assets/offers.png", Title = "Offers", Subtitle = "Subtitle 1", Description = "Description 1" });
            ds.Add(new MyImageData { Image = "ms-appx:///Assets/wlist.png", Title = "My Wishlist", Subtitle = "Subtitle 1", Description = "Description 1" });
            ds.Add(new MyImageData { Image = "ms-appx:///Assets/xbox.jpg", Title = "About", Subtitle = "Subtitle 1", Description = "Description 1" });

            menuList.ItemsSource = ds;

        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void ItemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Use e.AddedItems to get the items that are selected in the ItemsControl.
            //selectedItems = (List<object>)e.AddedItems;
            var data = e.AddedItems[0] as MyImageData;
            if(data.Title.Equals("Scan a Game"))
            {
                this.Frame.Navigate(typeof(MainPage));
            }
        }
    }
}
