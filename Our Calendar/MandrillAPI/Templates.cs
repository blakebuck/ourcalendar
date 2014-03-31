using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MandrillApi.Model;
using System.Web.Script.Serialization;

namespace MandrillApi
{
    public partial class MandrillApi
    {
        private const string templates = "templates";

        /// <summary>Add a new template</summary>
        /// <param name="key">the users api key</param>
        /// <param name="name">the name for the new template - must be unique</param>
        /// <param name="code">the HTML code with mc:edit attributes for the editable elements</param>
        /// <returns>the information saved about the new template</returns>
        public TemplateInfo Templateadd(string name, string code)
        {
            var request = new MandrillRestRequest
            {
                Method = RestSharp.Method.POST,
                Resource = string.Format("{0}/add.json", templates)
            };
            request.AddBody(new { key = _ApiKey, name = name, code = code });

            return Execute<TemplateInfo>(request);
        }

        /// <summary>Get the information for an existing template</summary>
        /// <param name="key">the users api key</param>
        /// <param name="name">the name of an existing template - must be unique</param>
        /// <returns>the information saved about the template</returns>
        public TemplateInfo Templateinfo(string name)
        {
            var request = new MandrillRestRequest
            {
                Method = RestSharp.Method.POST,
                Resource = string.Format("{0}/info.json", templates)
            };
            request.AddBody(new { key = _ApiKey, name = name });

            return Execute<TemplateInfo>(request);
        }

        /// <summary>Update the code for an existing template</summary>
        /// <param name="key">the users api key</param>
        /// <param name="name">the name of an existing template - must be unique</param>
        /// <param name="code">the new HTML code with mc:edit attributes for the editable elements</param>
        /// <returns>the information saved about the updated template</returns>
        public TemplateInfo update(string name, string code)
        {
            var request = new MandrillRestRequest
            {
                Method = RestSharp.Method.POST,
                Resource = string.Format("{0}/update.json", templates)
            };
            request.AddBody(new { key = _ApiKey, name = name, code = code });

            return Execute<TemplateInfo>(request);
        }

        /// <summar>Delete a template</summar></summary>
        /// <param name="key">the users api key</param>
        /// <param name="name">the name of an existing template</param>
        /// <returns>the information saved about the deleted template</returns>
        public TemplateInfo Templatedelete(string name)
        {
            var request = new MandrillRestRequest
            {
                Method = RestSharp.Method.POST,
                Resource = string.Format("{0}/delete.json", templates)
            };
            request.AddBody(new { key = _ApiKey, name = name });

            return Execute<TemplateInfo>(request);
        }

        /// <summar>Delete a template</summar></summary>
        /// <param name="key">the users api key</param>
        /// <returns>an array of the information about each template</returns>
        public List<TemplateInfo> Templatelist()
        {
            var request = new MandrillRestRequest
            {
                Method = RestSharp.Method.POST,
                Resource = string.Format("{0}/list.json", templates)
            };
            request.AddParameter("key", _ApiKey);

            return Execute<List<TemplateInfo>>(request);
        }

        /// <summar>Return the recent history (hourly stats for the last 30 days) for a template</summar></summary>
        /// <param name="key">the users api key</param>
        /// <param name="name">the name of an existing template</param>
        /// <returns>the array of history information</returns>
        public List<TemplateTimeSeries> TemplateTimeSeries(string name)
        {
            var request = new MandrillRestRequest
            {
                Method = RestSharp.Method.POST,
                Resource = string.Format("{0}/time-series.json", templates)
            };
            request.AddParameter("key", _ApiKey);

            return Execute<List<TemplateTimeSeries>>(request);
        }
    }
}
