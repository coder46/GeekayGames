﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace GeekayApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            this.InitializeComponent();
            DrawerLayout.InitializeDrawerLayout();

            string[] menuItems = new string[5] { "Search a Game", "Scan a Game", "Wish List", "Settings", "Sign Out" };
            ListMenuItems.ItemsSource = menuItems.ToList();

            
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            //Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;

            ObservableCollection<NewGameListData> ds2 = new ObservableCollection<NewGameListData>();
            ObservableCollection<NewGameListData> ds3 = new ObservableCollection<NewGameListData>();
            ObservableCollection<BannerListData> ds4 = new ObservableCollection<BannerListData>();

            HttpClient client = new HttpClient();
                
            try
            {
                string API_URL = "http://46.101.20.113:8000/games/homescreen/newgames";
                string API_URL2 = "http://46.101.20.113:8000/games/homescreen/topgames";
                
                var response = await client.GetAsync(new Uri(API_URL));
                var response2 = await client.GetAsync(new Uri(API_URL2));

                //client.Dispose();
                response.EnsureSuccessStatusCode();
                response2.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                var result2 = await response2.Content.ReadAsStringAsync();

                if (result != "")
                {
                    Debug.WriteLine(result);
                    JsonValue jsonValue = JsonValue.Parse(result);
                    int len = jsonValue.GetArray().Count;
                    if (len > 5)
                    {
                        len = 5;
                    }

                    for (int i = 0; i < len; i++)
                    {
                        try
                        {
                            IJsonValue ele = jsonValue.GetArray()[i];
                            int prodId = Convert.ToInt32(ele.GetObject().GetNamedNumber("prodId"));
                            API_URL = "http://46.101.20.113:8000/games/" + prodId.ToString();
                            var res = await client.GetAsync(new Uri(API_URL));
                            res.EnsureSuccessStatusCode();
                            var RESULT = await res.Content.ReadAsStringAsync();
                            if (RESULT != "")
                            {
                                JsonValue jv = JsonValue.Parse(RESULT);
                                string imgUrl = jv.GetObject().GetNamedString("pic2Url");
                                NewGameListData ngData = new NewGameListData();
                                Debug.WriteLine(imgUrl);
                                ngData.Image1 = new BitmapImage(new Uri(imgUrl));
                                ngData.prodId = prodId;
                                ds2.Add(ngData);
                            }
                        }
                        catch (HttpRequestException ex3)
                        {
                            Debug.WriteLine(ex3.Message.ToString());
                        }



                    }
                }

                if (result2 != "")
                {
                    Debug.WriteLine(result2);
                    JsonValue jsonValue = JsonValue.Parse(result2);
                    int len = jsonValue.GetArray().Count;
                    if (len > 5)
                    {
                        len = 5;
                    }

                    for (int i = 0; i < len; i++)
                    {
                        try
                        {
                            IJsonValue ele = jsonValue.GetArray()[i];
                            int prodId = Convert.ToInt32(ele.GetObject().GetNamedNumber("prodId"));
                            API_URL = "http://46.101.20.113:8000/games/" + prodId.ToString();
                            var res = await client.GetAsync(new Uri(API_URL));
                            res.EnsureSuccessStatusCode();
                            var RESULT = await res.Content.ReadAsStringAsync();
                            if (RESULT != "")
                            {
                                JsonValue jv = JsonValue.Parse(RESULT);
                                string imgUrl = jv.GetObject().GetNamedString("pic2Url");
                                NewGameListData ngData = new NewGameListData();
                                Debug.WriteLine(imgUrl);
                                ngData.Image1 = new BitmapImage(new Uri(imgUrl));
                                ngData.prodId = prodId;
                                ds3.Add(ngData);
                            }
                        }
                        catch (HttpRequestException ex3)
                        {
                            Debug.WriteLine(ex3.Message.ToString());
                        }



                    }
                }

            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine(ex.Message.ToString());
            }

            //Banner part

            try
            {
                string API_URL = "http://46.101.20.113:8000/games/homescreen/banners";

                var response = await client.GetAsync(new Uri(API_URL));

                //client.Dispose();
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();

                if (result != "")
                {
                    Debug.WriteLine(result);
                    JsonValue jv = JsonValue.Parse(result);
                    int len = jv.GetArray().Count;
                    if(len > 5)
                    {
                        len = 5;
                    }
                    for (int i = 0; i < len; i++)
                    {
                        IJsonValue ele = jv.GetArray()[i];
                        string imgPath = ele.GetObject().GetNamedString("imgPath");
                        Debug.WriteLine(imgPath);
                        BannerListData bannerData = new BannerListData();
                        bannerData.Image1 = new BitmapImage(new Uri(imgPath));
                        ds4.Add(bannerData);
                    }
                    
                }

            }
            catch (HttpRequestException ex4)
            {
                Debug.WriteLine(ex4.Message.ToString());
            }

            NewGamesList.ItemsSource = ds2;
            TopGamesList.ItemsSource = ds3;
            BannersList.ItemsSource = ds4;
            //bannerImg.Source = new BitmapImage(new Uri("http://www.geekaygames.com/a/home/images/banner-ps4-console.jpg"));
        }

        void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            if (DrawerLayout.IsDrawerOpen)
            {
                DrawerLayout.CloseDrawer();
                e.Handled = true;
            }
            else
            {
                Application.Current.Exit();
            }
        }  

        private void DrawerIcon_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (DrawerLayout.IsDrawerOpen)
                DrawerLayout.CloseDrawer();
            else
                DrawerLayout.OpenDrawer();
        }

        private void ListMenuItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var data = e.AddedItems[0] as String;

            if(data.Equals("Search a Game"))
            {
                this.Frame.Navigate(typeof(SearchEngine));
            }

            else if (data.Equals("Scan a Game"))
            {
                this.Frame.Navigate(typeof(MainPage));
            }

        }   


    }
}
