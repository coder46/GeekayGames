using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Data.Json;
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
    public sealed partial class GameList : Page
    {
        public GameList()
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
            
            TextBlock tb2 = new TextBlock();
            tb2.Text = "FIFA 16";
            tb2.FontSize = 25;
            tb2.Tag = 1;
            GamesListView.Items.Add(tb2);
            
            TextBlock tb3 = new TextBlock();
            tb3.Text = "Assasins Creed 3";
            tb3.FontSize = 25;
            tb3.Tag = 2;
            GamesListView.Items.Add(tb3);
            
            TextBlock tb4 = new TextBlock();
            tb4.Text = "Batman: Arkham City";
            tb4.FontSize = 25;
            tb4.Tag = 3;
            GamesListView.Items.Add(tb4);
            
            TextBlock tb5 = new TextBlock();
            tb5.Text = "Forza Motorsport 5";
            tb5.FontSize = 25;
            tb5.Tag = 4;
            GamesListView.Items.Add(tb5);            

        }

        
        private async void GamesListView_ItemClick(object sender, ItemClickEventArgs e)
        {

            //string API_URL = "http://128.199.120.231:8000/games/7";
            //string API_URL = "http://127.0.0.1:8000/games/7";
            string API_URL = "http://46.101.20.113:8000/games/10";
                
            int game_id;
            //Accessing the server
            game_id = 0;
            TextBlock item = e.ClickedItem as TextBlock;
            if (item.Tag.Equals(1) == true)
            {
                API_URL = "http://46.101.20.113:8000/games/7";
                //API_URL = "http://128.199.120.231:8000/games/7";
                //API_URL = "http://127.0.0.1:8000/games/7/";
                game_id = 1;
            }
            else if (item.Tag.Equals(2) == true)
            {
                API_URL = "http://46.101.20.113:8000/games/8";
                //API_URL = "http://128.199.120.231:8000/games/8";
                //API_URL = "http://127.0.0.1:8000/games/8";
                game_id = 2;
            }
            else if (item.Tag.Equals(3) == true)
            {
                API_URL = "http://46.101.20.113:8000/games/9";
                //API_URL = "http://128.199.120.231:8000/games/7";
                
                //API_URL = "http://127.0.0.1:8000/games/9";
                game_id = 3;
            }
            else if (item.Tag.Equals(4) == true)
            {
                API_URL = "http://46.101.20.113:8000/games/10";
                //API_URL = "http://128.199.120.231:8000/games/7";
                //API_URL = "http://127.0.0.1:8000/games/10";
                game_id = 4;

            }
            var client = new HttpClient();
            var response = await client.GetAsync(new Uri(API_URL));
            var result = await response.Content.ReadAsStringAsync();
            if (result != "")
            {
                JsonValue jsonValue = JsonValue.Parse(result);
                PayloadClass payload = new PayloadClass();
                payload.game_id = game_id;
                payload.gameTitle = jsonValue.GetObject().GetNamedString("title");
                payload.gamePublisher = jsonValue.GetObject().GetNamedString("publisher");
                payload.datePub = jsonValue.GetObject().GetNamedString("publishedDate");
                payload.gameCats = jsonValue.GetObject().GetNamedString("gameCats");
                payload.gameRated = jsonValue.GetObject().GetNamedString("ratedInfo");
                payload.rating = jsonValue.GetObject().GetNamedNumber("rating");
                payload.trailerUrl = jsonValue.GetObject().GetNamedString("trailerUrl");
                //this.Frame.Navigate(typeof(videoPage), payload);
                this.Frame.Navigate(typeof(GameInfoPage), payload);
            
            }
            
        }
    }
} 
