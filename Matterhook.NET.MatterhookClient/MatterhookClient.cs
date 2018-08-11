using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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

            _httpClient.Timeout = new TimeSpan(0, 0, 0, timeoutSeconds);
        }

        private MattermostMessage CloneMessage(MattermostMessage inMsg)
        {
            var outMsg = new MattermostMessage
            {
                Text = "",
                Channel = inMsg.Channel,
                Username = inMsg.Username,
                IconUrl = inMsg.IconUrl
            };

            return outMsg;
        }

        private static MattermostAttachment CloneAttachment(MattermostAttachment inAtt)
        {
            var outAtt = new MattermostAttachment
            {
                AuthorIcon = inAtt.AuthorIcon,
                AuthorLink = inAtt.AuthorLink,
                AuthorName = inAtt.AuthorName,
                Color = inAtt.Color,
                Fallback = inAtt.Fallback,
                Fields = inAtt.Fields,
                ImageUrl = inAtt.ImageUrl,
                Pretext = inAtt.Pretext,
                ThumbUrl = inAtt.ThumbUrl,
                Title = inAtt.Title,
                TitleLink = inAtt.TitleLink,
                Text = ""
            };
            return outAtt;
        }

        /// <summary>
        /// Post Message to Mattermost server. Messages will be automatically split. (Mattermost actually already auto splits long messages, but this will preserve whole words, rather than just splitting on message length alone.
        /// </summary>
        /// <param name="inMessage">The messsage you wish to send</param>
        /// <param name="maxMessageLength">(Optional) Defaulted to 4000, but can be set to any value (Check with your Mattermost server admin!)</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PostAsync(MattermostMessage inMessage, int maxMessageLength = 4000)
        {
            try
            {
                HttpResponseMessage response = null;

                maxMessageLength -= 10; //To allow for adding a message number indicator at the front end of the message.

                var outMessages = new List<MattermostMessage>
                {
                    CloneMessage(inMessage)
                };

                var msgIdx = 0;

                if (inMessage.Text != null)
                {
                    //Split messages text into chunks of maxMessageLength in size.
                    var textChunks = SplitTextIntoChunks(inMessage.Text, maxMessageLength);

                    //iterate through chunks and create a MattermostMessage object for each one and add it to outMessages list.
                    foreach (var chunk in textChunks)
                    {
                        outMessages[msgIdx].Text = chunk;
                        if (msgIdx < textChunks.Count() -1)
                        {
                            outMessages.Add(CloneMessage(inMessage));
                            msgIdx++;
                        }
                    }
                }

                //next check for attachments on the original message object
                if (inMessage.Attachments?.Any() ?? false)
                    {
                        outMessages[msgIdx].Attachments = new List<MattermostAttachment>();
                        var msgCnt = msgIdx;

                        foreach (var att in inMessage.Attachments)
                        {
                            outMessages[msgIdx].Attachments.Add(CloneAttachment(att));
                            var attIdx = outMessages[msgIdx].Attachments.Count - 1;

                            var attTextChunks = SplitTextIntoChunks(att.Text, 6600); //arbitrary limit. MM files suggest limit is 7600, but that still results in attachments being truncated...

                        foreach (var attChunk in attTextChunks)
                            {
                                outMessages[msgIdx].Attachments[attIdx].Text = attChunk;

                                if (msgIdx < msgCnt + attTextChunks.Count() - 1)
                                {
                                    outMessages.Add(CloneMessage(inMessage));
                                    msgIdx++;
                                    outMessages[msgIdx].Attachments = new List<MattermostAttachment> { CloneAttachment(att) };
                                }
                            }
                        }
                    }

                if (outMessages.Count > 1)
                {
                    var num = 1;
                    foreach (var msg in outMessages)
                    {
                        msg.Text = $"`({num}/{msgIdx + 1}): ` " + msg.Text;
                        num++;
                    }
                }

                foreach (var msg in outMessages)
                {
                     var serializedPayload = JsonConvert.SerializeObject(msg);
                    response = await _httpClient.PostAsync(_webhookUrl,
                        new StringContent(serializedPayload, Encoding.UTF8, "application/json"));
                }

                return response;
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        private static IEnumerable<string> SplitTextIntoChunks(string str, int maxChunkSize, bool preserveWords = true)
        {

            if (preserveWords)
            {
                //Less Simple
                var words = str.Split(' ');
                var tempString = new StringBuilder("");

                foreach (var word in words)
                {
                    if (word.Length + tempString.Length + 1 > maxChunkSize)
                    {
                        yield return tempString.ToString();
                        tempString.Clear();
                    }

                    tempString.Append(tempString.Length > 0 ? " " + word : word);
                }

                yield return tempString.ToString();
            }
            else
            {
                //Simple
                for (var i = 0; i < str.Length; i += maxChunkSize)
                {
                    yield return str.Substring(i, Math.Min(maxChunkSize, str.Length - i));
                }
            }

        }
    }
}