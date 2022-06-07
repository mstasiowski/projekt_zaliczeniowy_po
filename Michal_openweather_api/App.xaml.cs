using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Michal_openweather_api
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
    }

    /// <summary>
    /// temp class includes day temperature, minimum temperature, maximum temperature, night temperature, evening temperature and morning temperature
    /// </summary>
    public class temp
    {
        public double day { get; set; }
        public double min { get; set; }
        public double max { get; set; }
        public double night { get; set; }
        public double eve { get; set; }

        public double morn { get; set; }
    }

    /// <summary>
    /// The weather class contains the main, the weather description, and the weather icon string
    /// </summary>
    public class weather
    {
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }

    /// <summary>
    /// the daily class includes time of the forecasted data in Unix UTC, temp class, and list weather
    /// </summary>
    public class daily
    {
        public long dt { get; set; }
        public temp temp { get; set; }
        public List<weather> weather { get; set; }

    }
    /// <summary>
    /// The forecastinfo class lists the daily class
    /// </summary>
    public class forecastInfo
    {
        public List<daily> daily { get; set; }
    }
}
