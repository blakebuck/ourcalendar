using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MandrillApi.Model
{
    [DataContract]
    public class Tags
    {
        [DataMember]
        private string tag { get; set; }
        [DataMember]
        private int sent { get; set; }
        [DataMember]
        private int hard_bounces { get; set; }
        [DataMember]
        private int soft_bounces { get; set; }
        [DataMember]
        private int rejects { get; set; }
        [DataMember]
        private int complaints { get; set; }
        [DataMember]
        private int unsubs { get; set; }
        [DataMember]
        private int opens { get; set; }
        [DataMember]
        private int clicks { get; set; }
    }

    [DataContract]
    public class DetailedTags
    {
        [DataMember]
        private string tag { get; set; }
        [DataMember]
        private int sent { get; set; }
        [DataMember]
        private int hard_bounces { get; set; }
        [DataMember]
        private int soft_bounces { get; set; }
        [DataMember]
        private int rejects { get; set; }
        [DataMember]
        private int complaints { get; set; }
        [DataMember]
        private int unsubs { get; set; }
        [DataMember]
        private int opens { get; set; }
        [DataMember]
        private int clicks { get; set; }
        [DataMember]
        private Days stats { get; set; }
    }

    [DataContract]
    public class TimeSeries
    {
        [DataMember]
        private string time { get; set; }
        [DataMember]
        private string tag { get; set; }
        [DataMember]
        private int sent { get; set; }
        [DataMember]
        private int hard_bounces { get; set; }
        [DataMember]
        private int soft_bounces { get; set; }
        [DataMember]
        private int rejects { get; set; }
        [DataMember]
        private int complaints { get; set; }
        [DataMember]
        private int opens { get; set; }
        [DataMember]
        private int unique_opens { get; set; }
        [DataMember]
        private int clicks { get; set; }
        [DataMember]
        private int unique_clicks { get; set; }
    }
}
