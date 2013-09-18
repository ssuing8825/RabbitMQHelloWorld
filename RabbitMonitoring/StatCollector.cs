using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Diagnostics;

namespace RabbitMonitoring
{
    public class StatCollector : Worker
    {
        private string hostName = "localhost";
        private string userName = "guest";
        private string password = "guest";
        private EventLog _logger = new EventLog("RabbitMQ", System.Environment.MachineName,"Monitoring");

        protected override void DoWork()
        {

            using (var pingCheck = new AmqpPingCheck())
            {
                if (!pingCheck.IsConnected(hostName, userName, password))
                {
                    _logger.WriteEntry("Is Alive Failed.", EventLogEntryType.Error,500);
                }
                else
                {
                    _logger.WriteEntry("Connection to RabbitMQ Successful.", EventLogEntryType.SuccessAudit,100);
                }
            }

            if (!PingCheck.IsRabbitPingable("localhost"))
            {
                _logger.WriteEntry("Ping Check Failed Failed.", EventLogEntryType.Error, 500);
            }
            else
            {
                _logger.WriteEntry("Ping to RabbitMQ Successful.", EventLogEntryType.SuccessAudit, 100);
            }


            //if (!TelnetCheck.IsRabbitTelnet("localhost", 15672))
            //{
            //    _logger.WriteEntry("Telnet Check Failed Failed.", EventLogEntryType.Error, 500);
            //}
            //else
            //{
            //    _logger.WriteEntry("Telnet to RabbitMQ Successful.", EventLogEntryType.SuccessAudit, 100);
            //}


            var queueLogger = new QueueChecker();
            queueLogger.LogQueueInfo(hostName, userName, password,"HelloWorld");




           
        }
    }
}
