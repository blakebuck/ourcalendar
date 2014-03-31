using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MandrillApi.Model
{
    [DataContract]
    public class SenderDomains
    {
        [DataMember]
        public string domain { get; set; }
        [DataMember]
        public string created_at { get; set; }
    }
}
