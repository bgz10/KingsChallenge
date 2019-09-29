using Kings.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;

namespace Kings.Utilities
{
    public static class HttpDataRetriever
    {
        public static List<King> _DownloadKingsData(string url)
        {
            // Initializing a web client to retrieve the data
            using var w = new WebClient();
            // Reading the data as a string
            var data = string.Empty;
            try
            {
                data = w.DownloadString(url);
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }

            // If data is not empty, deserialize it to a list of Kings
            return !string.IsNullOrEmpty(data) ? JsonConvert.DeserializeObject<List<King>>(data) : new List<King>();
        }
    }
}
