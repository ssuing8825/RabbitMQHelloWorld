using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMonitoringCreateCounters
{
    using System;
    using System.Management.Automation;
    
    [Cmdlet(VerbsLifecycle.Install, "NServiceBusPerformanceCounters", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.Medium)]
    public class InstallPerformanceCounters : CmdletBase
    {
        protected override void ProcessRecord()
        {
            if (ShouldProcess(Environment.MachineName))
            {
                PerformanceCounterSetup.SetupCounters();
            }
        }
    }

    [Cmdlet(VerbsDiagnostic.Test, "NServiceBusPerformanceCountersInstallation")]
    public class ValidatePerformanceCounters : CmdletBase
    {
        protected override void ProcessRecord()
        {
            var countersAreGood = PerformanceCounterSetup.CheckCounters();

            WriteVerbose(countersAreGood
                             ? "NServiceBus Performance Counters are setup and ready for use with NServiceBus."
                             : "NServiceBus Performance Counters are not properly configured.");

            WriteObject(countersAreGood);
        }
    }
}
