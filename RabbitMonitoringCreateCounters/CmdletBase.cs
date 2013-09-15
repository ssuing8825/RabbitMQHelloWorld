using System.Security.Principal;

namespace RabbitMonitoringCreateCounters
{
    using System.Management.Automation;
    using System.Security;
  
    public abstract class CmdletBase : PSCmdlet
    {
        protected override void BeginProcessing()
        {
            if (!ProcessUtil.IsRunningWithElevatedPriviliges())
            {
                ThrowTerminatingError(new ErrorRecord(new SecurityException("You need to run this command with administrative rights."), "NotAuthorized", ErrorCategory.SecurityError, null));
            }
        }
    }

    public static class ProcessUtil
    {
        public static bool IsRunningWithElevatedPriviliges()
        {
            return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
