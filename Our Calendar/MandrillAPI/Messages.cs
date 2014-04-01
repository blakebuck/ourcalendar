using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MandrillApi.Model;
using System.Net;
using System.IO;

namespace MandrillApi
{
    public partial class MandrillApi
    {
        private const string messages = "messages";

        /// <summary>Send will send a new transactional message through Mandrill</summary>
        /// <param name="key">the users api key</param>
        /// <param name="T">The message data</param>
        /// <returns>A struct of type RecipientReturn</returns>
        public List<RecipientReturn> send(object T)
        {
            var request = new MandrillRestRequest
            {
                Method = RestSharp.Method.POST,
                Resource = string.Format("{0}/send.json", messages)
            };
            request.AddBody(new { key = _ApiKey, message = T });

            return Execute<List<RecipientReturn>>(request);
        }

        /// <summary>Sendtemplate will Send a new transactional message through Mandrill using a template</summary>
        /// <param name="key">the users api key</param>
        /// <param name="TemplateName">A string value of the name of the template</param>
        /// <param name="templateContent">An array of template content</param>
        /// <param name="message">A message struct of the other info to send (same as messagesend w/out the html)</param>
        /// <returns>A struct of type RecipientReturn</returns>
        public List<RecipientReturn> sendtemplate(string TemplateName, List<object> templateContent, object message)
        {
            var request = new MandrillRestRequest
            {
                Method = RestSharp.Method.POST,
                Resource = string.Format("{0}/send-template.xml", messages)
            };
            request.AddBody(new { key = _ApiKey, template_name = TemplateName, template_content = templateContent, message = message });
            return Execute<List<RecipientReturn>>(request);
        }

        /// <summary>Search the content of recently sent messages and optionally narrow by date range, tags and senders</summary>
        /// <param name="key">the users api key</param>
        /// <param name="query">the search items to find matching messages to</param>
        /// <param name="date_from">start date</param>
        /// <param name="date_to">end date</param>
        /// <param name="tags">array of string containing tag names</param>
        /// <param name="senders">array of senders addresses</param>
        /// <param name="limit">maximum number of results to return</param>
        /// <returns>A struct of type RecipientReturn</returns>
        public List<SearchReturn> search(string query, string date_from, string date_to, List<string> tags, List<string> senders, int limit)
        {
            var request = new MandrillRestRequest
            {
                Method = RestSharp.Method.POST,
                Resource = string.Format("{0}/search.xml", messages)
            };
            request.AddBody(new {key = _ApiKey, query = query, date_from = date_from, tags = tags, senders = senders, limit = limit});

            return Execute<List<SearchReturn>>(request);
        }
    }
}
