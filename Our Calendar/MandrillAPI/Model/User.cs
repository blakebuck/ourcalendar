using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MandrillApi.Model
{
    [DataContract]
    public class SenderData
    {
        [DataMember]
        public string address { get; set; }
        [DataMember]
        public string created_at { get; set; }
        [DataMember]
        public string sent { get; set; }
        [DataMember]
        public bool is_enabled { get; set; }
        [DataMember]
        public int hard_bounces { get; set; }
        [DataMember]
        public int soft_bounces { get; set; }
        [DataMember]
        public int rejects { get; set; }
        [DataMember]
        public int complaints { get; set; }
        [DataMember]
        public int unsubs { get; set; }
        [DataMember]
        public int opens { get; set; }
        [DataMember]
        public int clicks { get; set; }
    }

    [DataContract]
    public class SendingStats
    {
        [DataMember]
        public int sent { get; set; }
        [DataMember]
        public int hard_bounces { get; set; }
        [DataMember]
        public int soft_bounces { get; set; }
        [DataMember]
        public int rejects { get; set; }
        [DataMember]
        public int complaints { get; set; }
        [DataMember]
        public int unsubs { get; set; }
        [DataMember]
        public int opens { get; set; }
        [DataMember]
        public int unique_opens { get; set; }
        [DataMember]
        public int clicks { get; set; }
        [DataMember]
        public int unique_clicks { get; set; }
    }

    [DataContract]
    public class Days
    {
        [DataMember]
        public SendingStats today { get; set; }
        [DataMember]
        public SendingStats last_7_days { get; set; }
        [DataMember]
        public SendingStats last_30_days { get; set; }
        [DataMember]
        public SendingStats last_60_days { get; set; }
        [DataMember]
        public SendingStats last_90_days { get; set; }
        [DataMember]
        public SendingStats all_time { get; set; }
        
    }

    [DataContract]
    public class UserInformation
    {       
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string created_at { get; set; }
        [DataMember]
        public string public_id { get; set; }
        [DataMember]
        public int reputation { get; set; }
        [DataMember]
        public int hourly_quota { get; set; }
        [DataMember]
        public int backlog { get; set; }
        [DataMember]
        public Days stats { get; set; }
    }
}
