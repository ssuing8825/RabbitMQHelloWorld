using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace Consumer
{
    class Program
    {
        protected static IModel Model;
        protected static IConnection Connection;
        protected static string QueueName = "HelloWorld";
        protected static string HostName = "localhost";


        static void Main(string[] args)
        {
            Init();
            Console.WriteLine("Queue is initialized");
            Consume();


        }

        private static void Init()
        {

            var connectionFactory = new ConnectionFactory();
            connectionFactory.HostName = HostName;
            Connection = connectionFactory.CreateConnection();
            Model = Connection.CreateModel();
            Model.QueueDeclare(QueueName, false, false, false, null);
        }

        private static void Consume()
        {
            QueueingBasicConsumer consumer = new QueueingBasicConsumer(Model);
            String consumerTag = Model.BasicConsume(QueueName, false, consumer);
            while (true)
            {
                try
                {
                    RabbitMQ.Client.Events.BasicDeliverEventArgs e = (RabbitMQ.Client.Events.BasicDeliverEventArgs)consumer.Queue.Dequeue();
                    IBasicProperties props = e.BasicProperties;
                    byte[] body = e.Body;
                    // ... process the message
                    Console.WriteLine(System.Text.Encoding.UTF8.GetString(body));
                    Model.BasicAck(e.DeliveryTag, false);
                }
                catch (OperationInterruptedException ex)
                {
                    // The consumer was removed, either through
                    // channel or connection closure, or through the
                    // action of IModel.BasicCancel().
                    break;
                }
            }
        }
    }
}
