using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Diagnostics;

namespace RabbitMonitoring
{
    public class StatCollector : Worker
    {
        private static PerformanceCounter AverageMessagesInQueue = new PerformanceCounter("RabbitMQMonitoringCategory2", "MessagesInQueue", false);

        protected override void DoWork()
        {

            HttpClient c = new HttpClient();
            c.BaseAddress = new Uri("http://localhost:15672");
            c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

         //   Console.WriteLine("Example Worker is doing something.");
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, "/api/queues/%2f/HelloWorld");
            req.Headers.Authorization =
                     new AuthenticationHeaderValue(
                         "Basic",
                         Convert.ToBase64String(
                             System.Text.ASCIIEncoding.ASCII.GetBytes(
                                 string.Format("{0}:{1}", "guest", "guest"))));


            var dtconverter = new IsoDateTimeConverter {DateTimeFormat = "yyyy-MM-dd hh:mm:ss"};
          //  "2013-09-13 7:18:49"

            c.SendAsync(req).ContinueWith(gg =>
                {
                    gg.Result.Content.ReadAsStringAsync().ContinueWith(ddd =>
                        {
                            Console.WriteLine("sdfdsfwewe");
                            string r = ddd.Result;
                            Console.WriteLine(r);
                            try
                            {
                                var dasd = JsonConvert.DeserializeObject<QueueInfo>(r, dtconverter);
                                AverageMessagesInQueue.RawValue = dasd.messages;
                                
                                Console.WriteLine("Messages {0}", dasd.messages);
                            }
                            catch (Exception exception)
                            {

                                Console.WriteLine(exception);

                            }




                        });
                    Console.WriteLine("asdf");
                });

            //    c.GetStringAsync(@"http://localhost:15672/api/queues/%2f/Build.Tests.Stubs").ContinueWith(gg =>
            //        {
            //            string r = gg.Result;
            //            Console.WriteLine(r);
            //            var dasd = JsonConvert.DeserializeObject<QueueInfo>(r);
            //            Console.WriteLine(dasd.memory);
            //        }
            //);

            //    Debug.WriteLine("Example Worker is doing something.");



            //c.Se(req).ContinueWith(respTask =>
            //    {

            //        respTask.Result.Content.ReadAsStringAsync().ContinueWith(gg =>
            //            {
            //                Console.WriteLine(gg.Result);
            //                // gg.Result
            //            });

            //Console.WriteLine("Response: {0}", respTask.Result.Content.ReadAsStreamAsync().ContinueWith(t =>
            //    {
            //       Console.WriteLine(t.Result); 
            //    }));

            //    });


            //c.SendAsync(req).ContinueWith(respTask =>
            //{

            //    respTask.Result.Content.ReadAsStringAsync().ContinueWith(t =>
            //    {
            //        var dasd = JsonConvert.DeserializeObject<QueueInfo>(t.Result);

            //        Console.WriteLine(dasd.memory);
            //    });
            //});






        }
    }
}
