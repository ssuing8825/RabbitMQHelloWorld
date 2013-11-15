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
        protected static string ExchangeName = "helloworld";
        protected static string DirectQueueName = "HelloWorld";
        protected static string QueueName;

        protected static string HostName = "localhost";
        private static Random random = new Random();

        static void Main(string[] args)
        {
            //InitDirect();
            InitSubscriber();


               Consume();



        }

        private static void InitSubscriber()
        {
            var connectionFactory = new ConnectionFactory();
            connectionFactory.HostName = HostName;
            Connection = connectionFactory.CreateConnection();
            Model = Connection.CreateModel();
            Model.ExchangeDeclare(ExchangeName, "fanout");
            QueueName = Model.QueueDeclare().QueueName;
            Model.QueueBind(QueueName, ExchangeName, "");

        }

        private static void InitDirect()
        {


            var connectionFactory = new ConnectionFactory();
            connectionFactory.HostName = HostName;
            Connection = connectionFactory.CreateConnection();
            Model = Connection.CreateModel();
            Model.QueueDeclare(DirectQueueName, false, false, false, null);
            QueueName = DirectQueueName;

        }

        private static void Consume()
        {
            QueueingBasicConsumer consumer = new QueueingBasicConsumer(Model);

            String consumerTag = Model.BasicConsume(QueueName, false, consumer);

            while (true)
            {
                try
                {
                    var e = (RabbitMQ.Client.Events.BasicDeliverEventArgs)consumer.Queue.Dequeue();
                    IBasicProperties props = e.BasicProperties;
                    byte[] body = e.Body;
                    // ... process the message
                    Console.WriteLine(System.Text.Encoding.UTF8.GetString(body));

                    System.Threading.Thread.Sleep(random.Next(10, 1000));
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
