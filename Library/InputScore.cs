using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class InputScore
    {
        public List<StudentInformation> data { get; set; }
    }
    public class StudentInformation
    {
        public string number { get; set; }
        public string student_id { get; set; }
        public string student_schoolyear_id { get; set; }
        public string class_id { get; set; }
        public string class_name { get; set; }
        public string class_name_en { get; set; }
        public string schoolyear_id { get; set; }
        public string name { get; set; }
        public string name_en { get; set; }
        public string gender { get; set; }
        public string date_of_birth { get; set; }
        public string phone { get; set; }
        public SubjectScore result_subject { get; set; }
    }
    public class SubjectScore
    {
        public string id { get; set; }
        public string subject_id { get; set; }
        public string subject_name { get; set; }
        public string subject_name_en { get; set; }
        public string subject_short { get; set; }
        public string subject_score_min { get; set; }
        public string subject_score_max { get; set; }
        public string score { get; set; }
        public string rank { get; set; }
        public string month { get; set; }
        public string grading { get; set; }
        public string grading_en { get; set; }
        public string letter_grade { get; set; }
        public string grade_points { get; set; }
        public string is_fail { get; set; }
    }
}

