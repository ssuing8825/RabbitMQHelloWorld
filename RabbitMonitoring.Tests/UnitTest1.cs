using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RabbitMonitoring.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestConnection()
        {
            Assert.IsTrue(new AmqpPingCheck().IsConnected("localhost", "guest", "guest"));
        }

        [TestMethod]
        public void PingTest()
        {
            Assert.IsTrue(PingCheck.IsRabbitPingable("localhost"));
        }

        [TestMethod]
        public void TelNetTest()
        {
            Assert.IsTrue(TelnetCheck.IsRabbitTelnet("localhost", 15672));
        }

        [TestMethod]
        public void AlivenessTestTest()
        {
            var ac = new AlivenessChecker();

            Assert.IsTrue(ac.IsAlive("localhost", "guest", "guest"));

        }
    }
}
