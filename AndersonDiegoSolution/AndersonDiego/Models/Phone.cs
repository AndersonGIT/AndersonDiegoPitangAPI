using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AndersonDiego.Models
{
    public class Phone
    {
        [JsonProperty("number")]
        public int? Number { get; set; }

        [JsonProperty("area_code")]
        public int? AreaCode { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }
    }
}