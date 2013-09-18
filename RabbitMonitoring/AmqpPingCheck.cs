using System;
using RabbitMQ.Client;

namespace RabbitMonitoring
{
    public class AmqpPingCheck : IDisposable
    {
        IConnection connection = null;
        bool disposed;

        public bool IsConnected(string hostName, string userName, string password)
        {
            try
            {
                var connectionFactory = new ConnectionFactory();
                connectionFactory.HostName = hostName;
                connectionFactory.UserName = userName;
                connectionFactory.Password = password;
                connection = connectionFactory.CreateConnection();
                var model = connection.CreateModel();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                // Dispose managed resources.
                if (connection == null)
                {
                    return;
                }

                connection.Dispose();
                connection = null;
            }

            disposed = true;
        }

        ~AmqpPingCheck()
        {
            Dispose(false);
        }
    }
}
