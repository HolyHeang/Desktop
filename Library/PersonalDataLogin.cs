using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class PersonalDataLogin
    {
        public bool success { get; set; }
        public Personal data { get; set; }
        public Tokens laravel_token_data { get; set; }
    }
    public class Personal
    {
        [JsonProperty("ID")]
        public string ID { get; set; }
        public string FIRSTNAME { get; set; }
        public string LASTNAME { get; set; }
        public string GENDER { get; set; }
        public string ROLE { get; set; }
        public string NAME { get; set; }
        public string NAME_EN { get; set; }
        public string EMAIL { get; set; }
        public string PHONE { get; set; }
        public string ADDRESS { get; set; }
        public string DATE_BIRTH { get; set; }
        public string PROFILE_PHOTO { get; set; }
        public string Local_Profile_link { get; set; }

    }
    public class Tokens
    {
        [JsonProperty("access_token")]
        public string access_token { get; set; }

        [JsonProperty("token_type")]
        public string token_type { get; set; }

        [JsonProperty("expires_in")]
        public string expires_in { get; set; }

        [JsonProperty("expires_in_date")]
        public string expires_in_date { get; set; }
        public string access_url { get; set; }
    }
}
