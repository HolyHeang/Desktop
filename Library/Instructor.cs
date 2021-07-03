using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class ListInstructor
    {
        public List<Instructors> data { get; set; }
    }
    public class Instructors
    {
        public int number { get; set; }
        public string name { get; set; }
        public string position { get; set; } = "ទទួលបន្ទុកថ្នាក់ទី";
        public string class_name { get; set; }
    }
}
