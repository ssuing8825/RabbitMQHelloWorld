using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMonitoring
{
    public class TestResult
    {
        public StatusCode Status { get; set; }
        public String Description { get; set; }


    }
    public class SuccessResult : TestResult
    {
        public SuccessResult()
        {
            Status = StatusCode.OK;
        }
    }

    public class CriticalResult : TestResult
    {
        public CriticalResult()
        {
            Status = StatusCode.Critical;
        }
    }
}
