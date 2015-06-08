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
            var TOKEN = e.Parameter as string;
            //desc_block.Text = desc.PROD_DESC;
            dispatcherTimer = new DispatcherTimer();
            //dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Tick += (s, args) => dispatcherTimer_Tick(TOKEN);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 3);
            dispatcherTimer.Start();
        }

        private async void dispatcherTimer_Tick(string TOKEN)
        {

            HttpClient client2 = new HttpClient();
            client2.BaseAddress = new Uri("https://api.cloudsightapi.com/");
            client2.DefaultRequestHeaders.Add("Authorization", "CloudSight YqKiVgtYuWJwatAkkhtDsw");
            //do whatever you want to do here
            try
            {
                //TOKEN = "5as9FMuR4p3cjDjCbHz1dA";
                //var getResponse = await client2.GetAsync("image_response/5as9FMuR4p3cjDjCbHz1dA");
                var getResponse = await client2.GetAsync(new Uri("https://api.cloudsightapi.com/image_responses/" + TOKEN));
                client2.Dispose();
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
                    Debug.WriteLine(getRESULT);
                    var PRODUCT_DESC = jsonValue.GetObject().GetNamedString("name");
                    Debug.WriteLine(PRODUCT_DESC);
                    desc_block.Text = PRODUCT_DESC;
                    dispatcherTimer.Stop();
                    //this.Frame.Navigate(typeof(ProductDesc), Prod_Resp);

                }

            }
            catch (HttpRequestException ex2)
            {
                Debug.WriteLine(ex2.Message.ToString());
            }
        }

    }
}
