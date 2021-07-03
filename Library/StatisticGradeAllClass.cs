using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class StatisticGradeAllClass
    {

    }
    public class StatisticClass
    {
        public string name { get; set; }
        public string total_stu { get; set; }
        public string class_id { get; set; }
        public string[][] dataGradingScaleMonthly { get; set; }
        public string[][] totalResultBarChart { get; set; }
        public List<SubjectGradingScale> subjectGradingScale { get; set; }
    }
    public class SubjectGradingScale
    {
        public string subject_name { get; set; }
        public string total_pass { get; set; }
        public string total_fail { get; set; }
        public string[][] data { get; set; }
    }


}
