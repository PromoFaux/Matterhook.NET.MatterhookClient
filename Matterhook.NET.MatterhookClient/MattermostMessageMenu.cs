using System.Collections.Generic;
using Newtonsoft.Json;

namespace Matterhook.NET.MatterhookClient
{
    /// <summary>
    /// A mattermost message menu. https://docs.mattermost.com/developer/interactive-messages.html#message-menus
    /// </summary>
    public class MattermostMessageMenu : IMattermostAction
    {
        /// <summary>
        /// Action description. Will also appear as the menu's text.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// The integration for this action
        /// </summary>
        [JsonProperty(PropertyName = "integration")]
        public MattermostIntegration Integration { get; set; }

        /// <summary>
        /// Describes type of menu. For now only 'select'.
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string Type => "select";

        /// <summary>
        /// The options for the menu.
        /// </summary>
        [JsonProperty(PropertyName = "options")]
        public List<MessageMenuOption> Options { get; set; }
    }
}