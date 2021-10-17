using System.Collections.Generic;
using Newtonsoft.Json;

namespace Matterhook.NET.MatterhookClient
{
    /// <summary>
    /// Represents a Mattermost integration. See https://docs.mattermost.com/developer/interactive-messages.html />
    /// </summary>
    public class MattermostIntegration
    {
        public MattermostIntegration(string url, Dictionary<string, object> context)
        {
            Url = url;
            Context = context;
        }

        /// <summary>
        /// Determines where this action is sent to
        /// </summary>
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        /// <summary>
        /// Collection of key-value to store any data to be sent to the integration See https://docs.mattermost.com/developer/interactive-messages.html#parameters
        /// </summary>
        [JsonProperty(PropertyName = "context")]
        public Dictionary<string,object> Context { get; set; }
    }
}