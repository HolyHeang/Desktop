using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class StudentMonthlyResultData
    {
        public List<StudentMonthlyResult> data { get; set; }
    }
    public class StudentMonthlyResult
    {
        public string student_id { get; set; }
        public string numbers { get; set; }
        public bool print { get; set; } = false;
        public string print_title { get; set; } = "ព្រឹត្តិប័ត្រពិន្ទុ";
        public string print_font_color { get; set; } = "White";
        public string print_background { get; set; } = "Red";
        public string student_schoolyear_id { get; set; }
        public string student_school_id { get; set; }
        public string class_id { get; set; }
        public string class_name { get; set; }
        public string schoolyear_id { get; set; }
        public string name { get; set; }
        public string name_en { get; set; }
        public string gender { get; set; }
        public string date_of_birth { get; set; }
        public string phone { get; set; }
        public string localProfileLink { get; set; }
        public resultSemester result_semester { get; set; }
        public resultSemesterExam result_semester_exam { get; set; }
        public Profile profileMedia { get; set; }
        public Instructor instructor { get; set; }
        public MonthlyResult result_monthly { get; set; }
        public List<MonthlyResultAllSubjectResult> all_subject_result { get; set; }
        public List<allSubjectSemesterExamResult> all_subject_semester_exam_result { get; set; }
    }
    public class resultYearly {
        public string id { get; set; }
        public string avg_score { get; set; }
        public string rank { get; set; }
        public string behavior { get; set; }
        public string teacher_comment { get; set; }
        public string recommendation { get; set; }
        public string absence_total { get; set; }
        public string absence_with_permission { get; set; }
        public string absence_without_permission { get; set; }
        public string grading { get; set; }
        public string grading_en { get; set; }
        public string letter_grade { get; set; }
        public string grade_points { get; set; }
        public string is_fail { get; set; }
    }
    public class allSubjectSemesterExamResult
    {
        public string id { get; set; }
        public string subject_id { get; set; }
        public string subject_name { get; set; }
        public string subject_name_en { get; set; }
        public string subject_short { get; set; }
        public int subject_score_max { get; set; }
        public string score { get; set; }
        public int rank { get; set; }
        public string month { get; set; }
        public string grading { get; set; }
        public string grading_en { get; set; }
        public string letter_grade { get; set; }
        public string is_fail { get; set; }
        public int total_score { get; set; }
    }
    public class resultSemester
    {
        public string id { get; set; }
        public string color { get; set; } = "Blue";
        public string avg_score { get; set; }
        public string exam_avg { get; set; }
        public string month_avg { get; set; }
        public int rank { get; set; }
        public string term { get; set; }
        public string recommendation { get; set; }
        public string behavior { get; set; }
        public string absence_total { get; set; }
        public string absence_with_permission { get; set; }
        public string absence_without_permission { get; set; }
        public string grading { get; set; }
        public string grading_en { get; set; }
        public string letter_grade { get; set; }
        public string grade_points { get; set; }
        public string is_fail { get; set; }
        public string morality { get; set; }
        public string bangkeun_phal { get; set; }
        public string health { get; set; }
    }
    public class resultSemesterExam
    {
        public string color { get; set; }
        public string id { get; set; }
        public string avg_score { get; set; }
        public string rank { get; set; }
        public string term { get; set; }
        public string teacher_comment { get; set; }
        public string grading { get; set; }
        public string grading_en { get; set; }
        public string letter_grade { get; set; }
        public string grade_points { get; set; }
        public string is_fail { get; set; }
        public int total_score { get; set; }
    }

    public class ProfileMedia
    {
        public string id { get; set; }
        public string file_name { get; set; }
        public string file_show { get; set; }
    }
    public class Profile
    {
        public string id { get; set; }
        public string file_show { get; set; }
    }
    public class Instructor
    {
        public string id { get; set; }
        public string name { get; set; }
        public string name_en { get; set; }
        public string school_code { get; set; }
        public string gender_name { get; set; }
        public string dob { get; set; }
        public string phone { get; set; }
        public ProfileMedia profileMedia { get; set; }
    }
    public class MonthlyResult
    {
        public string id { get; set; }
        public string avg_score { get; set; }
        public int total_score { get; set; }
        public string total_coeff { get; set; }
        public int rank { get; set; }
        public string month { get; set; }
        public string teacher_comment { get; set; }
        public string recommendation { get; set; }
        public string behavior { get; set; }
        public string absence_total { get; set; }
        public string absence_with_permission { get; set; }
        public string absence_without_permission { get; set; }
        public int absence_exam { get; set; }
        public string grading { get; set; }
        public string grading_en { get; set; }
        public string letter_grade { get; set; }
        public string grade_points { get; set; }
        public string is_fail { get; set; }
        public string color { get; set; }
    }
    public class MonthlyResultAllSubjectResult
    {
        public string id { get; set; }
        public string subject_id { get; set; }
        public string subject_name { get; set; }
        public string subject_name_en { get; set; }
        public string subject_short { get; set; }
        public int subject_score_max { get; set; }
        public string score { get; set; }
        public int rank { get; set; }
        public string month { get; set; }
        public string grading { get; set; }
        public string grading_en { get; set; }
        public string letter_grade { get; set; }
        public string is_fail { get; set; }
    }
}
