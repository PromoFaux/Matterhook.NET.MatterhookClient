using Newtonsoft.Json;

namespace Matterhook.NET.MatterhookClient
{
    public class MessageActionOption
    {
        public MessageActionOption(string text, string value)
        {
            Text = text;
            Value = value;
        }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value  { get; set; }
    }
}