using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MandrillApi.Model
{
    //[DataContract]
    //public class TemplateContent
    //{
    //    [DataMember]
    //    public string name { get; set; }
    //    [DataMember]
    //    public string content { get; set; }
    //}

    [DataContract]
    public class Recipient
    {
        [DataMember]
        public string email { get; set; }
        [DataMember]
        public string name { get; set; }
    }

    [DataContract]
    public class RecipientReturn
    {
        [DataMember]
        public string email { get; set; }
        [DataMember]
        public string status { get; set; }
    }

    [DataContract]
    public class Attachments
    {
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string content { get; set; }
    }

    [DataContract]
    public class SearchReturn
    {
        [DataMember]
        public int ts { get; set; }
        [DataMember]
        public string sender { get; set; }
        [DataMember]
        public string subject { get; set; }
        [DataMember]
        public string email { get; set; }
        [DataMember]
        public string[] tags { get; set; }
        [DataMember]
        public int opens { get; set; }
        [DataMember]
        public int clicks { get; set; }
        [DataMember]
        public string state { get; set; }
    }
}
