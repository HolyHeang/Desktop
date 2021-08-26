using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class AllStudentList
    {
        public List<StuedntofTheYear> data { get; set; }
    }
    public class StuedntofTheYear
    {
        public string number { get; set; }
        public string id { get; set; }
        public string student_id { get; set; }
        public string school_code { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string name_en { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string gender { get; set; }
        public string dob { get; set; }
        public string lower_secondary_school { get; set; }
        public string come_from_class { get; set; }
        public string come_from_school { get; set; }
        public string current_class { get; set; }
        public string current_class_id { get; set; }
        public string level { get; set; }
        public string grade { get; set; }
        public string grade_id { get; set; }
        public string re_exam { get; set; }
        public string give_up { get; set; }
        public string give_up_date { get; set; }
    }
}
