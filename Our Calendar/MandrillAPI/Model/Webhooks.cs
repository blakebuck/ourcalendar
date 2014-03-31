using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MandrillApi.Model
{
    [DataContract]
    public class WebhookInfo
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string url { get; set; }
        [DataMember]
        public string created_at { get; set; }
        [DataMember]
        public string last_sent_at { get; set; }
        [DataMember]
        public int batches_sent { get; set; }
        [DataMember]
        public int events_sent { get; set; }
        [DataMember]
        public string last_error { get; set; }
    }
}
