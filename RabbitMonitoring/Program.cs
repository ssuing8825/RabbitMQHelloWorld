
namespace RabbitMonitoring
{
    using Topshelf;

    internal class Program
    {
        internal static void Main(string[] args)
        {
            HostFactory.Run(hostConfigurator =>
            {
                hostConfigurator.Service<StatCollector>(serviceConfigurator =>
                {
                    serviceConfigurator.ConstructUsing(name => new StatCollector());
                    serviceConfigurator.WhenStarted(ew => ew.Start());
                    serviceConfigurator.WhenStopped(ew =>
                    {
                        ew.Stop();

                        // And dispose or release any component containers (e.g. Castle) 
                        // or items resolved from the container.
                    });
                });
                hostConfigurator.RunAsLocalSystem();
                hostConfigurator.SetDescription("RabbitMQ Monitoring Service");
                hostConfigurator.SetDisplayName("RabbitMQ Monitoring");
                hostConfigurator.SetServiceName("RabbitMQMonitoring"); // No spaces allowed
                hostConfigurator.StartAutomatically();
            });
        }
    }
}
