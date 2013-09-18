using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMonitoring
{
    public static class PingCheck
    {
        public static bool IsRabbitPingable(string hostName)
        {
            var pingSender = new Ping();
            var timeout = 120;
            // Use the default Ttl value which is 128, 
            // but change the fragmentation behavior.
            var options = new PingOptions { DontFragment = true };
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);

            var reply = pingSender.Send(hostName, timeout, buffer, options);
            return reply.Status == IPStatus.Success;
        }
    }
}
