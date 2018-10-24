using Newtonsoft.Json;

namespace Matterhook.NET.MatterhookClient
{
    public class MattermostMessageMenuUsers : IMattermostAction
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
        public MattermostIntegration Integration { get; set; }

        /// <summary>
        /// Describes type of menu. For now only 'select'.
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string Type => "select";

        /// <summary>
        /// Indicates the source of the data for this menu.
        /// </summary>
        [JsonProperty(PropertyName = "data_source")]
        public string DataSource => "users";
    }
}