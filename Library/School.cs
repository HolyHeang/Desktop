using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Library
{
    public class Schools
    {
        [JsonProperty("action_key")]
        public string action_key { get; set; }
        [JsonProperty("url")]
        public string url { get; set; }

    }
}
