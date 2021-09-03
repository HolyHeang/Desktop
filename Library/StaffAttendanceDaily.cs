using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class StaffAttendanceDailyList
    {
        public List<StaffAttendanceDaily> data { get; set; }
    }
    public class StaffAttendanceDaily
    {
        public string number { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string gender { get; set; }
        public string position { get; set; }
        public DailyPresent daily_present { get; set; }
        public bool late { get; set; }
        public bool leave { get; set; }
        public bool permission { get; set; }
        public bool absent { get; set; }
        public string other { get; set; }
    }
    public class DailyPresent
    {
        public Morning morning { get; set; }
        public Afternoon afternoon { get; set; }
    }
    public class Morning
    {
        public string in_time { get; set; }
        public string out_time { get; set; }
    }
    public class Afternoon
    {
        public string in_time { get; set; }
        public string out_time { get; set; }
    }
}
