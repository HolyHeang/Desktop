using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class StaffAttendanceDailyList
    {
        public string date;
        public List<StaffAttendanceDaily> data { get; set; }
    }
    public class StaffAttendanceDaily : INotifyPropertyChanged
    {
        private bool Late, Leave, Permission, Absent;
        public string number { get; set; }
        public string show { get; set; } = "Collapsed";
        public string id { get; set; }
        public string name { get; set; }
        public string gender { get; set; }
        public string position { get; set; }
        public DailyPresent daily_present { get; set; }
        public bool? late { 
            get {
                return (bool)Late;
            }
            set
            {
                Late = value ?? false;
                OnpropertyChange();
            }
        }
        public bool? leave {
            get
            {
                return (bool)Leave;
            }
            set
            {
                Leave = value ?? false;
                OnpropertyChange();
            }
        }
        public bool? permission {
            get
            {
                return (bool)Permission;
            }
            set
            {
                Permission = value ?? false;
                OnpropertyChange();
            }
        }
        public bool? absent {
            get
            {
                return (bool)Absent;
            }
            set
            {
                Absent = value ?? false;
                OnpropertyChange();
            }
        }
        public string other { get; set; }
        public string mIn { get; set; } = "black";
        public string mOut { get; set; } = "black";
        public string aOut { get; set; } = "black";
        public string aIn { get; set; } = "black";

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnpropertyChange([CallerMemberName] string caller = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
        }
    }
    public class DailyPresent
    {
        public Morning morning { get; set; }
        public Afternoon afternoon { get; set; }
    }
    public class Morning: INotifyPropertyChanged
    {
        private string Min, Mout;

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnpropertyChange([CallerMemberName] string caller = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
        }

        public string in_time 
        {
            get
            {
                return Min;
            }
            set
            {
                Min = value;
                OnpropertyChange();
            }
        }
        public string out_time
        {
            get
            {
                return Mout;
            }
            set
            {
                Mout = value;
                OnpropertyChange();
            }
        }
    }
    public class Afternoon: INotifyPropertyChanged
    {
        private string Ain, Aout;
        public string in_time 
        {
            get
            {
                return Ain;
            }
            set
            {
                Ain = value;
                OnpropertyChange();
            }
        }
        public string out_time 
        {
            get
            {
                return Aout;
            }
            set
            {
                Aout = value;
                OnpropertyChange();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnpropertyChange([CallerMemberName] string caller = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
        }
    }
}
