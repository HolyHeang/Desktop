using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class ListMapteacherCode
    {
        public List<MapteacherCode> data { get; set; }
    }
    public class MapteacherCode
    {
        public string id { get; set; }
        public string name { get; set; }
        public string name_en { get; set; }
        public string code { get; set; }
        public List<MapTeacherSubject> mapTeacherSubject { get; set; }

    }
    public class MapTeacherSubject
    {
        public string number { get; set; }
        public string id { get; set; }
        public string code { get; set; }
        public string teach_class { get; set; }
        public string subject_name { get; set; }
        public string instructor_class_name { get; set; }
        public ListTeacherTeaching teacher { get; set; }
    }
    public class ListTeacherTeaching
    {
        public string id { get; set; }
        public string name { get; set; }
        public string gender_name { get; set; }
        public string phone { get; set; }
    }
}
