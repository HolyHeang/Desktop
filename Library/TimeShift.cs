using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class TimesButton
    {
        public string classId { get; set; }
        public string year { get; set; }
        public ObservableCollection<GradeTimeButton> data { get; set; }
    }
    public class GradeTimeButton
    {
        public string colors { get; set; } = "WhiteSmoke";
        [JsonProperty("semester")]
        public string semester { get; set; }
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("months")]
        public ObservableCollection<MonthsButton> months { get; set; }
    }
    public class MonthsButton
    {
        public bool isSelecet { get; set; }
        public string color { get; set; } = "WhiteSmoke";
        public string month { get; set; }
        public string displayMonth { get; set; }
        public string year { get; set; }
    }
}
