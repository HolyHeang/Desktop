using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class ListSchduleData
    {
        public List<ListSchedule> data { get; set; }
    }
    public class ListSchedule
    {
        public string name { get; set; }
        public List<Schedule> schedule { get; set; }

    }
    public class Schedule
    {
        public string start_time { get; set; }
        public string end_time { get; set; }
        public string full_time { 
            get {
                return start_time + "-" + end_time;
            }
            set {

            } }
        public Mo MO { get; set; }
        public Tu TU { get; set; }
        public We WE { get; set; }
        public TH TH { get; set; }
        public Fr FR { get; set; }
        public Sa SA { get; set; }
    }
    public class Mo
    {
        string subString;
        public string event_title 
        {
            get 
            {
                return subString;    
            }
            set
            {
                var sub = value.Split('(');
                subString = sub[0].ToString();
            } 
        }
        public string session_period { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public Teacher teacher { get; set; }
        public Room room { get; set; }
    }
    public class Tu
    {
        string subString;
        public string event_title
        {
            get
            {
                return subString;
            }
            set
            {
                var sub = value.Split('(');
                subString = sub[0].ToString();
            }
        }
        public string session_period { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public Teacher teacher { get; set; }
        public Room room { get; set; }
    }
    public class We
    {
        string subString;
        public string event_title
        {
            get
            {
                return subString;
            }
            set
            {
                var sub = value.Split('(');
                subString = sub[0].ToString();
            }
        }
        public string session_period { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public Teacher teacher { get; set; }
        public Room room { get; set; }
    }
    public class TH
    {
        string subString;
        public string event_title
        {
            get
            {
                return subString;
            }
            set
            {
                var sub = value.Split('(');
                subString = sub[0].ToString();
            }
        }
        public string session_period { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public Teacher teacher { get; set; }
        public Room room { get; set; }
    }
    public class Fr
    {
        string subString;
        public string event_title
        {
            get
            {
                return subString;
            }
            set
            {
                var sub = value.Split('(');
                subString = sub[0].ToString();
            }
        }
        public string session_period { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public Teacher teacher { get; set; }
        public Room room { get; set; }
    }
    public class Sa
    {
        string subString;
        public string event_title
        {
            get
            {
                return subString;
            }
            set
            {
                var sub = value.Split('(');
                subString = sub[0].ToString();
            }
        }
        public string session_period { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public Teacher teacher { get; set; }
        public Room room { get; set; }
    }
    public class Teacher
    {
        public string name { get; set; }
        public string phone { get; set; }
        public string role { get; set; }
    }
    public class Room
    {
        public string name { get; set; }
    }
}
