using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamemisOffLine.Windows
{
    class StaffAttendance
    {
        public string id { get; set; }
        public string name { get; set; }
        public string sex { get; set; }
        public string position { get; set; }
        public string mIn { get; set; }
        public string mOut { get; set; }
        public string eIn { get; set; }
        public string eOut { get; set; }
        public string late { get; set; }
        public string early { get; set; }
        public string signature { get; set; }
        public string other { get; set; }
        public StaffAttendance()
        {

        }

        public StaffAttendance(string id, string name, string sex, string position, string mIn, string mOut, string eIn, string eOut, string lateMorning,string lateEvening, string earlyMorning,string earlyEvening, string signature, string other)
        {
            this.id = id;
            this.name = name;
            this.sex = sex;
            this.position = position;
            this.mIn = mIn;
            this.mOut = mOut;
            this.eIn = eIn;
            this.eOut = eOut;
            this.late = late;

            this.early = early;
            this.signature = signature;
            this.other = other;
        }
    }
}
