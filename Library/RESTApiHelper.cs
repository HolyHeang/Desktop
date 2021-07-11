using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class RESTApiHelper
    {
        public static async Task<string> GetAll(string accessUrl="",string endPoint= "",string token="")
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(
               new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer",token );
                using (HttpResponseMessage res = await client.GetAsync(accessUrl + endPoint))
                {
                    using (HttpContent content = res.Content)
                    {
                        string data = await content.ReadAsStringAsync();
                        if (data != null)
                        {
                            return data;
                        }
                    }
                }
            }
            return string.Empty;
        }
        public static string BeautifulJson(string Jsonstr)
        {
            JToken parseJson = JToken.Parse(Jsonstr);
            return parseJson.ToString(Formatting.Indented);
        }

        public static async Task<string> GetToken(string user, string pass,string accessUrl= "ee9f4b9b-ffc2-11ea-89b1-00163c091aef")
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(
               new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage res = client.PostAsJsonAsync("https://api-services.camemis-learn.com/api/v1/ee9f4b9b-ffc2-11ea-89b1-00163c091aef" + "/auth/login", new User { loginname = user, password = pass }).Result)
                {
                    using (HttpContent content = res.Content)
                    {
                        string data = await content.ReadAsStringAsync();
                        if (data != null)
                        {
                            return data;
                        }
                    }
                }
            }
            return string.Empty;
        }
    }
}
