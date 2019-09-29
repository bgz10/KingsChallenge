using Kings.Models;

namespace Kings.Utilities
{
    public class Initializer
    {
        private readonly string _url;

        public Initializer(string url)
        {
            _url = url;
        }

        public Roialty Seed()
        {
            return new Roialty { Kings = HttpDataRetriever._DownloadKingsData(_url) };
        }
    }
}
