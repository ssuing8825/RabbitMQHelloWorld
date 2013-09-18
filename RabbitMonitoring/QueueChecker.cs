﻿using System;
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
    public class QueueChecker
    {
        private static PerformanceCounter Memory = new PerformanceCounter("Rabbit.HelloWorld", "Memory", "HelloWorldInstance", false);
        private static PerformanceCounter NumberOfMessages = new PerformanceCounter("Rabbit.HelloWorld", "Number of Messages", "HelloWorldInstance", false);
        private static PerformanceCounter NumberOfMessagesReady = new PerformanceCounter("Rabbit.HelloWorld", "Number of Messages ready", "HelloWorldInstance", false);
        private static PerformanceCounter NumberOfMessagesUnAck = new PerformanceCounter("Rabbit.HelloWorld", "Number of Messages Unacknowledged", "HelloWorldInstance", false);
        private static PerformanceCounter NumberOfConsumers = new PerformanceCounter("Rabbit.HelloWorld", "Number Consumers", "HelloWorldInstance", false);
        private static PerformanceCounter NumberOfActiveConsumers = new PerformanceCounter("Rabbit.HelloWorld", "Number Active Consumers", "HelloWorldInstance", false);

        public void LogQueueInfo(string hostName, string userName, string password, string queueName)
        {

            HttpClient c = new HttpClient();
            c.BaseAddress = new Uri("http://localhost:15672");
            c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //   Console.WriteLine("Example Worker is doing something.");
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, string.Format("/api/queues/%2f/{0}",queueName));
            req.Headers.Authorization =
                     new AuthenticationHeaderValue(
                         "Basic",
                         Convert.ToBase64String(
                             System.Text.ASCIIEncoding.ASCII.GetBytes(
                                 string.Format("{0}:{1}", userName, password))));


            var dtconverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd hh:mm:ss" };
            //  "2013-09-13 7:18:49"

            c.SendAsync(req).ContinueWith(gg =>
            {
                gg.Result.Content.ReadAsStringAsync().ContinueWith(ddd =>
                {

                    string r = ddd.Result;
                    Console.WriteLine("CollectingStats");
                    try
                    {
                        var queueInfo = JsonConvert.DeserializeObject<QueueInfo>(r, dtconverter);
                        Memory.RawValue = queueInfo.memory;
                        NumberOfMessages.RawValue = queueInfo.messages;
                        NumberOfMessagesReady.RawValue = queueInfo.messages_ready;
                        NumberOfMessagesUnAck.RawValue = queueInfo.messages_unacknowledged;
                        NumberOfConsumers.RawValue = queueInfo.consumers;
                        NumberOfActiveConsumers.RawValue = queueInfo.active_consumers;

                        Console.WriteLine("avg_egress_rate {0}", queueInfo.backing_queue_status.avg_egress_rate);
                        Console.WriteLine("avg_ingress_rate {0}", queueInfo.backing_queue_status.avg_ingress_rate);
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                    }
                });
            });
        }
    }
}