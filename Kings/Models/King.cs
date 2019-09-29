using Newtonsoft.Json;
using System;

namespace Kings.Models
{
    public class King
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nm")]
        public string Name { get; set; }

        [JsonProperty("cty")]
        public string City { get; set; }

        [JsonProperty("hse")]
        public string House { get; set; }

        [JsonProperty("yrs")]
        public string Years { get; set; }

        public override string ToString()
        {
            return "Monarch " + Name + " from " + City + ", " + House + " has ruled for " + this.RuledFor() + " years, between " + this.StartYear() + " and " + this.EndYear();
        }
    }

    public static class KingExtensions
    {
        public static int StartYear(this King king)
        {
            return Convert.ToInt32(king.Years.Split('-')[0]);
        }

        public static int EndYear(this King king)
        {
            var yr = king.Years.Split('-');
            // If the reign did not finish yet
            if (yr.Length > 1 && yr[1] == "")
                return DateTime.Now.Year;
            // Normal case - Started one year, finished another
            if (yr.Length > 1  && yr[1] != "")
                return Convert.ToInt32(yr[1]);
            // Year that ended was the same as the one that started
            return king.StartYear();
        }

        public static int RuledFor(this King king)
        {
            return king.EndYear() - king.StartYear();
        }
    }
}
