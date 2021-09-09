using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class InputScore
    {
        public string month { get; set; }
        public string subject_id { get; set; }
        public string student_school_id { get; set; }
        public string type { get; set; }
        public string term { get; set; }
        public string message { get; set; }
        public string Datadate { get; set; }
        public List<StudentInformation> data { get; set; }

        public ApproveLearningResult approve_learning_result { get; set; }
    }
    public class StudentInformation : INotifyPropertyChanged
    {
        private string Score;
        private bool? Absance_exam;
        private string TeacherRecomment,Color="";
        private bool Check;
        public int tabIndex { get; set; }
        public string color { get; set; } = "Blue";
        public string checkBoxColor 
        {

            get
            {
                return ((bool)absent_exam) ? "#F54928" : "#579DDB";
            }
            set
            {
                Color = value;
                OnpropertyChange();
            }
        }
        public bool focus { get; set; }
        public string error { get; set; } 
        public string visible { get; set; } = "Collapsed";
        public string number { get; set; }
        public string student_id { get; set; }
        public string student_school_id { get; set; }
        public Profiles profileMedia { get; set; }
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
        public string id { get; set; }
        public string subject_id { get; set; }
        public string subject_name { get; set; }
        public string subject_name_en { get; set; }
        public string subject_short { get; set; }
        public string subject_score_min { get; set; }
        public string subject_score_max { get; set; }
        public string score
        {
            get
            {
                return (bool)(absent_exam) ? "0" : Score;
            }
            set
            {
                Score = value;
                OnpropertyChange();
            }
        }
        public string rank { get; set; }
        public string month { get; set; }
        public string grading { get; set; }
        public string grading_en { get; set; }
        public string letter_grade { get; set; }
        public string grade_points { get; set; }
        public string is_fail { get; set; }
        public string teacher_comment
        {
            get
            {
                return ((bool)absent_exam) ? "" : TeacherRecomment;
            }
            set
            {
                TeacherRecomment = value;
                OnpropertyChange();
            }
        }
        public bool? absent_exam 
        {
            get
            {
                return (bool)Absance_exam;
            }
            set
            {
                Absance_exam = value??false;
                OnpropertyChange();
            }
        }
        public bool check 
        {
            get
            {
                return ((bool)absent_exam) ? false : true;
            }
            set
            {
                Check = value;
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
    public class Profiles
    {
        public string id { get; set; }
        public string file_show { get; set; }
    }
    public class ApproveLearningResult
    {
        public string id { get; set; }
        public string class_id { get; set; }
        public string month { get; set; }
        public string term { get; set; }
        public string type { get; set; }
        public string is_approved { get; set; }
        public string approved_date { get; set; }
        public string approved_by { get; set; }
        public string is_submitted { get; set; }
        public string submitted_date { get; set; }
        public string submitted_by { get; set; }
    }
}

