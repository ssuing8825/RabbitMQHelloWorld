using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMonitoring
{
    public static class TelnetCheck
    {
        public static bool IsRabbitTelnet(string hostName, Int32 port)
        {
            using (var tcpClient = new TcpClient())
            {
                tcpClient.Connect(hostName, port);
                return tcpClient.Connected;
            }
        }
    }
}
