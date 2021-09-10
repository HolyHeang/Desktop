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

        public List<string> totalA { get; set; } = new List<string>();
        public List<string> totalB { get; set; } = new List<string>();
        public List<string> totalC { get; set; } = new List<string>();
        public List<string> totalD { get; set; } = new List<string>();
        public List<string> totalE { get; set; } = new List<string>();
        public List<string> totalF { get; set; } = new List<string>();

        public List<string> girlA { get; set; } = new List<string>();
        public List<string> girlB { get; set; } = new List<string>();
        public List<string> girlC { get; set; } = new List<string>();
        public List<string> girlD { get; set; } = new List<string>();
        public List<string> girlE { get; set; } = new List<string>();
        public List<string> girlF { get; set; } = new List<string>();

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
