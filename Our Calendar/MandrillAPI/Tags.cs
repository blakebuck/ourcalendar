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
        private const string tags = "tags";

        /// <summaryReturn all of the user-defined tag information</summary>
        /// <param name="key">the users api key</param>
        /// <returns>Return the 100 most clicked URLs that match the search query given</returns>        
        public List<Tags> list()
        {
            var request = new MandrillRestRequest
            {
                Method = RestSharp.Method.POST,
                Resource = string.Format("{0}/list.json", tags)
            };
            request.AddBody(new { key = _ApiKey });

            return Execute<List<Tags>>(request);
        }

        /// <summaryReturn all of the user-defined tag information</summary>
        /// <param name="key">the users api key</param>
        /// <param name="tag"> an existing tag name</param>
        /// <returns>struct of the detailed information about the tag</returns>        
        public List<DetailedTags> info(string tag)
        {
            var request = new MandrillRestRequest
            {
                Method = RestSharp.Method.POST,
                Resource = string.Format("{0}/info.json", tags)
            };
            request.AddBody(new { key = _ApiKey, tag = tag });

            return Execute<List<DetailedTags>>(request);
        }

        /// <summaryReturn the recent history (hourly stats for the last 30 days) for a tag</summary>
        /// <param name="key">the users api key</param>
        /// <param name="tag"> an existing tag name</param>
        /// <returns>struct of the detailed information about the tag</returns>        
        public List<TimeSeries> TimeSeries(string tag)
        {
            var request = new MandrillRestRequest
            {
                Method = RestSharp.Method.POST,
                Resource = string.Format("{0}/time-series.json", tags)
            };
            request.AddBody(new { key = _ApiKey, tag = tag });

            return Execute<List<TimeSeries>>(request);
        }

        /// <summaryReturn the recent history (hourly stats for the last 30 days) for a tag</summary>
        /// <param name="key">the users api key</param>
        /// <returns>struct of the detailed information about the tag</returns>        
        public List<TimeSeries> AllTimeSeries()
        {
            var request = new MandrillRestRequest
            {
                Method = RestSharp.Method.POST,
                Resource = string.Format("{0}/all-time-series.json", tags)
            };
            request.AddBody(new { key = _ApiKey });

            return Execute<List<TimeSeries>>(request);
        }
    }
}
