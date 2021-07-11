using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Library
{
    public class SQLiteHelper
    {
        public static List<Subject> LoadSubject()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadString()))
            {
                var output = cnn.Query<Subject>("Select * from tblSubject", new DynamicParameters());
                return output.ToList();
            }
        }
        public static void SaveSubject(List<Subject> subjects)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadString()))
            {
                cnn.Execute($"Insert into tblSubject (id, name, name_en, code) Values (@id, @name, @name_en, @code)", subjects);
            }
        }
        private static string LoadString(string id = "Defualt")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}
