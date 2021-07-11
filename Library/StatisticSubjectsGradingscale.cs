using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    class StatisticSubjectsGradingscale
    {
        public string[] features { get; set; }
        public List<SubjectsTarget> subjects_target { get; set; }
    }
    class SubjectsTarget
    {
        public string subject_name { get; set; }
        public string[] data { get; set; }
    }
}
