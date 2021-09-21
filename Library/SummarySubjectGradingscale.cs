using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class SummarySubjectGradingList
    {
       public List<SummarySubjectGrading> data = new List<SummarySubjectGrading>();
    }

    public class SummarySubjectGrading
    {
        public string level { get; set; }
        public string schoolyear { get; set; }
        public string schooltype { get; set; }
        public string international_edu { get; set; }
        public List<Subjects> subjects { get; set; }
        public Students students { get; set; }

    }
    public class Subjects
    {
        public string id { get; set; }
        public string subject_id { get; set; }
        public string name { get; set; }

        public string totalA { get; set; }
        public string totalB { get; set; }
        public string totalC { get; set; }
        public string totalD { get; set; }
        public string totalE { get; set; }
        public string totalF { get; set; }

        public string girlA { get; set; } 
        public string girlB { get; set; } 
        public string girlC { get; set; } 
        public string girlD { get; set; } 
        public string girlE { get; set; } 
        public string girlF { get; set; } 

        public List<GradingSystem> grading_system { get; set; }
    }
    public class GradingSystem
    {
        public string letter_grade { get; set; }
        public Student student { get; set; }
    }
    public class Student
    {
        public string total { get; set; }
        public string female { get; set; }
    }
    public class Students
    {
        public string totals { get; set; }
        public string female { get; set; }
    }
}
