using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamemisOffLine
{

    public class YearofAcademy
    {
        public YearofAcademy(ObservableCollection<Year> data)
        {
            this.data = data;
        }

        public ObservableCollection<Year> data { get; set; }
    }
    public class Year
    {
        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("name_en")]
        public string name_en { get; set; }
        [JsonProperty("term_name")]
        public string term_name { get; set; }
        [JsonProperty("term_number")]
        public string term_number { get; set; }
        [JsonProperty("start")]
        public string start { get; set; }
        [JsonProperty("end")]
        public string end { get; set; }

        public ObservableCollection<SchoolSystem> school_system { get; set; }
    }
    public class SchoolSystem
    {
        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("name")]
        public string name { get; set; }
        public string color { get; set; } = "Black";
        public bool IsExpande { get; set; } = false;
        public List<Grade> grade { get; set; }
    }
    public class Grade
    {
        public string id { get; set; }
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("name_en")]
        public string name_en { get; set; }
        public bool IsExpaned { get; set; } = false;
        public string color { get; set; } = "Black";
        public int sortkey { get; set; }
        public ObservableCollection<Children> children { get; set; }
    }
    public class Children
    {
        [JsonProperty("id")]
        public int id { get; set; }
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("name_en")]
        public string name_en { get; set; }
        public string color { get; set; } = "Black";
    }
}
