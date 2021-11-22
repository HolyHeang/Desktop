using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class VM
    {
        public List<SummarySubjectGradingScal> summaries { get; set; }
    }
    public class SummarySubjectGradingScal
    {
        public SummarySubjectGrading data { get; set; }
    }
}
