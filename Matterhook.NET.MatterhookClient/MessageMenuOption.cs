using Newtonsoft.Json;

namespace Matterhook.NET.MatterhookClient
{
    /// <summary>
    /// Represents an option for a message menu
    /// </summary>
    public class MessageMenuOption
    {
        /// <summary>
        /// Creates an option 
        /// </summary>
        /// <param name="text">Option's name</param>
        /// <param name="value">Option's value</param>
        public MessageMenuOption(string text, string value)
        {
            Text = text;
            Value = value;
        }

        /// <summary>
        /// Option's name
        /// </summary>
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        /// <summary>
        /// Option's value
        /// </summary>
        [JsonProperty(PropertyName = "value")]
        public string Value  { get; set; }
    }
}