using System.Collections.Generic;
using System.Linq;

namespace Kings.Models
{
    public class Roialty
    {
        public List<King> Kings { get; set; } = new List<King>();
    }

    public static class RoialtyExtensions
    {
        public static King[] LongestReign(this Roialty roialty)
        {
            var max = roialty.Kings.Max(x => x.RuledFor());
            return roialty.Kings.Where(x => x.RuledFor() == max).ToArray();
        }
    }
}
