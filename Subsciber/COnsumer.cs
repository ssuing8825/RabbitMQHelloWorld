﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;


//http://simonwdixon.wordpress.com/2011/05/08/getting-started-with-rabbitmq-in-dotnet/


namespace Subsciber
{
    public class Consumer
    {
        protected IModel Model;
        protected IConnection Connection;
        protected string QueueName;

        protected bool isConsuming;

        // used to pass messages back to UI for processing
        public delegate void onReceiveMessage(byte[] message);
        public event onReceiveMessage onMessageReceived;

        public Consumer(string hostName, string queueName)
        {
            QueueName = queueName;
            ConnectionFactory connectionFactory = new ConnectionFactory();
            connectionFactory.HostName = hostName;
            Connection = connectionFactory.CreateConnection();
            Model = Connection.CreateModel();
            Model.QueueDeclare(QueueName, false, false, false, null);
        }

        //internal delegate to run the queue consumer on a seperate thread
        private delegate void ConsumeDelegate();

        public void StartConsuming()
        {
            isConsuming = true;
            ConsumeDelegate c = new ConsumeDelegate(Consume);
            c.BeginInvoke(null, null);
        }

        public void Consume()
        {
            QueueingBasicConsumer consumer = new QueueingBasicConsumer(Model);
            String consumerTag = Model.BasicConsume(QueueName, false, consumer);
            while (isConsuming)
            {
                try
                {
                    RabbitMQ.Client.Events.BasicDeliverEventArgs e = (RabbitMQ.Client.Events.BasicDeliverEventArgs)consumer.Queue.Dequeue();
                    IBasicProperties props = e.BasicProperties;
                    byte[] body = e.Body;
                    // ... process the message
                    onMessageReceived(body);
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

        public void Dispose()
        {
            isConsuming = false;
            if (Connection != null)
                Connection.Close();
            if (Model != null)
                Model.Abort();
        }
    }
}
