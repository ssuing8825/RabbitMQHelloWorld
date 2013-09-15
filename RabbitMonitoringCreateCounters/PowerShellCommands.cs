using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMonitoringCreateCounters
{
    using System;
    using System.Management.Automation;
    
    [Cmdlet(VerbsLifecycle.Install, "RabbitQueuePerformanceCounters", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.Medium)]
    public class InstallPerformanceCounters : CmdletBase
    {

        private string queueName;


        [Parameter(Position = 0,
                Mandatory = true,
                ValueFromPipeline = true,
                ValueFromPipelineByPropertyName = true,
                HelpMessage = "Enter filter by queue name")]
        [Alias("QueueName")]
        public string Name
        {
            set { queueName = value; }
        }


        protected override void ProcessRecord()
        {
            if (ShouldProcess(Environment.MachineName))
            {
                PerformanceCounterSetup.SetupCounters(String.Format("Rabbit.{0}", queueName));
            }
        }
    }

    [Cmdlet(VerbsDiagnostic.Test, "NServiceBusPerformanceCountersInstallation")]
    public class ValidatePerformanceCounters : CmdletBase
    {
        private string queueName;


        [Parameter(Position = 0,
                Mandatory = true,
                ValueFromPipeline = true,
                ValueFromPipelineByPropertyName = true,
                HelpMessage = "Enter filter by queue name")]
        [Alias("QueueName")]
        public string Name
        {
            set { queueName = value; }
        }
        protected override void ProcessRecord()
        {
            var countersAreGood = PerformanceCounterSetup.CheckCounters(String.Format("Rabbit.{0}", queueName));

            WriteVerbose(countersAreGood
                             ? "NServiceBus Performance Counters are setup and ready for use with NServiceBus."
                             : "NServiceBus Performance Counters are not properly configured.");

            WriteObject(countersAreGood);
        }
    }
}
