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
        private const string url = "urls";

        /// <summary>Get the 100 most clicked URLs</summary>
        /// <param name="key">the users api key</param>
        /// <returns>the 100 most clicked URLs and their stats</returns>        
        public List<URLStats> UrlList()
        {
            var request = new MandrillRestRequest
            {
                Method = RestSharp.Method.POST,
                Resource = string.Format("{0}/list.json", url)
            };
            request.AddParameter("key", _ApiKey);

            return Execute<List<URLStats>>(request);
        }

        /// <summary>Return the 100 most clicked URLs that match the search query given</summary>
        /// <param name="key">the users api key</param>
        /// <param name="q">a search query</param>
        /// <returns>the 100 most clicked URLs and their stats</returns>        
        public List<URLStats> UrlSearch(string q)
        {
            var request = new MandrillRestRequest
            {
                Method = RestSharp.Method.POST,
                Resource = string.Format("{0}/search.json", url)
            };
            request.AddBody(new { key = _ApiKey, q = q });

            return Execute<List<URLStats>>(request);
        }

        /// <summary>Return the recent history (hourly stats for the last 30 days) for a url</summary>
        /// <param name="key">the users api key</param>
        /// <param name="url">an existing url</param>
        /// <returns>the array of history information</returns>        
        public List<URLHistory> UrlTimeSeries(string url)
        {
            var request = new MandrillRestRequest
            {
                Method = RestSharp.Method.POST,
                Resource = string.Format("{0}/time-series.json", url)
            };
            request.AddParameter("key", _ApiKey);

            return Execute<List<URLHistory>>(request);
        }
    }
}
