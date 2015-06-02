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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Media.Capture;
using Windows.Media.Devices;
using Windows.Devices;
using Windows.Devices.Enumeration;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace GeekayApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PhotoReview : Page
    {
        public PhotoReview()
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
            var bitmap = e.Parameter as BitmapImage;
            //var wb = e.Parameter as WriteableBitmap;
            //var cropped = wb.Rotate(90).Crop(0, 0, 10, 10);
            //var cropped = wb.Rotate(90);
            
            //gameImg.Source = cropped.RotateFree(270);
            //wb = wb.RotateFree(270);
            //gameImg.Width = wb.PixelWidth;
            //gameImg.Height = wb.PixelHeight;
            //gameImg.Source = wb;

            
            gameImg.Source = bitmap;


   
            
        }

        private void Retake_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(GameList));
        }
    }
}
