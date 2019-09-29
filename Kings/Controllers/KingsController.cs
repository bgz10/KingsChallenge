using System.Collections.Generic;
using System.Linq;
using Kings.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kings.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KingsController : ControllerBase
    {
        private readonly Roialty _roialty;

        public KingsController(Roialty roialty)
        {
            _roialty = roialty;
        }

        /// <summary>
        /// Gets the total number of monarchs
        /// </summary>
        /// <returns>A response code 200 and the number of monarchs</returns>
        /// <response code="200">Returns the number of monarchs</response>
        /// <response code="400">If the item is null</response>
        [HttpGet("/api/kings/total-monarchs")]
        public IActionResult TotalMonarchs()
        {
            if (_roialty == null)
                return BadRequest("There are no monarchs in the list");
            return Ok(_roialty.Kings.Count());
        }

        /// <summary>
        /// Gets the longest reign.
        /// It accounts for monarchs with equal max number of years
        /// </summary>
        /// <returns>A response code 200 and a list of all the monarchs that have reigned the most</returns>
        /// <response code="200">Returns a list of monarchs that have reigned the most</response>
        /// <response code="400">If the item is null</response>
        [HttpGet("/api/kings/longest-reign")]
        public IActionResult LongestReign()
        {
            if (_roialty == null)
                return BadRequest("There are no monarchs in this list");
            // Consider if there are multiple monarchs with the same amount of years ruled.
            return Ok(_roialty.LongestReign().Select(x => x.ToString()));
        }

        /// <summary>
        /// Gets the house that has reigned the most.
        /// Alongside that, it prints the ordered list of houses per years reigned
        /// </summary>
        /// <returns>A response code 200, the house that has reigned the most and for how many years + all the houses and how much they reigned</returns>
        /// <response code="200">House that has reigned the most and all the other houses</response>
        /// <response code="400">If the item is null</response>
        [HttpGet("/api/kings/house-reign")]
        public IActionResult LongestHouseReign()
        {
            if (_roialty == null)
                return BadRequest("There are no monarchs in this list");
            var dict = new Dictionary<string, int>();
            foreach (var ent in _roialty.Kings)
            {
                if (dict.TryGetValue(ent.House, out _))
                {
                    dict[ent.House] += ent.RuledFor();
                }
                else
                {
                    dict.Add(ent.House, ent.RuledFor());
                }
            }

            var x = from entry in dict orderby entry.Value descending select entry;
            var topList = x.Aggregate("", (current, entry) => current + (entry.Key + "  -  " + entry.Value + "\n"));

            var longestReign = dict.Aggregate((pair, y) => pair.Value > y.Value ? pair : y).Key;
            return Ok(longestReign + " has reigned the most. Cumulative, it has reigned for " + dict.Values.Max() + " years. \nTOP\n" + topList);
        }

        /// <summary>
        /// Gets the most common name - without the surname and roman letter - Not sure if that's what was asked of me xD
        /// It also provides the list of all the names and how often they are occurred. 
        /// </summary>
        /// <returns>A response code 200, the most common name and its occurrence + a list with all the names and their occurrence</returns>
        /// <response code="200">Returns the most common name</response>
        /// <response code="400">If the item is null</response>
        [HttpGet("/api/kings/common-names")]
        public IActionResult MostCommonName()
        {
            if (_roialty == null)
                return BadRequest("There are no monarchs in this list");
            var dict = new Dictionary<string, int>();
            foreach (var name in _roialty.Kings.Select(ent => ent.Name.Split(" ")[0]))
            {
                if (dict.TryGetValue(name, out _))
                {
                    dict[name]++;
                }
                else
                {
                    dict.Add(name, 1);
                }
            }
            var topList = dict.Aggregate("", (current, entry) => current + (entry.Key + "  -  " + entry.Value + "\n"));
            var commonName = dict.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

            return Ok(commonName + " is the most common name with " + dict.Values.Max() + " entries. \nList of Names and their occurrence\n" + topList);
        }

        /// <summary>
        /// Gets all the monarchs / Sanity check
        /// </summary>
        /// <returns>A response code 200 and all the data / Sanity check</returns>
        /// <response code="200">Returns the monarchs</response>
        /// <response code="400">If the item is null</response>
        [HttpGet("/api/kings/data")]
        public IActionResult DisplayData()
        {
            if (_roialty == null)
                return BadRequest("There are no monarchs in this list");
            return Ok(_roialty.Kings);
        }
    }
}