using System;
using System.Collections.Generic;
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
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace GeekayApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProductDesc : Page
    {
       
        private DispatcherTimer dispatcherTimer;
        public ProductDesc()
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
            LoadingBar.IsEnabled = true;
            LoadingBar.Visibility = Visibility.Visible;
            var TOKEN = e.Parameter as string;
            //desc_block.Text = desc.PROD_DESC;
            dispatcherTimer = new DispatcherTimer();
            //dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Tick += (s, args) => dispatcherTimer_Tick(TOKEN);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 3);
            dispatcherTimer.Start();
                        
            Debug.WriteLine("Testing asyc");
        }

        private async void dispatcherTimer_Tick(string TOKEN)
        {

            HttpClient client2 = new HttpClient();
            //client2.BaseAddress = new Uri("https://api.cloudsightapi.com/");
            client2.DefaultRequestHeaders.Add("Authorization", "CloudSight YqKiVgtYuWJwatAkkhtDsw");
            //do whatever you want to do here
            try
            {
                //TOKEN = "5as9FMuR4p3cjDjCbHz1dA";
                //var getResponse = await client2.GetAsync("image_response/5as9FMuR4p3cjDjCbHz1dA");
                var getResponse = await client2.GetAsync(new Uri("https://api.cloudsightapi.com/image_responses/" + TOKEN));
                //client2.Dispose();
                getResponse.EnsureSuccessStatusCode();
                var getRESULT = getResponse.Content.ReadAsStringAsync().Result;
                JsonValue jsonValue = JsonValue.Parse(getRESULT);
                var STATUS = jsonValue.GetObject().GetNamedString("status");
                Debug.WriteLine(STATUS);

                TextBlock statusTB = new TextBlock();
                statusTB.Text = STATUS;
                statusTB.FontSize = 18;
                statusView.Items.Add(statusTB);

                if (STATUS == "completed")
                {
                    dispatcherTimer.Stop();
                    Debug.WriteLine(getRESULT);
                    var PRODUCT_DESC = jsonValue.GetObject().GetNamedString("name");
                    Debug.WriteLine(PRODUCT_DESC.ToLower());
                    desc_block.Text = PRODUCT_DESC;

                    //Now start searching product in database


                    string API_URL = "http://46.101.20.113:8000/games/9";
                    //API_URL = "http://127.0.0.1:8000/games/9/";
                    //API_URL = "http://azuregeekay.cloudapp.net:8000/games/9/";
                    int game_id = 3;
                    
                    if (PRODUCT_DESC.ToLower().Contains("fifa"))
                    {
                        API_URL = "http://46.101.20.113:8000/games/7";
                        //API_URL = "http://azuregeekay.cloudapp.net:8000/games/7/";
                        game_id = 1;
                    }

                    if (PRODUCT_DESC.ToLower().Contains("creed"))
                    {
                        game_id = 2;
                        API_URL = "http://46.101.20.113:8000/games/8";
                        //API_URL = "http://azuregeekay.cloudapp.net:8000/games/8/";
                    
                    }

                    if (PRODUCT_DESC.ToLower().Contains("batman"))
                    {
                        game_id = 3;
                        API_URL = "http://46.101.20.113:8000/games/9";
                        //API_URL = "http://azuregeekay.cloudapp.net:8000/games/9/";
                    
                    }

                    if (PRODUCT_DESC.ToLower().Contains("forza"))
                    {
                        game_id = 4;
                        API_URL = "http://46.101.20.113:8000/games/10";
                        //API_URL = "http://azuregeekay.cloudapp.net:8000/games/10/";
                    
                    }

                    if(game_id > 0)
                    {
                        Debug.WriteLine(API_URL);
                        //var client = new HttpClient();
                        //client.BaseAddress = new Uri("http://46.101.20.113:8000/games");
                        try
                        {
                            client2.DefaultRequestHeaders.Remove("Authorization");
                            var response = await client2.GetAsync(new Uri(API_URL));
                            client2.Dispose();
                            response.EnsureSuccessStatusCode();
                            Debug.WriteLine("Received Response");
                            var result = await response.Content.ReadAsStringAsync();

                            if (result != "")
                            {
                                Debug.WriteLine("Entered Result");
                                Debug.WriteLine(result);
                                LoadingBar.IsEnabled = false;
                                LoadingBar.Visibility = Visibility.Collapsed;

                                JsonValue jsonValue2 = JsonValue.Parse(result);
                                PayloadClass payload = new PayloadClass();
                                payload.game_id = game_id;
                                payload.gameTitle = jsonValue2.GetObject().GetNamedString("title");
                                payload.gamePublisher = jsonValue2.GetObject().GetNamedString("publisher");
                                payload.datePub = jsonValue2.GetObject().GetNamedString("publishedDate");
                                payload.gameCats = jsonValue2.GetObject().GetNamedString("gameCats");
                                payload.gameRated = jsonValue2.GetObject().GetNamedString("ratedInfo");
                                payload.rating = jsonValue2.GetObject().GetNamedNumber("rating");
                                payload.trailerUrl = jsonValue2.GetObject().GetNamedString("trailerUrl");
                                //this.Frame.Navigate(typeof(videoPage), payload);
                                this.Frame.Navigate(typeof(GameInfoPage), payload);
                            
                            }

                        }
                        catch (HttpRequestException ex3)
                        {
                            Debug.WriteLine(ex3.Message.ToString());
                        }
                        
                    }
                    Debug.WriteLine("Reached End");
                    LoadingBar.IsEnabled = false;
                    LoadingBar.Visibility = Visibility.Collapsed;
                    
                    
                    
                }

            }
            catch (HttpRequestException ex2)
            {
                Debug.WriteLine(ex2.Message.ToString());
            }
        }

    }
}
