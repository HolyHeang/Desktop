using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamemisOffLine
{
    public class StudentSubjectMonthlyResultCR
    {
        public string number { get; set; }
        public string subject_name { get; set; }
        public string score { get; set; }
        public int max_score { get; set; }
        public string grade  { get; set; }
        public int rank { get; set; }
    }
}
