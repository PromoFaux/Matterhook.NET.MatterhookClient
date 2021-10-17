using System.Collections.Generic;
using Newtonsoft.Json;

namespace Matterhook.NET.MatterhookClient
{
    public class MattermostMessage
    {
        //https://docs.mattermost.com/developer/webhooks-incoming.html

        /// <summary>
        /// Channel to post to
        /// </summary>
        [JsonProperty(PropertyName = "channel")]
        public string Channel { get; set; }

        /// <summary>
        /// Username for bot
        /// </summary>
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }

        /// <summary>
        /// Bot/User Icon
        /// </summary>
        [JsonProperty(PropertyName = "icon_url")]
        public string IconUrl { get; set; }

        /// <summary>
        /// Message body. Supports Markdown
        /// </summary>
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        /// <summary>
        /// Richtext attachments
        /// </summary>
        [JsonProperty(PropertyName = "attachments")]
        public List<MattermostAttachment> Attachments { get; set; }

        [JsonProperty(PropertyName = "props")]
        public MattermostProps Props { get; set; }

        internal MattermostMessage Clone()
        {
            return new MattermostMessage
            {
                Text = "",
                Channel = this.Channel,
                Username = this.Username,
                IconUrl = this.IconUrl
            };
        }


        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this, new MattermostJsonSerializerSettings());
        }
    }
}