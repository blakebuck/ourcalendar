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
        private const string sender = "senders";

        /// <summary>"SenderList" returns a list of senders</summary>
        /// <param name="key">the users api key</param>
        /// <returns>Return the senders that have tried to use this account.</returns>
        public List<SenderData> SenderList()
        {
            var request = new MandrillRestRequest
            {
                Method = RestSharp.Method.POST,
                Resource = string.Format("{0}/list.json", sender)
            };
            request.AddParameter("key", _ApiKey);

            return Execute<List<SenderData>>(request);
        }

        /// <summary>"Domains" Returns the sender domains that have been added to this account.</summary>
        /// <param name="key">the users api key</param>
        /// <returns>Return the senders that have tried to use this account.</returns>
        public List<SenderDomains> SenderDomains()
        {
            var request = new MandrillRestRequest
            {
                Method = RestSharp.Method.POST,
                Resource = string.Format("{0}/domains.json", sender)
            };
            request.AddParameter("key", _ApiKey);

            return Execute<List<SenderDomains>>(request);
        }

        /// <summary>Info returns more detailed information about a single sender,
        /// including aggregates of recent stats</summary>
        /// <param name="key">the user api key</param>
        /// <param name="address">The email address of the sender</param>
        /// <returns>a struct of the detailed information about the sender</returns>
        public List<SenderData> SenderInfo(string Address)
        {
            var request = new MandrillRestRequest
            {
                Method = RestSharp.Method.POST,
                Resource = string.Format("{0}/info.json", sender)
            };
            request.AddBody(new { key = _ApiKey, address = Address });

            return Execute<List<SenderData>>(request);
        }

        /// <summary>"time-series" Return the recent history (hourly stats for the last 30 days) for a sender</summary>
        /// <param name="key">the users api key</param>
        /// <param name="address">The email address of the sender</param>
        /// <returns>the array of history information</returns>
        public List<TimeSeries> SenderTimeSeries(string Address)
        {
            var request = new MandrillRestRequest
            {
                Method = RestSharp.Method.POST,
                Resource = string.Format("{0}/time-series.json", sender)
            };
            request.AddBody(new { key = _ApiKey, address = Address });

            return Execute<List<TimeSeries>>(request);
        }
    }
}
