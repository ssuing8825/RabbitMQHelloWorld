using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.MessagePatterns;

namespace Producer
{
    class Program
    {

        protected static IModel Model;
        protected static IConnection Connection;
        protected static string ExchangeName = "logs";
        protected static string HostName = "frws2356.geicoapp.net";
        protected static string QueueName = "HelloWorld";

        static void Main(string[] args)
        {
            Init();
            Console.WriteLine("Queue is initialized");
            Console.WriteLine("Press any <enter> to send 1000 messages - enter xx to exit");
            string line;
            while ((line = Console.ReadLine()) != "xx")
            {
                SendMessages();
                Console.WriteLine("Press any key to send 1000 messages - enter xx to exit");
            }

        }

        private static void SendMessages()
        {
            IBasicProperties basicProperties = Model.CreateBasicProperties();

            for (int i = 0; i < 1000; i++)
            {
                Model.BasicPublish("", QueueName, basicProperties, System.Text.Encoding.UTF8.GetBytes("Hello World: " + i.ToString()));
                Model.BasicPublish(ExchangeName, string.Empty, basicProperties, System.Text.Encoding.UTF8.GetBytes("Hello World: " + i.ToString()));
                System.Threading.Thread.Sleep(50);
                Console.WriteLine(i);
            }
        }

        private static void Init()
        {

            var connectionFactory = new ConnectionFactory();
            connectionFactory.HostName = HostName;
            Connection = connectionFactory.CreateConnection();
            Model = Connection.CreateModel();
            Model.QueueDeclare(QueueName, false, false, false, null);

            Model.ExchangeDeclare(ExchangeName, "fanout");
        }
    }
}
