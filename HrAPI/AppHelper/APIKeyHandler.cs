using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace HrAPI.Helper
{
    public class APIKeyHandler : DelegatingHandler
    {
        private const string keyName = "HBLAPIKEY";
        private const string apiKey = "VMSAPI@789";


        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
            bool isValidAPIKey = false;
            IEnumerable<string> lsHeaders;

            var checkApiKeyExists = request.Headers.TryGetValues(keyName, out lsHeaders);

            if (checkApiKeyExists)
            {
                if (lsHeaders.FirstOrDefault().Equals(apiKey))
                {
                    isValidAPIKey = true;
                }
            }

            if (!isValidAPIKey)
                return request.CreateResponse(HttpStatusCode.Forbidden, "Bad API Key");

            var response = await base.SendAsync(request, cancellationToken);

            return response;
        }
    }
}