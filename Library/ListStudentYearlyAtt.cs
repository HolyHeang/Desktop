using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class ListStudentYearlyAtt
    {
        public List<StudentYearly> data { get; set; }
    }
    public class StudentYearly
    {
        public string id { get; set; }
        public string student_school_id { get; set; }
        public string name { get; set; }
        public string gender_name { get; set; }
        public List<StudentYearlyAtt> all_attendance { get; set; }
    }
    public class StudentYearlyAtt
    {
        public string name { get; set; }
        public string month { get; set; }
        public string term { get; set; }
        public string type { get; set; }
        public string total { get; set; }
        public string total_absent { get; set; }
        public string total_permission { get; set; }
    }
    public class SubListStudentAtt
    {
        public string number { get; set; }
        public string student_school_id { get; set; }
        public string name { get; set; }
        public string month1 { get; set; }
        public string absent1 { get; set; }
        public string per1 { get; set; }
        public string month2 { get; set; }
        public string absent2 { get; set; }
        public string per2 { get; set; }
        public string month3 { get; set; }
        public string absent3 { get; set; }
        public string per3 { get; set; }
        public string month4 { get; set; }
        public string absent4 { get; set; }
        public string per4 { get; set; }
        public string month5 { get; set; }
        public string absent5 { get; set; }
        public string per5 { get; set; }
        public string month6 { get; set; }
        public string absent6 { get; set; }
        public string per6 { get; set; }
        public string month7 { get; set; }
        public string absent7 { get; set; }
        public string per7 { get; set; }
        public string month8 { get; set; }
        public string absent8 { get; set; }
        public string per8 { get; set; }
        public string month9 { get; set; }
        public string absent9 { get; set; }
        public string per9 { get; set; }
        public string month10 { get; set; }
        public string absent10 { get; set; }
        public string per10 { get; set; }

        public string semester1 { get; set; }
        public string semersterAtt1 { get; set; }
        public string semesterPer1 { get; set; }
        public string semester2 { get; set; }
        public string semersterAtt2 { get; set; }
        public string semesterPer2 { get; set; }

        public string yearly { get; set; }
        public string yearlyAtt { get; set; }
        public string yearlyPer { get; set; }
        public string yearlyTotal { get; set; }

    }
}
