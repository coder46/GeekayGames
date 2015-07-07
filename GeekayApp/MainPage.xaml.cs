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
using System.Net.Http.Headers;
using System.Drawing;
using Windows.Storage.FileProperties;
using System.Collections.Specialized;



namespace GeekayApp
{

    public sealed partial class MainPage : Page
    {
        bool frontCam;
        public MediaCapture mediaCapture;
        
        private ProductPayload Prod_Resp;
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
            BtnCapturePhoto.Visibility = Visibility.Visible;
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

        byte[] ConvertBitmapToByteArray(WriteableBitmap bitmap)
        {
            using (Stream stream = bitmap.PixelBuffer.AsStream())
            using (MemoryStream memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }



        private async void capture_Click(object sender, RoutedEventArgs e)
        {
            LoadingRing.IsEnabled = true;
            LoadingRing.Visibility = Visibility.Visible;
            

            BtnCapturePhoto.IsEnabled = false;
            BtnCapturePhoto.Visibility = Visibility.Visible;
            string desiredName = "photo14.jpg";
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
            
            YellowRect.Visibility = Visibility.Collapsed;
            LoadingRing.IsActive = true;

            
            //this.Frame.Navigate(typeof(PhotoReview), bitmap);
            

             
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://api.cloudsightapi.com/");
            client.DefaultRequestHeaders.Add("Authorization", "CloudSight YqKiVgtYuWJwatAkkhtDsw");
            MultipartFormDataContent form = new MultipartFormDataContent();
            form.Add(new StringContent("en-US"), "image_request[locale]");
            
            //HttpContent content = new StringContent(imgString);
            //form.Add(content, "image_request[image]");

            var imgStream = await photoStorageFile.OpenStreamForReadAsync();
            HttpContent imgContent = new StreamContent(imgStream);

            //Compressing the Image
            
            ImageProperties myProps = await photoStorageFile.Properties.GetImagePropertiesAsync();
            
            WriteableBitmap bmp = new WriteableBitmap((int)myProps.Width, (int)myProps.Height);
            
            bmp.SetSource((await photoStorageFile.OpenReadAsync()));
            
            
            var rotated = bmp.Rotate(90);
            var resized = bmp.Resize(700, 1000, WriteableBitmapExtensions.Interpolation.Bilinear);

            //this.Frame.Navigate(typeof(PhotoReview), resized);
            
            var fileStream = new MemoryStream();
            var newArray = rotated.ToByteArray();
            fileStream.Write(newArray, 0, newArray.Length);

            //imgContent = new StreamContent(fileStream);
            //imgContent = new ByteArrayContent(newArray);
            
            imgContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "image_request[image]",
                FileName = "image.jpg"
            };
            form.Add(imgContent);
            string RESULT = "";

            try
            {
                var response = await client.PostAsync("image_requests", form);
                response.EnsureSuccessStatusCode();
                client.Dispose();
                RESULT = response.Content.ReadAsStringAsync().Result;
                Debug.WriteLine("GET TOKEN "+RESULT);
            
            }
            catch(HttpRequestException ex)
            {
                Debug.WriteLine(ex.Message.ToString());
            }
            
            
            if (RESULT != "")
            {
                JsonValue jsonValue = JsonValue.Parse(RESULT);
                string TOKEN = jsonValue.GetObject().GetNamedString("token");
                Debug.WriteLine(TOKEN);
                LoadingRing.IsActive = false;
                LoadingRing.IsEnabled = false;
                LoadingRing.Visibility = Visibility.Collapsed;
               

                this.Frame.Navigate(typeof(ProductDesc),TOKEN);
                
            }
            //this.Frame.Navigate(typeof(PhotoReview),cropped);
            //this.Frame.Navigate(typeof(PhotoReview), c);
            //this.Frame.Navigate(typeof(NextStep), props);


        }

        


    }   
    
}
