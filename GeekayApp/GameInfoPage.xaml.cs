using MyToolkit.Multimedia;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace GeekayApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 

    


    public sealed partial class GameInfoPage : Page
    {
        public int game_id;
        public string trailerUrl;
        ImageBrush ImgSrc;
        PayloadClass payload;
        
            
        public GameInfoPage()
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
            payload = e.Parameter as PayloadClass;
            GameHub.Header = payload.gameTitle;
            trailerUrl = payload.trailerUrl;

            /*
            TextBlock gameName = FindChildControl<TextBlock>(HS1, "gameName") as TextBlock;
            gameName.Text = null;
            gameName.Text = payload.gameTitle;
            var pubName = FindChildControl<TextBlock>(HS1, "pubName") as TextBlock;
            pubName.Text = payload.gamePublisher;
            var yearRel = FindChildControl<TextBlock>(HS1, "yearRel") as TextBlock;
            yearRel.Text = payload.datePub;
            trailerUrl = payload.trailerUrl;
            */
            MediaElement player = FindChildControl<MediaElement>(HS1, "player") as MediaElement;

            ImgSrc = new ImageBrush();
            ImgSrc.Stretch = Stretch.UniformToFill;
            ImgSrc.Opacity = 0.15;

            if (payload.game_id == 1)
            {

                ImgSrc.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/fifa_16.jpg", UriKind.Absolute));
                
                //GInfoPage.Background = ImgSrc;
            }
            else if (payload.game_id == 2)
            {
                ImgSrc.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/ac3.jpg", UriKind.Absolute));

            }
            else if (payload.game_id == 3)
            {
                //player.PosterSource = new BitmapImage(new Uri("ms-appx:///Assets/batman.jpg", UriKind.Absolute));
                ImgSrc.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/batman.jpg", UriKind.Absolute));
            }
            else if (payload.game_id == 4)
            {
                ImgSrc.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/forza5_1.jpg", UriKind.Absolute));

            }

            //ImgSrc.ImageSource = player.PosterSource;
            GInfoPage.Background = ImgSrc;


            //code for "related" hubsection

           



        }
        
        private DependencyObject FindChildControl<T>(DependencyObject control, string ctrlName)
        {
            int childNumber = VisualTreeHelper.GetChildrenCount(control);
            for (int i = 0; i < childNumber; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(control, i);
                FrameworkElement fe = child as FrameworkElement;
                // Not a framework element or is null
                if (fe == null) return null;
                Debug.WriteLine(fe);
                if (child is T && fe.Name == ctrlName)
                {
                    // Found the control so return
                    return child;
                }
                else
                {
                    // Not found it - search children
                    DependencyObject nextLevel = FindChildControl<T>(child, ctrlName);
                    if (nextLevel != null)
                        return nextLevel;
                }
            }
            return null;
        }

        private async void playBtn2_Click(object sender, RoutedEventArgs e)
        {
           
            //trailerUrl = "https://www.youtube.com/watch?v=8lxqoXeft1U";
            string[] stringSeparators = new string[] { "?v=" };
            string[] result;
            result = trailerUrl.Split(stringSeparators, StringSplitOptions.None);
            var youTubeId = result[1];

            MediaElement player = FindChildControl<MediaElement>(HS1, "player") as MediaElement;


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

        private void gameName_Loaded(object sender, RoutedEventArgs e)
        {
            var gameName = (TextBlock)sender;
            gameName.Text = payload.gameTitle;
        }

        private void player_Loaded(object sender, RoutedEventArgs e)
        {
            var player = (MediaElement)sender;
            player.PosterSource = ImgSrc.ImageSource;
            
        }

        private void gameImg_Loaded(object sender, RoutedEventArgs e)
        {
            var gameImg = (Image)sender;
            if (payload.game_id == 1)
            {

                gameImg.Source = new BitmapImage(new Uri("ms-appx:///Assets/fifa_icon.jpg", UriKind.Absolute));
                
            }
            else if (payload.game_id == 2)
            {
                gameImg.Source = new BitmapImage(new Uri("ms-appx:///Assets/ac3_icon.jpg", UriKind.Absolute));
                
            }
            else if (payload.game_id == 3)
            {
                gameImg.Source = new BitmapImage(new Uri("ms-appx:///Assets/batman_icon.jpg", UriKind.Absolute));
            }
            else if (payload.game_id == 4)
            {
                gameImg.Source = new BitmapImage(new Uri("ms-appx:///Assets/forza_icon.jpg", UriKind.Absolute));
                
            }

        }

        private void pubName_Loaded(object sender, RoutedEventArgs e)
        {
            var pubName = (TextBlock)sender;
            pubName.Text = payload.gamePublisher;
        }

        private void yearRel_Loaded(object sender, RoutedEventArgs e)
        {
            var yearRel = (TextBlock)sender;

            string[] stringSeparators = new string[] { "-" };
            //string[] result;
            
            yearRel.Text = payload.datePub.Split(stringSeparators, StringSplitOptions.None)[0];
        }

        private void gameListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Use e.AddedItems to get the items that are selected in the ItemsControl.
            //selectedItems = (List<object>)e.AddedItems;
            var data = e.AddedItems[0] as GameListData;
            Console.WriteLine(data.gameName1);


        }

        private void gameList_Loaded(object sender, RoutedEventArgs e)
        {
            var gameList = (ListView)sender;
            Console.WriteLine("HELLO");
            ObservableCollection<GameListData> ds2 = new ObservableCollection<GameListData>();
            /*
            ds2.Add(new GameListData { Image1 = "ms-appx:///Assets/batman_icon.jpg", gameName1 = "Batman: Arkham City", pubName1 = "Warner Bros. Interactive Entertainment", yearRel1 = "2011", ratingVal1 = "2" });
            ds2.Add(new GameListData { Image1 = "ms-appx:///Assets/ac3_icon.jpg", gameName1 = "Assasins Creed 3", pubName1 = "Ubisoft", yearRel1 = "2013", ratingVal1 = "5" });
            ds2.Add(new GameListData { Image1 = "ms-appx:///Assets/fifa_icon.jpg", gameName1 = "Fifa 15", pubName1 = "EA Sports", yearRel1 = "2015", ratingVal1 = "3" });
            ds2.Add(new GameListData { Image1 = "ms-appx:///Assets/forza_icon.jpg", gameName1 = "Forza Motorsport 5", pubName1 = "Microsoft Studios", yearRel1 = "2013", ratingVal1 = "1" });

            */
            
            gameList.ItemsSource = ds2;

        }

    }
}
