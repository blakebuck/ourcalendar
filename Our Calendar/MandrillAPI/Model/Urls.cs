using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MandrillApi.Model
{
    [DataContract]
    public class URLStats
    {
        [DataMember]
        public string url { get; set; }
        [DataMember]
        public int sent { get; set; }
        [DataMember]
        public int clicks { get; set; }
        [DataMember]
        public int unique_clicks { get; set; }
    }

    [DataContract]
    public class URLHistory
    {
        [DataMember]
        public string time { get; set; }
        [DataMember]
        public int sent { get; set; }
        [DataMember]
        public int clicks { get; set; }
        [DataMember]
        public int unique_clicks { get; set; }
    }
}
