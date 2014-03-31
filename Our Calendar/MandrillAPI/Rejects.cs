using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MandrillApi.Model;

namespace MandrillApi
{
    public partial class MandrillApi
    {
        private const string rejects = "rejects";

        /// <summary>Retrieves your email rejection blacklist. You can provide an email
        /// address to limit the results. Returns up to 1000 results</summary>
        /// <param name="key">the users api key</param>
        /// <param name="Email">an optional email address to search by</param>
        /// <returns>the information for each rejection blacklist entry</returns>
        public List<BlackList> list(string Email)
        {
            var request = new MandrillRestRequest
            {
                Method = RestSharp.Method.POST,
                Resource = string.Format("{0}/list.xml", tags)
            };
            request.AddBody(new { key = _ApiKey, email = Email });
            
            return Execute<List<BlackList>>(request);
        }

        /// <summary>Deletes an email rejection. There is no limit to how many rejections
        /// you can remove from your blacklist, but keep in mind that each deletion  
        /// has an affect on your reputation.</summary>
        /// <param name="key">the users api key</param>
        /// <param name="Email">an email address</param>
        /// <returns>a status object containing the address and whether the deletion succeeded.</returns>
        public List<DeletedSuccesses> delete(string Email)
        {
            var request = new MandrillRestRequest
            {
                Method = RestSharp.Method.POST,
                Resource = string.Format("{0}/delete.xml", tags)
            };
            request.AddBody(new { key = _ApiKey, email = Email });

            return Execute<List<DeletedSuccesses>>(request);
        }
    }
}
