﻿using System;
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

    public class EventDetail
    {
        public double rate { get; set; }
        public int interval { get; set; }
        public long last_event { get; set; }
    }

    public class Exchange
    {
        public string name { get; set; }
        public string vhost { get; set; }
    }
    public class Incoming
    {
        public MessageStats stats { get; set; }
        public Exchange exchange { get; set; }
    }

    public class MessageStats
    {
        public int ack { get; set; }
        public EventDetail ack_details { get; set; }
        public int deliver { get; set; }
        public EventDetail deliver_details { get; set; }
        public int deliver_get { get; set; }
        public EventDetail deliver_get_details { get; set; }
        public int redeliver { get; set; }
        public EventDetail redeliver_details { get; set; }
        public int publish { get; set; }
        public EventDetail publish_details { get; set; }
    }

    public class ChannelDetails
    {
        public string name { get; set; }
        public int number { get; set; }
        public string connection_name { get; set; }
        public int peer_port { get; set; }
        public string peer_host { get; set; }
    }
    public class QueueDetails
    {
        public string name { get; set; }
        public string vhost { get; set; }
    }

    public class ConsumerDetail
    {
        public ChannelDetails channel_details { get; set; }
        public QueueDetails queue_details { get; set; }
        public string consumer_tag { get; set; }
        public bool exclusive { get; set; }
        public bool ack_required { get; set; }
    }

    public class Delivery
    {
        public MessageStats stats { get; set; }
        public ChannelDetails channel_details { get; set; }
    }
    public class Arguments
    {
    }

    public class QueueInfo
    {
        public string policy { get; set; }
        public string exclusive_consumer_tag { get; set; }
        public int messages_ready { get; set; }
        public int messages_unacknowledged { get; set; }
        public int messages { get; set; }
        public int consumers { get; set; }
        public int active_consumers { get; set; }
        public int memory { get; set; }
        public BackingQueueStatus backing_queue_status { get; set; }
        public EventDetail messages_details { get; set; }
        public EventDetail messages_ready_details { get; set; }
        public EventDetail messages_unacknowledged_details { get; set; }
        public List<Incoming> incoming { get; set; }
        public List<Delivery> deliveries { get; set; }
        public MessageStats message_stats { get; set; }
        public List<ConsumerDetail> consumer_details { get; set; }
        public string name { get; set; }
        public string vhost { get; set; }
        public bool durable { get; set; }
        public bool auto_delete { get; set; }
        public Arguments arguments { get; set; }
        public string node { get; set; }
    }
}
