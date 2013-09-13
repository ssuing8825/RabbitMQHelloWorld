using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;

namespace RabbitMonitoringCreateCounters
{
    class Program
    {
        private static PerformanceCounter countPerTimeInterval32;

        static void Main(string[] args)
        {
            if (SetupCategory())
                return;
            CreateCounters();

     //       CollectSamples(samplesList);
      //      CalculateResults(samplesList);

        }

        private static bool SetupCategory()
        {
            if (!PerformanceCounterCategory.Exists("RabbitMQMonitoringCategory2"))
            {

                CounterCreationDataCollection counterDataCollection = new CounterCreationDataCollection();

                // Add the counter.
                var averageMessagesInQueue = new CounterCreationData();
                averageMessagesInQueue.CounterType = PerformanceCounterType.NumberOfItems32;
                averageMessagesInQueue.CounterName = "MessagesInQueue";
                counterDataCollection.Add(averageMessagesInQueue);

                // Create the category.
                PerformanceCounterCategory.Create("RabbitMQMonitoringCategory2",
                    "Demonstrates usage of the AverageCounter64 performance counter type.",
                    PerformanceCounterCategoryType.SingleInstance, counterDataCollection);

                return (true);
            }
            else
            {
                Console.WriteLine("Category exists - MessagesInQueue");
                return (false);
            }
        }
        private static void CreateCounters()
        {
            // Create the counters.

            countPerTimeInterval32 = new PerformanceCounter("RabbitMQMonitoringCategory2",
                "MessagesInQueue",
                false);


            //avgCounter64SampleBase = new PerformanceCounter("AverageCounter64SampleCategory",
            //    "AverageCounter64SampleBase",
            //    false);


            countPerTimeInterval32.RawValue = 0;
            
        }
    }
}

