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
                            gamePL.trailerUrl = ele.GetObject().GetNamedString("trailerUrl");
                            gamePL.Image1 = new BitmapImage(new Uri(gamePL.pic2Url));
                            gamePL.gameCats = ele.GetObject().GetNamedString("gameCats");
                            
                            gamePL.game_id = Convert.ToInt32(ele.GetObject().GetNamedNumber("id"));

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
                //Debug.WriteLine(data.gameCats.Split(new String[] {"/"},StringSplitOptions.None)[0]);
                this.Frame.Navigate(typeof(GameInfoPage), data);

            }
            

        }

        private void gameList_Loaded(object sender, RoutedEventArgs e)
        {
          

        }

     

    }
}
