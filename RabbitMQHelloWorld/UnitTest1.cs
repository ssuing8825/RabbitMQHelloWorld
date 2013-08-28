using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Publisher;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using Subsciber;

namespace RabbitMQHelloWorld
{
    [TestClass]
    public class UnitTest1
    {
        public string HOST_NAME = "localhost";
        public string QUEUE_NAME = "helloWorld";

        private Consumer consumer;
        private Producer producer;

        [TestMethod]
        public void TestMethod1()
        {
            producer = new Producer(HOST_NAME, QUEUE_NAME);
            producer.SendMessage(System.Text.Encoding.UTF8.GetBytes("Hello World"));
        }

        [TestMethod]
        public void GetMessage()
        {
            //create the consumer
            consumer = new Consumer(HOST_NAME, QUEUE_NAME);

            //listen for message events
            consumer.onMessageReceived += handleMessage;

            //start consuming
            consumer.StartConsuming();
        }

        //delegate to post to UI thread
     //   private delegate void showMessageDelegate(string message);

        //Callback for message receive
        public void handleMessage(byte[] message)
        {
            Console.WriteLine(System.Text.Encoding.UTF8.GetString(message));
           // showMessageDelegate s = new showMessageDelegate(richTextBox1.AppendText);

          //  this.Invoke(s, System.Text.Encoding.UTF8.GetString(message) + Environment.NewLine);
        }


    }
}
