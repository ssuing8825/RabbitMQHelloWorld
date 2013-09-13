using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMonitoring
{
    public class BackingQueueStatus
    {
        public int q1 { get; set; }
        public int q2 { get; set; }
        public List<object> delta { get; set; }
        public int q3 { get; set; }
        public int q4 { get; set; }
        public int len { get; set; }
        public int pending_acks { get; set; }
        public string target_ram_count { get; set; }
        public int ram_msg_count { get; set; }
        public int ram_ack_count { get; set; }
        public int next_seq_id { get; set; }
        public int persistent_count { get; set; }
        public double avg_ingress_rate { get; set; }
        public double avg_egress_rate { get; set; }
        public double avg_ack_ingress_rate { get; set; }
        public double avg_ack_egress_rate { get; set; }
    }

    public class Arguments
    {
    }

    public class QueueInfo
    {
        public int memory { get; set; }
        public string idle_since { get; set; }
        public string policy { get; set; }
        public string exclusive_consumer_tag { get; set; }
        public int messages_ready { get; set; }
        public int messages_unacknowledged { get; set; }
        public int messages { get; set; }
        public int consumers { get; set; }
        public int active_consumers { get; set; }
        public BackingQueueStatus backing_queue_status { get; set; }
        public List<object> consumer_details { get; set; }
        public string name { get; set; }
        public string vhost { get; set; }
        public bool durable { get; set; }
        public bool auto_delete { get; set; }
        public Arguments arguments { get; set; }
        public string node { get; set; }
    }
}
