namespace RabbitMonitoringCreateCounters
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;


    //http://flyingpies.wordpress.com/2011/04/07/performance-counters-types-with-the-net-framework/
    

    public class PerformanceCounterSetup
    {

        public static bool CheckCounters(string categoryName)
        {
            if (PerformanceCounterCategory.Exists(categoryName))
            {
                bool needToRecreateCategory = false;

                foreach (CounterCreationData counter in Counters)
                {
                    if (!PerformanceCounterCategory.CounterExists(counter.CounterName, categoryName))
                    {
                        needToRecreateCategory = true;
                    }
                }

                if (!needToRecreateCategory)
                {
                    return true;
                }
            }

            return false;
        }

        public static void SetupCounters(string categoryName)
        {
            try
            {
                PerformanceCounterCategory.Delete(categoryName);
            }
            catch (Win32Exception)
            {
                //Making sure this won't stop the process.
            }
            catch (Exception)
            {
                //Ignore exception.
                //We need to ensure that we attempt to delete category before recreating it. 
            }

            PerformanceCounterCategory.Create(categoryName, "RabbitMQ statistics",
                                              PerformanceCounterCategoryType.MultiInstance, Counters);
            PerformanceCounter.CloseSharedResources(); // http://blog.dezfowler.com/2007/08/net-performance-counter-problems.html
        }

        static readonly CounterCreationDataCollection Counters = new CounterCreationDataCollection
                    {
                        new CounterCreationData("Memory", 
                                                "Memory Used by Queue",
                                                PerformanceCounterType.NumberOfItems64),
                        new CounterCreationData("Number of Messages",
                                                "Number of Messages in Queue",
                                                PerformanceCounterType.NumberOfItems32),
                         new CounterCreationData("Number of Messages ready",
                                                "Number of Messages ready in Queue",
                                                PerformanceCounterType.NumberOfItems32),
                       new CounterCreationData("Number of Messages Unacknowledged",
                                                "Number of Messages unacknowledged in Queue",
                                                PerformanceCounterType.NumberOfItems32),
                       new CounterCreationData("Number Consumers",
                                                "Number Consumers",
                                                PerformanceCounterType.NumberOfItems32),
                       new CounterCreationData("Number Active Consumers",
                                                "Number Active Consumers",
                                                PerformanceCounterType.NumberOfItems32),

                       new CounterCreationData("avg_ingress_rate",
                                                "Number Active Consumers",
                                                PerformanceCounterType.NumberOfItems32),
                       new CounterCreationData("avg_egress_rate",
                                                "Number Active Consumers",
                                                PerformanceCounterType.NumberOfItems32),
                       new CounterCreationData("avg_ack_ingress_rate",
                                                "Number Active Consumers",
                                                PerformanceCounterType.NumberOfItems32),
                       new CounterCreationData("avg_ack_egress_rate",
                                                "Number Active Consumers",
                                                PerformanceCounterType.NumberOfItems32),


                       //new CounterCreationData("Average Messages",
                       //                         "Average Messages",
                       //                         PerformanceCounterType.AverageCount64),
                       //new CounterCreationData("Average Messages base",
                       //                         "Average Messages",
                       //                         PerformanceCounterType.AverageBase),

                        //new CounterCreationData("# of msgs successfully processed / sec",
                        //                        "The current number of messages processed successfully by the transport per second.",
                        //                        PerformanceCounterType.RateOfCountsPerSecond32),
                        //new CounterCreationData("# of msgs pulled from the input queue /sec",
                        //                        "The current number of messages pulled from the input queue by the transport per second.",
                        //                        PerformanceCounterType.RateOfCountsPerSecond32),
                        //new CounterCreationData("# of msgs failures / sec",
                        //                        "The current number of failed processed messages by the transport per second.",
                        //                        PerformanceCounterType.RateOfCountsPerSecond32)
                    };
    }
}