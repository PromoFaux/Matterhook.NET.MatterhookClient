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
            return new MattermostMessage
            {
                Text = "",
                Attachments = null,
                Channel = message.Channel,
                Username = message.Username,
                IconUrl = message.IconUrl
            };
        }

        public async Task<HttpResponseMessage> PostAsync(MattermostMessage message)
        {
            try
            {
                //string[] lines = theText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                HttpResponseMessage response = null;
                var messages = new List<MattermostMessage>();


                var cnt = 0;

                var lines = new string[] { };
                if (message.Text != null)
                {
                    lines = message.Text.Split(new[] {"\r\n", "\r", "\n"}, StringSplitOptions.None);
                }

                messages.Add(CloneMessage(message));

                foreach (var line in lines)
                {
                    if (line.Length + messages[cnt].Text.Length > 3800)
                    {
                        cnt += 1;
                        messages.Add(CloneMessage(message));
                    }

                    messages[cnt].Text += line;
                }

                var len = messages[cnt].Text.Length;

                if (message.Attachments.Any())
                {
                    messages[cnt].Attachments = new List<MattermostAttachment> {new MattermostAttachment()};
                    messages[cnt].Attachments[0].Text = "";

                    lines = message.Attachments[0].Text.Split(new[] {"\r\n", "\r", "\n"}, StringSplitOptions.None);

                    foreach (var line in lines)
                    {
                        if (len + messages[cnt].Attachments[0].Text.Length + line.Length > 3800)
                        {
                            cnt += 1;
                            messages.Add(CloneMessage(message));
                            messages[cnt].Attachments = new List<MattermostAttachment> {new MattermostAttachment()};
                            messages[cnt].Attachments[0].Text = "";
                        }

                        messages[cnt].Attachments[0].Text += $"{line}\r\n";
                    }
                }


                if (messages.Count > 1)
                {
                    var num = 1;
                    foreach (var msg in messages)
                    {
                        msg.Text += $" ({num}/{cnt + 1})";
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

