using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NadekoBot.Modules.Searches.Commands.Models
{
    public class Coord
    {
        public double lon { get; set; }
        public double lat { get; set; }
    }

    public class Weather
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }

    public class Main
    {
        public double temp { get; set; }
        public string tempA {
            get {
                float tempA = (float)temp * 1.8 + 32;
                return tempA.ToString("N1");
            }
        }
        public float pressure { get; set; }
        public float humidity { get; set; }
        public double temp_min { get; set; }
        public string temp_minA {
            get {
                double temp_minA = (double)temp_min * 1.8 + 32;
                return temp_minA.ToString("N1");
            }
        }
        public double temp_max { get; set; }
        public string temp_maxA {
            get {
                double temp_maxA = (double)temp_max * 1.8 + 32;
                return temp_maxA.ToString("N1");
            }
        }
    }

    public class Wind
    {
        public double speed { get; set; }
        public string speedA {
            get {
                double speedA = (double)speed * 0.621371;
                return speedA.ToString("N1");
            }
        }
        public double deg { get; set; }
    }

    public class Clouds
    {
        public int all { get; set; }
    }

    public class Sys
    {
        public int type { get; set; }
        public int id { get; set; }
        public double message { get; set; }
        public string country { get; set; }
        public double sunrise { get; set; }
        public double sunset { get; set; }
    }

    public class WeatherData
    {
        public Coord coord { get; set; }
        public List<Weather> weather { get; set; }
        public Main main { get; set; }
        public int visibility { get; set; }
        public Wind wind { get; set; }
        public Clouds clouds { get; set; }
        public int dt { get; set; }
        public Sys sys { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int cod { get; set; }
    }
}
