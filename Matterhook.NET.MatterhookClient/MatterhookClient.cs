using System;
using System.Collections.Generic;
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

        public MattermostMessage CloneMessage(MattermostMessage message)
        {
            var retval = new MattermostMessage
            {
                Text = "",
                Channel = message.Channel,
                Username = message.Username,
                IconUrl = message.IconUrl
            };

            //if no attachments on the original, return clone without attachments.
            if (retval.Attachments == null) return retval;

            //we have attachment(s) on the original, we need at least one attachment on the clone
            retval.Attachments[0] = message.Attachments[0];
            retval.Attachments[0].Text = "";

            return retval;

        }

        /// <summary>
        /// Post Message to Mattermost server. Messages will be automatically split if total text length > 4000
        /// </summary>
        /// <param name="message">The messsage you wish to send</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PostAsync(MattermostMessage message)
        {
            try
            {
                HttpResponseMessage response = null;
                var messages = new List<MattermostMessage>();

                var cnt = 0;

                var lines = new string[] { };
                if (message.Text != null)
                {
                    lines = message.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                }

                //start with one cloned message in the list
                messages.Add(CloneMessage(message));

                //add text from original. If we go over 3800, we'll split it to a new message.
                foreach (var line in lines)
                {
                    if (line.Length + messages[cnt].Text.Length > 3800)
                    {
                        cnt += 1;
                        messages.Add(CloneMessage(message));
                    }

                    messages[cnt].Text += $"{line}\r\n";
                }

                //Length of text on the last (or first if only one) message.
                var len = messages[cnt].Text.Length;

                //does our original have attachments?
                if (message.Attachments?.Any() ?? false)
                {

                    //loop through them in a similar fashion to the message text above.
                    foreach (var att in message.Attachments)
                    {
                        messages[cnt].Attachments = new List<MattermostAttachment> { new MattermostAttachment() };
                        messages[cnt].Attachments[0].Text = "";

                        lines = message.Attachments[0].Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                        foreach (var line in lines)
                        {
                            if (len + messages[cnt].Attachments[0].Text.Length + line.Length > 3800)
                            {
                                cnt += 1;
                                messages.Add(CloneMessage(message));
                                messages[cnt].Attachments[0].Text = "";
                            }

                            messages[cnt].Attachments[0].Text += $"{line}\r\n";
                        }
                    }
                }


                if (messages.Count > 1)
                {
                    var num = 1;
                    foreach (var msg in messages)
                    {
                        msg.Text = $"`({num}/{cnt + 1}): ` " + msg.Text;
                        num++;
                    }
                }

                foreach (var msg in messages)
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
    }
}

