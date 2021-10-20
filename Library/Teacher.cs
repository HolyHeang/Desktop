using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class Teachers
    {
        public List<ListTeacher> data { get; set; }
    }
    public class ListTeacher
    {
        public string id { get; set; }
        public string object_type { get; set; }
        public string name { get; set; }
        public string name_en { get; set; }
        public string schoolyear { get; set; }
        public string school_type { get; set; }
        public string calculate_additional_subject_monthly { get; set; }
        public List<TeachingClass> teaching_classes { get; set; }
    }
    public class TeachingClass
    {
        public bool isSelected { get; set; } = true;
        public string id { get; set; }
        public string object_type { get; set; }
        public string name { get; set; }
        public string name_en { get; set; }
        public string schoolyear { get; set; }
        public string school_type { get; set; }
        public string is_instructor { get; set; }
        public string color { get; set; } = "Black";
    }
}
