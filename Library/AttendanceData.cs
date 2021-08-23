using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
     public class ListAttendanceData
    {
        public List<StudentAttendance> data { get; set; }
    }
    public class StudentAttendance
    {
        public string number { get; set; }
        public string id { get; set; }
        public string student_school_id { get; set; }
        public string name { get; set; }
        public string gender_name { get; set; }
        public string total { get; set; }
        public string total_permission { get; set; }
        public string total_absent { get; set; }
        public List<ListAttendance> month_attendance { get; set; }
    }
    public class ListAttendance
    {
        public string day_number { get; set; }
        public Attendance absent { get; set; }
    }
    public class Attendance
    {
        public string value { get; set; }
        public AttendanceData absent_data { get; set; }
    }
    public class AttendanceData
    {
        public string student_id { get; set; }
        public string attendance_date { get; set; }
        public string absent_count { get; set; }
        public string permission_count { get; set; }
        public string late_count { get; set; }
    }
}
