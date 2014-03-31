using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MandrillApi.Model
{
    [DataContract]
    public class BlackList
    {
        [DataMember]
        private string email { get; set; }
        [DataMember]
        private string reason { get; set; }
        [DataMember]
        private string created_at { get; set; }
        [DataMember]
        private string expires_at { get; set; }
        [DataMember]
        private bool expired { get; set; }
        [DataMember]
        private List<string> Sender { get; set; }
    }

    [DataContract]
    public class DeletedSuccesses
    {
        [DataMember]
        private string email { get; set; }
        [DataMember]
        private bool deleted { get; set; }
    }
}
