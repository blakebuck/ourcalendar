using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MandrillApi.Model;

namespace MandrillApi
{
    public partial class MandrillApi
    {
        private const string webhooks = "webhooks";

        /// <summary>Get the list of all webhooks defined on the account</summary>
        /// <param name="key">the users api key</param>
        /// <returns>A struct of the webhook information</returns>
        public WebhookInfo Webhookslist()
        {
            var request = new MandrillRestRequest
            {
                Method = RestSharp.Method.POST,
                Resource = string.Format("{0}/list.json", webhooks)
            };

            request.AddBody(new { key = _ApiKey } );
            return Execute<WebhookInfo>(request);
        }


        /// <summary>Add a new webhook</summary>
        /// <param name="key">the users api key</param>
        /// <param name="url">the URL to POST batches of events</param>
        /// <param name="T">an optional list of events that will be posted to the webhook</param>
        /// <returns>the information saved about the new webhook</returns>
        public WebhookInfo Webhooksadd(string url, List<object> T)
        {
            var request = new MandrillRestRequest
            {
                Method = RestSharp.Method.POST,
                Resource = string.Format("{0}/add.json", webhooks)
            };
            request.AddBody(new { key = _ApiKey, message = T });

            return Execute<WebhookInfo>(request);
        }

        /// <summary>Given the ID of an existing webhook, return the data about it</summary>
        /// <param name="key">the users api key</param>
        /// <param name="id">the unique identifier of a webhook belonging to this account</param>
        /// <returns>Return the information about the Webhook</returns>
        public WebhookInfo Webhooksinfo(int id)
        {
            var request = new MandrillRestRequest
            {
                Method = RestSharp.Method.POST,
                Resource = string.Format("{0}/info.json", webhooks)
            };
            request.AddBody(new { key = _ApiKey, id = id });

            return Execute<WebhookInfo>(request);
        }

        /// <summary>Update an existing webhook</summary>
        /// <param name="key">the users api key</param>
        /// <param name="id">the unique identifier of a webhook belonging to this account</param>
        /// <param name="url">the URL to POST batches of events</param>
        /// <param name="T">an optional list of events that will be posted to the webhook</param>
        /// <returns>the information for the updated webhook</returns>
        public WebhookInfo Webhooksupdate(int id, string url, List<object> T)
        {
            var request = new MandrillRestRequest
            {
                Method = RestSharp.Method.POST,
                Resource = string.Format("{0}/update.json", webhooks)
            };
            request.AddBody(new { key = _ApiKey, id = id, url = url, events = T });

            return Execute<WebhookInfo>(request);
        }

        /// <summary>Delete an existing webhook</summary>
        /// <param name="key">the users api key</param>
        /// <param name="id">the unique identifier of a webhook belonging to this account</param>
        /// <returns>the information for the deleted webhook</returns>
        public WebhookInfo Webhooksdelete(int id)
        {
            var request = new MandrillRestRequest
            {
                Method = RestSharp.Method.POST,
                Resource = string.Format("{0}/delete.json", webhooks)
            };
            request.AddBody(new { key = _ApiKey, id = id });

            return Execute<WebhookInfo>(request);
        }
    }
}
