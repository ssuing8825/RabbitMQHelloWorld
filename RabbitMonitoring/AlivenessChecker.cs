using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RabbitMQ.Client;

namespace RabbitMonitoring
{
    public class AlivenessChecker
    {

        public bool IsAlive(string hostName, string userName, string password)
        {
            using (HttpClient c = new HttpClient())
            {
                c.BaseAddress = new Uri("http://localhost:15672");
                c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, string.Format("/api/aliveness-test/%2f"));
                req.Headers.Authorization =
                         new AuthenticationHeaderValue(
                             "Basic",
                             Convert.ToBase64String(
                                 System.Text.ASCIIEncoding.ASCII.GetBytes(
                                     string.Format("{0}:{1}", userName, password))));


                var sendTask = c.SendAsync(req);
               

                c.SendAsync(req).ContinueWith(gg =>
                {
                    if (!gg.Result.IsSuccessStatusCode)
                    {
                        gg.Result.Content.ReadAsStringAsync().ContinueWith(r =>
                        {
                            //Need to do something with this string.
                            string result = r.Result;
                            return false;

                        });
                    }
                  
                });
            }
            return true;
        }
    }
}
