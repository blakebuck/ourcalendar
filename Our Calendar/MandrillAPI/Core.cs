using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;

namespace MandrillApi
{
    public partial class MandrillApi
    {
        private RestClient _client;
        private string _ApiKey;
        private const string _MandrillUrl = "http://mandrillapp.com/api/1.0/";
        private string _messageType;

        public MandrillApi(string ApiKey, string MessageType)
        {
            _client = new RestClient(_MandrillUrl);
            _ApiKey = ApiKey;
            _messageType = MessageType;
            //_client.AddHandler("application/xml; charset=utf-8", new ZenDeskXmlDeserializer());
            //_client.AddHandler("application/xml", new ZenDeskXmlDeserializer());                        
        }

        public T Execute<T>(MandrillRestRequest request) where T : new()
        {
            var response = _client.Execute<T>(request);


            return response.Data;
        }

        public string Execute(MandrillRestRequest request)
        {
            var res = _client.Execute(request);
            ValidateMandrillRestResponse((RestResponse)res);
            return res.Content;
        }

        protected void ValidateMandrillRestResponse(RestResponse response)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.NotAcceptable)
            {
                string error = "Mandrill could not handle the input you gave it";
                try
                {
                    //XmlDocument doc = new XmlDocument();
                    //doc.LoadXml(response.Content);
                    //error = doc.DocumentElement["error"].FirstChild.Value;
                }
                catch (Exception)
                { }

                throw new Exception(error);
            }
        }
    }
}
