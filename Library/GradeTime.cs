using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class GradeTime
    {
        public List<Times> data { get; set; }
    }
    public class Times
    {
        public string semester { get; set; }
        public string name { get; set; }
        public List<Months> months { get; set; }
    }
    public class Months
    {
        public string month { get; set; }
        public string displayMonth { get; set; }
        public string year { get; set; }
    }
}
