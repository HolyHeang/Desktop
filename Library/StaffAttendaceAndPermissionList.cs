using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class StaffPermissionList
    {
        public List<StaffPermission> data { get; set; }
    }
    public class StaffPermission
    {
        public string number { get; set; }
        public string visble { get; set; } = "Visible";
        public string id { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public string duration { get; set; }
        public string reason { get; set; }
        public string others { get; set; }
        public UserPermission user { get; set; }
        public string is_approve { get; set; }
        public string approved_comment { get; set; }
        public Approved_by approved_by { get; set; }
        public string approved_at { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
    }
    public class Approved_by
    {
        public string name { get; set; }
    }
    public class UserPermission
    {
        public string id { get; set; }
        public string school_code { get; set; }
        public string name { get; set; }
        public string gender_name { get; set; }
    }
}
