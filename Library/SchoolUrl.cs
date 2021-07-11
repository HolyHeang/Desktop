using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class SchoolUrl
    {
        public bool success { get; set; }
        public D data { get; set; }
    }
    public class D
    {
        public string ID { get; set; }
        public string SCHOOL_NAME { get; set; }
        public SchoolSetting schoolSettings { get; set; }
    }
    public class SchoolSetting
    {
        public string NAME { get; set; }
        public string ADDRESS { get; set; }
        public string LOGO_LEFT_NAME_EN { get; set; }
        public string LOGO_LEFT_NAME { get; set; }
        public string LOGO_LEFT_SLOGAN_EN { get; set; }
        public string LOGO_LEFT_SLOGAN { get; set; }
    }
}
