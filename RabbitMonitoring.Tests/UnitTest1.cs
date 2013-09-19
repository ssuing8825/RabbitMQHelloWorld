using System;
using System.Net.Http.Headers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace RabbitMonitoring.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private string hostName = "localhost";
        private string userName = "guest";
        private string password = "guest";
        private string queueName = "HelloWorld";


        [TestMethod]
        public void TestConnection()
        {
            Assert.IsTrue(new AmqpPingCheck().IsConnected("localhost", "guest", "guest"));
        }

        [TestMethod]
        public void PingTest()
        {
            Assert.IsTrue(PingCheck.IsRabbitPingable("localhost"));
        }

        [TestMethod]
        public void TelNetTest()
        {
            Assert.IsTrue(TelnetCheck.IsRabbitTelnet("localhost", 15672));
        }

        [TestMethod]
        public void AlivenessTestTest()
        {
            var ac = new AlivenessChecker();

            Assert.IsTrue(ac.IsAlive("localhost", "guest", "guest").Status == StatusCode.OK);
        }


        [TestMethod]
        public void QueueInfoSerializeTest()
        {
            HttpClient c = new HttpClient();
            c.BaseAddress = new Uri("http://localhost:15672");
            c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //   Console.WriteLine("Example Worker is doing something.");
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, string.Format("/api/queues/%2f/{0}", queueName));
            req.Headers.Authorization =
                     new AuthenticationHeaderValue(
                         "Basic",
                         Convert.ToBase64String(
                             System.Text.ASCIIEncoding.ASCII.GetBytes(
                                 string.Format("{0}:{1}", userName, password))));


            var dtconverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd hh:mm:ss" };
            //  "2013-09-13 7:18:49"
            var r = c.SendAsync(req).Result.Content.ReadAsStringAsync().Result;
            Console.WriteLine(r);
            var queueInfo = JsonConvert.DeserializeObject<QueueInfo>(r, dtconverter);

            //c.SendAsync()
            //c.SendAsync(req).ContinueWith(gg =>
            //{
            //    gg.Result.Content.ReadAsStringAsync().ContinueWith(ddd =>
            //    {

            //        string r = ddd.Result;
            //        // Console.WriteLine("CollectingStats");
            //        try
            //        {
            //            var queueInfo = JsonConvert.DeserializeObject<QueueInfo>(r, dtconverter);




            //            //Console.WriteLine("avg_egress_rate {0}", queueInfo.backing_queue_status.avg_egress_rate);
            //            //Console.WriteLine("avg_ingress_rate {0}", queueInfo.backing_queue_status.avg_ingress_rate);
            //        }
            //        catch (Exception exception)
            //        {
            //            Console.WriteLine(exception);
            //        }
            //    });
            //});

        }

    }
}
