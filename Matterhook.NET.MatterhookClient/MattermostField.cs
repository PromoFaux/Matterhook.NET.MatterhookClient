using Newtonsoft.Json;

namespace Matterhook.NET.MatterhookClient
{
    /// <summary>
    /// https://docs.mattermost.com/developer/message-attachments.html#fieldshttps://docs.mattermost.com/developer/message-attachments.html#fields
    /// </summary>
    public class MattermostField
    {
        /// <summary>
        /// A title shown in the table above the value.
        /// </summary>
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        /// <summary>
        /// The text value of the field. It can be formatted using Markdown.
        /// </summary>
        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }

        /// <summary>
        /// Optionally set to “True” or “False” to indicate whether the value is short enough to be displayed beside other values.
        /// </summary>
        [JsonProperty(PropertyName = "short")]
        public bool Short { get; set; }
    }
}