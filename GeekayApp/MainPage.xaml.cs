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
using System.Net;
using System.Runtime.Serialization.Json;
using System.Net.Http;
using Windows.Data.Json;
using Windows.Media.Capture;
using Windows.Media.Devices;
using Windows.Devices;
using Windows.Devices.Enumeration;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Graphics.Display;
using Windows.Storage.Streams;
using System.Diagnostics;


namespace GeekayApp
{

    public sealed partial class MainPage : Page
    {
        bool frontCam;
        public MediaCapture mediaCapture;
        public MainPage()
        {
            this.InitializeComponent();
            
            this.NavigationCacheMode = NavigationCacheMode.Required;

        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            mediaCapture = new MediaCapture();
            DeviceInformationCollection devices =
        await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);

            // Use the front camera if found one
            if (devices == null) return;
            DeviceInformation info = devices[0];

            
            foreach (var devInfo in devices)
            {
                if (devInfo.Name.ToLowerInvariant().Contains("back"))
                {
                    info = devInfo;
                    frontCam = false;
                    continue;
                }
            }
            
            await mediaCapture.InitializeAsync(
                new MediaCaptureInitializationSettings
                {
                    VideoDeviceId = info.Id
                });

            CaptureElement.Source = mediaCapture;
            CaptureElement.FlowDirection = frontCam ?
        FlowDirection.RightToLeft : FlowDirection.LeftToRight;
            await mediaCapture.StartPreviewAsync();

            DisplayInformation displayInfo = DisplayInformation.GetForCurrentView();
            displayInfo.OrientationChanged += DisplayInfo_OrientationChanged;

            DisplayInfo_OrientationChanged(displayInfo, null);

        }
        protected override async void OnNavigatedFrom(NavigationEventArgs e)
        {
            BtnCapturePhoto.IsEnabled = true;
            // Release resources
            if (mediaCapture != null)
            {
                await mediaCapture.StopPreviewAsync();
                CaptureElement.Source = null;
                mediaCapture.Dispose();
                mediaCapture = null;
            }
        }
        private void DisplayInfo_OrientationChanged(DisplayInformation sender, object args)
        {
            if (mediaCapture != null)
            {
                mediaCapture.SetPreviewRotation(frontCam
                ? VideoRotationLookup(sender.CurrentOrientation, true)
                : VideoRotationLookup(sender.CurrentOrientation, false));
                var rotation = VideoRotationLookup(sender.CurrentOrientation, false);
                mediaCapture.SetRecordRotation(rotation);
            }
        }

        private VideoRotation VideoRotationLookup(DisplayOrientations displayOrientation, bool counterclockwise)
        {
            switch (displayOrientation)
            {
                case DisplayOrientations.Landscape:
                    return VideoRotation.None;

                case DisplayOrientations.Portrait:
                    return (counterclockwise) ? VideoRotation.Clockwise270Degrees : VideoRotation.Clockwise90Degrees;

                case DisplayOrientations.LandscapeFlipped:
                    return VideoRotation.Clockwise180Degrees;

                case DisplayOrientations.PortraitFlipped:
                    return (counterclockwise) ? VideoRotation.Clockwise90Degrees :
                    VideoRotation.Clockwise270Degrees;

                default:
                    return VideoRotation.None;
            }
        }

        
        private async void capture_Click(object sender, RoutedEventArgs e)
        {
            BtnCapturePhoto.IsEnabled = false;
            
            string desiredName = "photo4.jpg";
            ImageEncodingProperties imageProperties = ImageEncodingProperties.CreateJpeg();
            var photoStorageFile = await KnownFolders.PicturesLibrary.CreateFileAsync(desiredName, CreationCollisionOption.GenerateUniqueName);
            await mediaCapture.CapturePhotoToStorageFileAsync(imageProperties, photoStorageFile);

            if (mediaCapture != null)
            {
                await mediaCapture.StopPreviewAsync();
                CaptureElement.Source = null;
                mediaCapture.Dispose();
                mediaCapture = null;
            }

            
            var bitmap = new BitmapImage();
            await bitmap.SetSourceAsync(await photoStorageFile.OpenReadAsync());
            //var bitmap = new BitmapImage(new Uri("ms-appx:///Assets/photo.jpg", UriKind.Absolute));
            int pixWidth = bitmap.PixelWidth;
            int pixHeight = bitmap.PixelHeight;
            ImageProps props = new ImageProps();
            props.pixelWidth = pixWidth;
            props.pixelHeight = pixHeight;
            
            /*
            bitmap = null;
            //BtnCapturePhoto.IsEnabled = true;

            // load a jpeg, be sure to have the Pictures Library capability in your manifest
            var folder = KnownFolders.PicturesLibrary;
            var file = await folder.GetFileAsync("photo.jpg");
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
            var wb = new WriteableBitmap(pixWidth, pixHeight);
            ms.Seek(0);
            
            // load the writable bitpamp from the stream
            await wb.SetSourceAsync(ms);
            Debug.WriteLine("Width: "+wb.PixelWidth.ToString());
            Debug.WriteLine("Height: " + wb.PixelHeight.ToString());
            Debug.WriteLine("Width: " + pixWidth.ToString());
            Debug.WriteLine("Height: " + pixHeight.ToString());
            
            //var cropped = wb.Rotate(90).Crop(100, 173, 201, 260);
            var cropped = wb.Rotate(90);
            //wb = null;
            Debug.WriteLine("Width: " + cropped.PixelWidth.ToString());
            Debug.WriteLine("Height: " + cropped.PixelHeight.ToString());
            //cropped = cropped.Crop(0,0,0,0);
            var c = cropped.Crop(new  Rect(0, 0, 10, 10));
            Debug.WriteLine("Cropped_Width: " + c.PixelWidth.ToString());
            Debug.WriteLine("Cropped_Height: " + c.PixelHeight.ToString());
            
            */


            this.Frame.Navigate(typeof(PhotoReview),bitmap);
            
            //this.Frame.Navigate(typeof(PhotoReview), c);
            //this.Frame.Navigate(typeof(NextStep), props);
            
            
        }

       
    }
}
