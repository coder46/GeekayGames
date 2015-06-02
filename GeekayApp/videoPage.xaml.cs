using System;
using System.Collections.Generic;
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
using MyToolkit.Multimedia;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace GeekayApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class videoPage : Page
    {
        public int game_id;
        public string trailerUrl;
        public videoPage()
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
            PayloadClass payload = new PayloadClass();
            payload = e.Parameter as PayloadClass;
            gameTitle.Text = payload.gameTitle;
            gamePublisher.Text = payload.gamePublisher;
            datePub.Text = payload.datePub;
            gameCats.Text = payload.gameCats;
            gameRated.Text = payload.gameRated;
            ratingBox.Text = payload.rating.ToString();
            ratingBox.IsReadOnly = true;
            game_id = payload.game_id;
            trailerUrl = payload.trailerUrl;

            if (payload.game_id == 1)
            {

                player.PosterSource = new BitmapImage(new Uri("ms-appx:///Assets/fifa_16.jpg", UriKind.Absolute));
             
            }
            else if(payload.game_id == 2)
            {
                player.PosterSource = new BitmapImage(new Uri("ms-appx:///Assets/ac3.jpg", UriKind.Absolute));
             
            }
            else if (payload.game_id == 3)
            {
                player.PosterSource = new BitmapImage(new Uri("ms-appx:///Assets/batman.jpg", UriKind.Absolute));

            }
            else if (payload.game_id == 4)
            {
                player.PosterSource = new BitmapImage(new Uri("ms-appx:///Assets/forza5_1.jpg", UriKind.Absolute));

            }

        }
        
        private async void playBtn2_Click(object sender, RoutedEventArgs e)
        {
            string[] stringSeparators = new string[] { "?v=" };
            string[] result;
            result = trailerUrl.Split(stringSeparators, StringSplitOptions.None);
            var youTubeId = result[1];
            try
            {
                var url = await YouTube.GetVideoUriAsync(youTubeId, YouTubeQuality.Quality720P);
                if (url != null)
                {
                    player.Source = url.Uri;
                    player.Play();
                }
                else
                {

                }
            }
            catch
            {
                player.PosterSource = new BitmapImage(new Uri("ms-appx:///Assets/error.jpg", UriKind.Absolute));
            }
        }

        
    }
}
