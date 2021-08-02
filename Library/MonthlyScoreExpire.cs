using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class MonthlyScoreExpire
    {
        public Expire data { get; set; }
    }
    public class Expire
    {
        public string id { get; set; }
        public string schoolyear_id { get; set; }
        public string month_value { get; set; }
        public string semester { get; set; }
        public string type { get; set; }
        public string expired_at { get; set; }
        public string is_expired { get; set; }
    }
}
