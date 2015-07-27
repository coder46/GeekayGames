using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace GeekayApp
{
    class PayloadClass
    {
        public string gameTitle { get; set; }
        public BitmapImage Image1 { get; set; }
        public string pic1Url { get; set; }
        public string pic2Url { get; set; }
        public string gamePublisher { get; set; }
        public string datePub { get; set; }
        public string gameCats { get; set; }
        public string gameRated { get; set; }
        public double rating { get; set; }
        public string ratingVal { get; set; }

        public string trailerUrl { get; set; }
        public int game_id { get; set; }
        public int prodId { get; set; }

        

    }
}
