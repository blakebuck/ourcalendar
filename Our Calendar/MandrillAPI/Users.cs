using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;
using MandrillApi.Model;

namespace MandrillApi
{
    public partial class MandrillApi
    {
        private const string users = "users";

        /// <summary>"Ping" the Mandrill API - a simple method you can call that will
        /// return a constant value as long as everything is good.</summary>
        /// <param name="key">the users api key</param>
        /// <returns>the string "PONG!"</returns>
        public string Ping()
        {
            var request = new MandrillRestRequest
            {
                Method = RestSharp.Method.POST,
                Resource = string.Format("{0}/ping.json", users)
            };
            request.AddParameter("key", _ApiKey);

            return Execute(request).ToString();
        }

        /// <summary>Validate an API key and respond to a ping (anal JSON parser version)</summary>
        /// <param name="key">the users api key</param>
        /// <returns>a struct with one key "PING" with a static value "PONG!"</returns>
        public string Ping2()
        {
            var request = new MandrillRestRequest
            {
                Method = RestSharp.Method.POST,
                Resource = string.Format("{0}/ping2.xml", users)
            };
            request.AddParameter("key", _ApiKey);

            return Execute(request).ToString();
        }

        /// <summary> "Info" retrieves infomation about the user</summary>
        /// <param name="key">the users api key</param>
        /// <returns>Return the information about the API-connected user</returns>
        //public UserInformation info()
        public object info()    
        {
            var request = new MandrillRestRequest
            {
                Method = RestSharp.Method.POST,
                Resource = string.Format("{0}/info.xml", users)
            };
            request.AddParameter("key", _ApiKey);

            return Execute<UserInformation>(request);
        }

        /// <summary> "Sender" retrieves infomation who is sending</summary>
        /// <param name="key">the users api key</param>
        /// <returns>Return the senders that have tried to use this account, both verified and unverified</returns>
        public List<SenderData> senders()
        {
            var request = new MandrillRestRequest
            {
                Method = RestSharp.Method.POST,
                Resource = string.Format("{0}/senders.json", users)
            };
            request.AddParameter("key", _ApiKey);

            return Execute<List<SenderData>>(request);
        }
    }
}
