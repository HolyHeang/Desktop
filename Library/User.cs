using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Library
{
    [DataContract]
    public class User
    {
        [DataMember]
        public string _id { get; set; } = "";
        [DataMember]
        public string loginname { get; set; } = "";
        [DataMember]
        public string email { get; set; } = "";
        [DataMember]
        public string password { get; set; } = "";

        public User()
        {
        }

        public User(string name, string pw)
        {
          
            this.loginname = name;            
            this.password = pw;
        }
    }
}
