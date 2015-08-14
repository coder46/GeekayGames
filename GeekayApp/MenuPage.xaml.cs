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
    /*
     * =================================================
     * -------------------------------------------------
     * METRO STYLE VERSION OF MENU PAGE
     * -------------------------------------------------
     * This page represents the MenuPage when users enter 
     * the App for the first time. Menu Items include:
     * => Scan a game -> Goes to -> MainPage.xaml 
     * => Search a game -> Goes to -> SearchEngine.xaml
     * => Offers
     * => My Wishlist
     * => HomePage (DrawerLayout Version) -> Goes to -> HomePage.xaml
     * =================================================
     */


    /*
     * ------------------------------------------------
     * MenuData class stores data for Menu list
     * ------------------------------------------------
     */
                       
    public class MenuData
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

            /// ObservableCollection is used to store ListView items
            
            ObservableCollection<MenuData> ds = new ObservableCollection<MenuData>();
            ds.Add(new MenuData { Image = "ms-appx:///Assets/magnify.png", Title = "Scan a Game", Subtitle = "Subtitle 1", Description = "Description 1" });
            ds.Add(new MenuData { Image = "ms-appx:///Assets/xbox.jpg", Title = "Search a Game", Subtitle = "Subtitle 1", Description = "Description 1" });
            ds.Add(new MenuData { Image = "ms-appx:///Assets/offers.png", Title = "Offers", Subtitle = "Subtitle 1", Description = "Description 1" });
            ds.Add(new MenuData { Image = "ms-appx:///Assets/wlist.png", Title = "My Wishlist", Subtitle = "Subtitle 1", Description = "Description 1" });
            ds.Add(new MenuData { Image = "ms-appx:///Assets/wlist.png", Title = "Home Page", Subtitle = "Subtitle 1", Description = "Description 1" });
            
            /// ItemsSource property of ListView is set to the ObservableCollection object

            menuList.ItemsSource = ds;

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        /*
         * ----------------------------------------------------------
         * ItemListView_SelectionChanged invoked when user
         * presses any item on menuList. Clicked item stored
         * in e.AddedItems[0]
         * ----------------------------------------------------------
         */
        private void ItemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Use e.AddedItems to get the items that are selected in the ItemsControl.
            var data = e.AddedItems[0] as MenuData;
            if(data.Title.Equals("Scan a Game"))
            {
                this.Frame.Navigate(typeof(MainPage));
            }

            else if(data.Title.Equals("Search a Game"))
            {
                this.Frame.Navigate(typeof(SearchEngine));  
            }
            
            else if (data.Title.Equals("Home Page"))
            {
                this.Frame.Navigate(typeof(HomePage));
            }

        }
    }
}
