using Newtonsoft.Json;

namespace Matterhook.NET.MatterhookClient
{
    /// <summary>
    /// Represents a Mattermost integration. See https://developers.mattermost.com/integrate/other-integrations/incoming-webhooks/ />
    /// </summary>
    public class MattermostProps
    {
        /// <summary>
        /// Markdown formated texst
        /// </summary>
        [JsonProperty(PropertyName = "card")]
        public string Card { get; set; }
    }
}

