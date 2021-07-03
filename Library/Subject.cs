using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class Data
    {
        public List<Subject> data { get; set; }
    }
    public class Subject
    {
        public int id { get; set; }
        public string name { get; set; }
        public string name_en { get; set; }
        public string code { get; set; }
    }
}
