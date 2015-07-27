using Newtonsoft.Json;
using System;
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
using Windows.UI.Core;
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
    /// 

   
   

    public sealed partial class SearchEngine : Page
    {
        public SearchEngine()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private async void queryBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if(e.Key == Windows.System.VirtualKey.Enter)
            {
                Windows.UI.ViewManagement.InputPane.GetForCurrentView().TryHide();
                //myText.Text = queryBox.Text;
                LoadingBar.IsEnabled = true;
                LoadingBar.Visibility = Visibility.Visible;
            
                ObservableCollection<PayloadClass> ds2 = new ObservableCollection<PayloadClass>();

                try
                {
                    HttpClient client = new HttpClient();
                    string API_URL = "http://46.101.20.113:8000/games/search/" + queryBox.Text;
                    var response = await client.GetAsync(new Uri(API_URL));
                    client.Dispose();
                    response.EnsureSuccessStatusCode();
                    var result = await response.Content.ReadAsStringAsync();

                    if(result != "")
                    {
                        Debug.WriteLine(result);
                        
                        JsonValue jsonValue = JsonValue.Parse(result);
                        for(int i=0;i<jsonValue.GetArray().Count;i++)
                        {
                            IJsonValue ele = jsonValue.GetArray()[i];
                            Debug.WriteLine(ele.GetObject().GetNamedString("title"));
                            PayloadClass gamePL = new PayloadClass();
                            gamePL.gameTitle = ele.GetObject().GetNamedString("title");
                            gamePL.gamePublisher = ele.GetObject().GetNamedString("publisher");
                            Double rating = (ele.GetObject().GetNamedNumber("rating"));
                            rating = rating * 0.5;
                            gamePL.ratingVal = Math.Floor(rating).ToString();
                            gamePL.datePub = ele.GetObject().GetNamedString("publishedDate").Split('-')[0];
                            gamePL.pic2Url = ele.GetObject().GetNamedString("pic2Url");
                            gamePL.Image1 = new BitmapImage(new Uri(gamePL.pic2Url));

                            

                            ds2.Add(gamePL);
                            
                        }
                        
                    }
                }
                catch (HttpRequestException ex)
                {
                    LoadingBar.IsEnabled = false;
                    LoadingBar.Visibility = Visibility.Collapsed;

                    Debug.WriteLine(ex.Message.ToString());
                }
                
                

                gameList.ItemsSource = ds2;
                LoadingBar.IsEnabled = false;
                LoadingBar.Visibility = Visibility.Collapsed;


            }
        }

        private void gameListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Use e.AddedItems to get the items that are selected in the ItemsControl.
            //selectedItems = (List<object>)e.AddedItems;
            if (e.AddedItems.Count>0)
            {
                var data = e.AddedItems[0] as PayloadClass;
                Debug.WriteLine(data.gameTitle);

            }
            

        }

        private void gameList_Loaded(object sender, RoutedEventArgs e)
        {
            /*
        
            var gameList = (ListView)sender;
            Console.WriteLine("HELLO");
            ObservableCollection<GameListData> ds2 = new ObservableCollection<GameListData>();
            ds2.Add(new GameListData { Image1 = "ms-appx:///Assets/batman_icon.jpg", gameName1 = "Batman: Arkham City", pubName1 = "Warner Bros. Interactive Entertainment", yearRel1 = "2011", ratingVal1 = "2" });
            ds2.Add(new GameListData { Image1 = "ms-appx:///Assets/ac3_icon.jpg", gameName1 = "Assasins Creed 3", pubName1 = "Ubisoft", yearRel1 = "2013", ratingVal1 = "5" });
            ds2.Add(new GameListData { Image1 = "ms-appx:///Assets/fifa_icon.jpg", gameName1 = "Fifa 15", pubName1 = "EA Sports", yearRel1 = "2015", ratingVal1 = "3" });
            ds2.Add(new GameListData { Image1 = "ms-appx:///Assets/forza_icon.jpg", gameName1 = "Forza Motorsport 5", pubName1 = "Microsoft Studios", yearRel1 = "2013", ratingVal1 = "1" });
            gameList.ItemsSource = ds2;
            
             */

        }

        private void searchListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Use e.AddedItems to get the items that are selected in the ItemsControl.
            //selectedItems = (List<object>)e.AddedItems;
            var data = e.AddedItems[0] as GameListData;
            Console.WriteLine(data.gameName1);


        }

        private void searchList_Loaded(object sender, RoutedEventArgs e)
        {
            /*
        
            var gameList = (ListView)sender;
            Console.WriteLine("HELLO");
            ObservableCollection<GameListData> ds2 = new ObservableCollection<GameListData>();
            ds2.Add(new GameListData { Image1 = "ms-appx:///Assets/batman_icon.jpg", gameName1 = "Batman: Arkham City", pubName1 = "Warner Bros. Interactive Entertainment", yearRel1 = "2011", ratingVal1 = "2" });
            ds2.Add(new GameListData { Image1 = "ms-appx:///Assets/ac3_icon.jpg", gameName1 = "Assasins Creed 3", pubName1 = "Ubisoft", yearRel1 = "2013", ratingVal1 = "5" });
            ds2.Add(new GameListData { Image1 = "ms-appx:///Assets/fifa_icon.jpg", gameName1 = "Fifa 15", pubName1 = "EA Sports", yearRel1 = "2015", ratingVal1 = "3" });
            ds2.Add(new GameListData { Image1 = "ms-appx:///Assets/forza_icon.jpg", gameName1 = "Forza Motorsport 5", pubName1 = "Microsoft Studios", yearRel1 = "2013", ratingVal1 = "1" });
            gameList.ItemsSource = ds2;
            
             */

        }

    }
}
