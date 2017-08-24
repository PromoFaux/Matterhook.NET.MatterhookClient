using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace Matterhook.NET.MatterhookClient
{
    public class MatterhookClient
    {
        private readonly Uri _webhookUrl;
        private readonly HttpClient _httpClient = new HttpClient();

        /// <summary>
        /// Create a new Mattermost Client
        /// </summary>
        /// <param name="webhookUrl">The URL of your Mattermost Webhook</param>
        /// <param name="timeoutSeconds">Timeout Value (Default 100)</param>
        public MatterhookClient(string webhookUrl, int timeoutSeconds = 100)
        {
            if (!Uri.TryCreate(webhookUrl, UriKind.Absolute, out _webhookUrl))
                throw new ArgumentException("Mattermost URL invalid");
           
            _httpClient.Timeout = new TimeSpan(0,0,0,timeoutSeconds);
        }

        public async Task<HttpResponseMessage> PostAsync(MattermostMessage message)
        {
            var serializedPayload = JsonConvert.SerializeObject(message);
            var response = await _httpClient.PostAsync(_webhookUrl,
                new StringContent(serializedPayload, Encoding.UTF8, "application/json"));

            return response;
        }

    }
}