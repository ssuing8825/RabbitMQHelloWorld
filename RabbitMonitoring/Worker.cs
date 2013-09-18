using System;
using System.Globalization;
using System.Threading;


namespace RabbitMonitoring
{
    public abstract class Worker
    {
        private Thread thread;

        private bool stop = true;

        protected Worker()
        {
            this.SleepPeriod = new TimeSpan(0, 0, 0, 1);
            this.Id = Guid.NewGuid();
        }

        public TimeSpan SleepPeriod { get; set; }

        public bool IsStopped { get; private set; }

        protected Guid Id { get; private set; }

        public void Start()
        {
            string logMessage = string.Format(CultureInfo.CurrentCulture, "Starting worker of type '{0}'.", this.GetType().FullName);
            System.Diagnostics.Debug.WriteLine(logMessage);
            this.stop = false;

            // Multiple thread instances cannot be created
            if (this.thread == null || this.thread.ThreadState == ThreadState.Stopped)
            {
                this.thread = new Thread(this.Run);
            }

            // Start thread if it's not running yet
            if (this.thread.ThreadState != ThreadState.Running)
            {
                this.thread.Start();
            }
        }

        public void Stop()
        {
            string logMessage = string.Format(CultureInfo.CurrentCulture, "Stopping worker of type '{0}'.", this.GetType().FullName);
            System.Diagnostics.Debug.WriteLine(logMessage);
            this.stop = true;
        }

        protected abstract void DoWork();

        private void Run()
        {
            try
            {
                try
                {
                    while (!this.stop)
                    {
                        this.IsStopped = false;
                        this.DoWork();
                        Thread.Sleep(this.SleepPeriod);
                    }
                }
                catch (ThreadAbortException)
                {
                    Thread.ResetAbort();
                }
                finally
                {
                    this.thread = null;
                    this.IsStopped = true;
                    string logMessage = string.Format(CultureInfo.CurrentCulture, "Stopped worker of type '{0}'.", this.GetType().FullName);
                    System.Diagnostics.Debug.WriteLine(logMessage);
                }
            }
            catch (Exception e)
            {
                string exceptionMessage = string.Format(CultureInfo.CurrentCulture, "Error running the '{0}' worker.", this.GetType().FullName);
                System.Diagnostics.Debug.WriteLine(exceptionMessage, e);
                throw;
            }
        }
    }
}
