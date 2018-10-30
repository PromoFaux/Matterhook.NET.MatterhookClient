using Newtonsoft.Json;

namespace Matterhook.NET.MatterhookClient
{
    /// <summary>
    /// https://docs.mattermost.com/developer/interactive-messages.html
    /// </summary>
    public class MattermostAction : IMattermostAction
    {
        /// <summary>
        /// Action description. Will also appear as the button's text.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// The integration for this action
        /// </summary>
        [JsonProperty(PropertyName = "integration")]
        public MattermostIntegration Integration{ get; set; }
    }
}
