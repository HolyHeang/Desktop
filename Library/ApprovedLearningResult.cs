using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class ApprovedLearningResult
    {
        public string status { get; set; }
        public RequestApproved data { get; set; }
    }
    public class RequestApproved
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
        public string success { get; set; }
    }
}
