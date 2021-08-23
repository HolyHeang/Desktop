using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class ClassificationListTeacher
    {
        public List<ClassificationTeacer> data { get; set; }
    }
    public class ClassificationTeacer
    {
        public string number { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string name_en { get; set; }
        public string gender_name { get; set; }
        public string dob { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string staff_school_code { get; set; }
        public string code { get; set; }
        public string framework { get; set; }
        public string prokas { get; set; }
        public string mapCode { get; set; }
        public string class_id { get; set; }
        public string class_name { get; set; }
        public string class_name_en { get; set; }

    }
}
