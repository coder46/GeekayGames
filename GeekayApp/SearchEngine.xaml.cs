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
                myText.Text = queryBox.Text;

                ObservableCollection<GameListData> ds2 = new ObservableCollection<GameListData>();

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
                        myText.Text = result;

                        var json = JsonArray.Parse(result);
                        foreach ( var dataItem in json)
                        {
                            Debug.WriteLine(dataItem.GetObject().GetNamedString("title"));
                            GameListData gameData = new GameListData();


                            gameData.gameName1 = dataItem.GetObject().GetNamedString("title");
                            gameData.pubName1 = dataItem.GetObject().GetNamedString("publisher");
                            //Double rating = Convert.ToDouble(dataItem.GetObject().GetNamedString("rating").ToString());
                            Double rating = 3.0;
                            rating = rating * 0.5;
                            gameData.ratingVal1 = Math.Ceiling(rating).ToString();
                            gameData.yearRel1 = dataItem.GetObject().GetNamedString("publishedDate").Split('-')[0];

                            //gameData.Image1 = "ms-appx:///Assets/batman_icon.jpg";
                            BitmapImage img = new BitmapImage(new Uri(dataItem.GetObject().GetNamedString("pic2Url")));
                            gameData.Image1 = img;
                            /*
                            string gameId = dataItem.GetObject().GetNamedNumber("id").ToString();
                            if (gameId.Equals("10"))
                            {
                                gameData.Image1 = "ms-appx:///Assets/forza_icon.jpg";
                            }
                            else if (gameId.Equals("4"))
                            {
                                gameData.Image1 = "ms-appx:///Assets/fifa_icon.jpg";
                            }
                            else if (gameId.Equals("7"))
                            {
                                gameData.Image1 = "ms-appx:///Assets/fifa_icon.jpg";
                            }
                            else if (gameId.Equals("9"))
                            {
                                gameData.Image1 = "ms-appx:///Assets/batman_icon.jpg";
                            }
                            else if (gameId.Equals("8"))
                            {
                                gameData.Image1 = "ms-appx:///Assets/ac3_icon.jpg";
                            }
                            */
                            ds2.Add(gameData);
                        }

                        /*
                        dynamic dynJson = JsonConvert.DeserializeObject(result);
                        PayloadClass payload = new PayloadClass();
                        foreach (var item in dynJson)
                        {
                            GameListData gameData = new GameListData();
                            Debug.WriteLine(item.title);
                            
                            gameData.gameName1 = item.title;
                            gameData.pubName1 = item.publisher;
                            Double rating = Convert.ToDouble(item.rating.toString());
                            rating = rating * 0.5;
                            gameData.ratingVal1 = Math.Ceiling(rating).ToString();
                            gameData.yearRel1 = item.publishedDate.Split('-')[0];

                            gameData.Image1 = "ms-appx:///Assets/batman_icon.jpg";
                            string gameId = item.id.ToString();
                            if (gameId.Equals("10"))
                            {
                                gameData.Image1 = "ms-appx:///Assets/forza_icon.jpg";
                            }
                            else if (gameId.Equals("4"))
                            {
                                gameData.Image1 = "ms-appx:///Assets/fifa_icon.jpg";
                            }
                            else if (gameId.Equals("7"))
                            {
                                gameData.Image1 = "ms-appx:///Assets/fifa_icon.jpg";
                            }
                            else if (gameId.Equals("9"))
                            {
                                gameData.Image1 = "ms-appx:///Assets/batman_icon.jpg";
                            }
                            else if (gameId.Equals("8"))
                            {
                                gameData.Image1 = "ms-appx:///Assets/ac3_icon.jpg";
                            }

                            ds2.Add(gameData);
                            
                        }
                         */
                        
                        
                    }
                }
                catch (HttpRequestException ex)
                {
                    Debug.WriteLine(ex.Message.ToString());
                }
                
                /*
                ds2.Add(new GameListData { Image1 = "ms-appx:///Assets/batman_icon.jpg", gameName1 = "Batman: Arkham City", pubName1 = "Warner Bros. Interactive Entertainment", yearRel1 = "2011", ratingVal1 = "2" });
                ds2.Add(new GameListData { Image1 = "ms-appx:///Assets/ac3_icon.jpg", gameName1 = "Assasins Creed 3", pubName1 = "Ubisoft", yearRel1 = "2013", ratingVal1 = "5" });
                ds2.Add(new GameListData { Image1 = "ms-appx:///Assets/fifa_icon.jpg", gameName1 = "Fifa 15", pubName1 = "EA Sports", yearRel1 = "2015", ratingVal1 = "3" });
                ds2.Add(new GameListData { Image1 = "ms-appx:///Assets/forza_icon.jpg", gameName1 = "Forza Motorsport 5", pubName1 = "Microsoft Studios", yearRel1 = "2013", ratingVal1 = "1" });
                */
                searchList.ItemsSource = ds2;

            }
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
