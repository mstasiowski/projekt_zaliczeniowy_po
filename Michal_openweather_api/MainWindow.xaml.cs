using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net;

namespace Michal_openweather_api
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }


        /// <summary>
        /// INSERT MY APIKEY
        /// </summary>
        string APIKEY = "907faf56db9342f237f4d1acebd67c0e";


        /// <summary>
        /// That is, the class of coordinates includes longitude and latitude
        /// </summary>
        public class coord
        {
            
           public double lon { get; set; }
           public double lat { get; set; }
        }

        /// <summary>
        /// This is the weather class contains the main, description of the weather and the weather icon
        /// </summary>
        public class weather
        {
            public string main { get; set; }
            public string description { get; set; }
            public string icon { get; set; }
        }
        /// <summary>
        /// That is, the main class includes the current temperature, air pressure, humidity and perceptible temperature
        /// </summary>
        public class main
        {
           public double temp { get; set; }
           public double pressure { get; set; }
           public double humidity { get; set; }
           public double feels_like { get; set; }

        }
        /// <summary>
        ///  That is the wind class includes wind speed in m/s
        /// </summary>
        public class wind
        {
           public double speed { get; set; }
        }

        /// <summary>
        /// The sys class contains sunrise and sunset at a specific location
        /// </summary>
        public class sys
        {
           public long sunrise { get; set; }
           public long sunset { get; set; }
        }

        /// <summary>
        /// The root class is a container that contains all available classes in the current weather api
        /// </summary>
        public class root
        {
            public coord coord { get; set; }

            /// <summary>
            /// The weather list includes main, description, and icon
            /// </summary>
            public List <weather> weather { get; set; }
            public main main { get; set; }
            public wind wind { get; set; }

            /// <summary>
            /// dt contains time of data calculation, unix, UTC
            /// </summary>
            public long dt { get; set; }
            public sys sys { get; set; }

            /// <summary>
            /// name contains City name
            /// </summary>
            public string name { get; set; }


        }

        /// <summary>
        /// gets the photo data from the link
        /// </summary>
        /// <param name="id">Icon string from weather class</param>
        /// <returns>image</returns>
        public static BitmapImage addImg(string id)
        {
            var path = $"http://openweathermap.org/img/w/{id}.png";

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(path, UriKind.Absolute);
            bitmap.EndInit();

            return bitmap;
        }


        public double lat { get; set; }
        public double lon { get; set; }



        /// <summary>
        /// Gets and returns data from the current weather api to the form
        /// </summary>
        void getWeather()
        {
           
            
            using (WebClient web = new WebClient())
            {
                ///<summary>
                ///check if website url is valid if returns valid returns true if not returns false
                /// </summary>
                bool checkWebsite(string URL)
                {
                    try
                    {
                        WebClient wc = new WebClient();
                        string HTMLSource = wc.DownloadString(URL);
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
                
                ///<summary>
                ///current weather api url
                /// </summary>
                string url = $"https://api.openweathermap.org/data/2.5/weather?q={searchbox.Text}&appid={APIKEY}&units=metric";

                ///<summary>checking website</summary>
                bool websiteExists = checkWebsite(url);

                    ///<summary>
                    ///If website is valid returns data else return messagebox not valid city becouse valid city is parameter of url
                    ///</summary>
                if(websiteExists)
                {

                    var json = web.DownloadString(url);

                    root root = JsonSerializer.Deserialize<root>(json);

                    var tempp = Math.Round(root.main.temp);

                    tempboxnow.Text = tempp.ToString() + "°C";
                    weatherimagenow.Source = addImg(root.weather[0].icon);
                    datenow.Text = convertDateTime(root.dt).ToString("dddd, dd MMMM HH:mmmm ");
                    weatherdescnow.Text = root.weather[0].description.ToString();
                     
                    string cityToUp = searchbox.Text.ToUpper();
                    citybox.Text = cityToUp;

                    latitudeblocknow.Text = Math.Round(root.coord.lat, 2).ToString();
                    longitudeblocknow.Text = Math.Round(root.coord.lon, 2).ToString();
                    windblocknow.Text = root.wind.speed.ToString() + " m/s";
                    humidityblocknow.Text = root.main.humidity.ToString() + " %";
                    pressureblocknow.Text = root.main.pressure.ToString() + " hPa";
                    sunriseblocknow.Text = convertDateTime(root.sys.sunrise).ToShortTimeString();
                    sunsetblocknow.Text = convertDateTime(root.sys.sunset).ToShortTimeString();

                    var feelslike = Math.Round(root.main.feels_like);
                    feelslikeblocknow.Text = feelslike.ToString() + "°C";

                    



                    DateTime convertDateTime(long seconds)
                    {
                        DateTime day = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                        day = day.AddSeconds(seconds).ToLocalTime();
                        return day;
                    }

                    lat = root.coord.lat;
                    lon = root.coord.lon;



                }
                else
                {
                    MessageBox.Show("Wrong city");
                }



            }
        }

        /// <summary>
        /// Gets and returns data from the one call api to the form
        /// </summary>
        void getForecast()
        {
            using (WebClient web = new WebClient())
            {
                string url = $"https://api.openweathermap.org/data/2.5/onecall?lat={lat}&lon={lon}&exclude=currently,minutely,hourly,alerts&appid={APIKEY}&units=metric";
                var json = web.DownloadString(url);

                forecastInfo forecastInfo = JsonSerializer.Deserialize<forecastInfo>(json);



                datetime1.Text = convertDateTime(forecastInfo.daily[1].dt).ToLongDateString();
                var temp1r = Math.Round(forecastInfo.daily[1].temp.day); 
                temp1.Text =temp1r.ToString() + "°C";
                weatherimage1.Source = addImg(forecastInfo.daily[1].weather[0].icon);
                weatherblockdesc1.Text = forecastInfo.daily[1].weather[0].description.ToString();
                tempmaxblock1.Text =Math.Round(forecastInfo.daily[1].temp.max).ToString() + "°C/";
                tempminblock1.Text =Math.Round(forecastInfo.daily[1].temp.min).ToString() + "°C";


                datetime2.Text = convertDateTime(forecastInfo.daily[2].dt).ToLongDateString();
                var temp2r = Math.Round(forecastInfo.daily[2].temp.day);
                temp2.Text = temp1r.ToString() + "°C";
                weatherimage2.Source = addImg(forecastInfo.daily[2].weather[0].icon);
                weatherblockdesc2.Text = forecastInfo.daily[2].weather[0].description.ToString();
                tempmaxblock2.Text = Math.Round(forecastInfo.daily[2].temp.max).ToString() + "°C/";
                tempminblock2.Text = Math.Round(forecastInfo.daily[2].temp.min).ToString() + "°C";


                datetime3.Text = convertDateTime(forecastInfo.daily[3].dt).ToLongDateString();
                var temp3r = Math.Round(forecastInfo.daily[3].temp.day);
                temp3.Text = temp1r.ToString() + "°C";
                weatherimage3.Source = addImg(forecastInfo.daily[3].weather[0].icon);
                weatherblockdesc3.Text = forecastInfo.daily[3].weather[0].description.ToString();
                tempmaxblock3.Text = Math.Round(forecastInfo.daily[3].temp.max).ToString() + "°C/";
                tempminblock3.Text = Math.Round(forecastInfo.daily[3].temp.min).ToString() + "°C";










                DateTime convertDateTime(long seconds)
                {
                    DateTime day = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                    day = day.AddSeconds(seconds).ToLocalTime();
                    return day;
                }

            }
        }

        /// <summary>
        /// Click event that runs the getWeather and getForecast methods
        /// </summary>
        private void search(object sender, RoutedEventArgs e)
        {
            getWeather();
           getForecast();
            
        }



        /// <summary>
        /// The event that turns off the applications
        /// </summary>
        private void closeapp(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// An event that minimizes the applications
        /// </summary>
        private void minimizeme(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;

        }

        /// <summary>
        /// The event changes the cursor to the hand after hovering over the image "close"
        /// </summary>
        private void cursorhand1(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;

        }

        /// <summary>
        /// The event changes the cursor to the default one after leaving the image "close" area
        /// </summary>
        private void cursorhand2(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;

        }

        /// <summary>
        /// the event changes the cursor to the hand after hovering over the image "minimize"
        /// </summary>
        private void minimize1(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;

        }

        /// <summary>
        /// The event changes the cursor to the default one after leaving the image "minimize" area
        /// </summary>
        private void minimize2(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;

        }
    }
}
