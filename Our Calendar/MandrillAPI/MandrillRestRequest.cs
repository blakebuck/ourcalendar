using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;


namespace MandrillApi
{
    public class MandrillRestRequest : RestRequest
    {
        public MandrillRestRequest()
        {
            RequestFormat = DataFormat.Json;
            AddHeader("Content-Type", "application/xml");
        }
    }
}
