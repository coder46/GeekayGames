using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
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
    public sealed partial class NextStep : Page
    {
        public NextStep()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var props = e.Parameter as ImageProps;

            // load a jpeg, be sure to have the Pictures Library capability in your manifest
            var folder = KnownFolders.PicturesLibrary;
            var file = await folder.GetFileAsync("photo4.jpg");
            var data = await FileIO.ReadBufferAsync(file);

            // create a stream from the file
            var ms = new InMemoryRandomAccessStream();
            var dw = new Windows.Storage.Streams.DataWriter(ms);
            dw.WriteBuffer(data);
            await dw.StoreAsync();
            ms.Seek(0);

            // find out how big the image is, don't need this if you already know
            //var bm = new BitmapImage();
            //await bm.SetSourceAsync(ms);

            // create a writable bitmap of the right size
            //var wb = new WriteableBitmap(props.pixelWidth, props.pixelHeight);
            var wb = new WriteableBitmap(1000, 1000);
            
            ms.Seek(0);

            // load the writable bitpamp from the stream
            await wb.SetSourceAsync(ms);
            Debug.WriteLine("Width: " + wb.PixelWidth.ToString());
            Debug.WriteLine("Height: " + wb.PixelHeight.ToString());
            Debug.WriteLine("Width: " + props.pixelWidth.ToString());
            Debug.WriteLine("Height: " + props.pixelHeight.ToString());

            //var cropped = wb.Rotate(90).Crop(100, 173, 201, 260);
            var cropped = wb.Rotate(90);
            //wb = null;
            Debug.WriteLine("Width: " + cropped.PixelWidth.ToString());
            Debug.WriteLine("Height: " + cropped.PixelHeight.ToString());
            //cropped = cropped.Crop(0,0,0,0);
            var c = cropped.Crop(new Rect(0, 0, 1000, 1000));
            //var c = cropped.Crop(new Rect(100, 173, 201, 260));
            
            Debug.WriteLine("Cropped_Width: " + c.PixelWidth.ToString());
            Debug.WriteLine("Cropped_Height: " + c.PixelHeight.ToString());
            this.Frame.Navigate(typeof(PhotoReview), c);
            
        }
    }
}
