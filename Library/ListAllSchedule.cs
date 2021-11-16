using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class ListAllScheduleData
    {
        public List<ListAllSchedule> data { get; set; }
    }
    public class ListAllSchedule
    {
        public List<Schedule_Data> schedule_data { get; set; }
        public First_Instructor first_instructor { get; set; }
        public string name { get; set; }
        public string id { get; set; }
    }
    public class Schedule_Data
    {
        public string name { get; set; }
        public List<Schedule> schedule { get; set; }
    }
    public class First_Instructor
    {
        public string name { get; set; }
    }
}
