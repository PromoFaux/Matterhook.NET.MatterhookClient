using Matterhook.NET.MatterhookClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ManualTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            //Make sure the file `config.json` exists with a `webhookURL` and `testChannel` properties (copy always)
            var _config = LoadConfig();
            var client = new MatterhookClient(_config.incomingWebHookUrl);

            PostBasicMessage(_config);
            PostAdvancedMessage(_config);
            PostButtonsMessage(_config);
        }

        public static void PostBasicMessage(Config _config)
        {
            var client = new MatterhookClient(_config.incomingWebHookUrl);
            var message = new MattermostMessage
            {
                Text = "Hello, I was posted using [Matterhook.NET](https://github.com/promofaux/Matterhook.NET)",
                Channel = _config.testChannel,
                Username = "Awesome-O-Matic"
            };

            Task.WaitAll(client.PostAsync(message));
        }

        public static void PostAdvancedMessage(Config _config)
        {
            var client = new MatterhookClient(_config.incomingWebHookUrl);
            var message = new MattermostMessage
            {
                Text = "Hello, I was posted using [Matterhook.NET](https://github.com/promofaux/Matterhook.NET)",
                Channel = _config.testChannel,
                Username = "Awesome-O-Matic",
                IconUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/0/05/Robot_icon.svg/2000px-Robot_icon.svg.png",

                Attachments = new List<MattermostAttachment>
                {
                    new MattermostAttachment
                    {
                        Fallback = "test",
                        Color = "#FF8000",
                        Pretext = "This is optional pretext that shows above the attachment.",
                        Text = "This is the text of the attachment. It should appear just above an image of the Mattermost logo. The left border of the attachment should be colored orange, and below the image it should include additional fields that are formatted in columns. At the top of the attachment, there should be an author name followed by a bolded title. Both the author name and the title should be hyperlinks.",
                        AuthorName = "Mattermost",
                        AuthorIcon = "http://www.mattermost.org/wp-content/uploads/2016/04/icon_WS.png",
                        AuthorLink = "http://www.mattermost.org/",
                        Title = "Example Attachment",
                        TitleLink = "http://docs.mattermost.com/developer/message-attachments.html",

                        Fields = new List<MattermostField>
                        {
                            new MattermostField
                            {
                                Short = false,
                                Title = "Long Field.",
                                Value =
                                    "Testing with a very long piece of text that will take up the whole width of the table. And then some more text to make it extra long."
                            },
                            new MattermostField
                            {
                                Short = true,
                                Title = "Column One",
                                Value = "Testing"
                            },
                            new MattermostField
                            {
                                Short = true,
                                Title = "Column Two",
                                Value = "Testing"
                            },
                            new MattermostField
                            {
                                Short = false,
                                Title = "Another Field",
                                Value = "Testing"
                            }
                        },
                        ImageUrl = "http://www.mattermost.org/wp-content/uploads/2016/03/logoHorizontal_WS.png"
                    }
                }
            };
            Task.WaitAll(client.PostAsync(message));
        }

        public static void PostButtonsMessage(Config _config)
        {
            var client = new MatterhookClient(_config.incomingWebHookUrl);
            var message = new MattermostMessage()
            {
                Text = "Message Text Example",
                Channel = _config.testChannel,
                Username = "Awesome-O-Matic",
                IconUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/0/05/Robot_icon.svg/2000px-Robot_icon.svg.png",
                Attachments = new List<MattermostAttachment>()
                {
                    new MattermostAttachment()
                    {
                        Text = "Attachment Text Example",
                        Actions = new List<IMattermostAction>()
                        {
                           new MattermostAction()
                            {
                                Name = "Merge",
                                Integration = new MattermostIntegration(_config.outgoingWebHookUrl,new Dictionary<string, object>()
                                {
                                    {"pr",1234 },
                                    {"action","merge"}
                                })
                            },
                            new MattermostAction()
                            {
                                Name = "Notify",
                                Integration = new MattermostIntegration(_config.outgoingWebHookUrl, new Dictionary<string, object>()
                                {
                                    {"text","code was pushed." }
                                })
                            }
                        }
                    }
                }
            };
            Task.WaitAll(client.PostAsync(message));
        }

        public static Config LoadConfig()
        {
            var configFile = "config.json";
            var _config = new Config();
            if (File.Exists(configFile))
            {
                using (var file = File.OpenText(configFile))
                {
                    var serializer = new JsonSerializer();
                    _config = (Config)serializer.Deserialize(file, typeof(Config));

                    return _config;
                }
            }
            else
            {
                Console.WriteLine("No Config file found, make sure it exists first");
                Environment.Exit(1);
            }
            return null;
        }
    }

    public class Config
    {
        /// <summary>
        /// The incoming webhook URL on the mattermost server
        /// </summary>
        public string incomingWebHookUrl { get; set; }
        /// <summary>
        /// For interactive buttons
        /// </summary>
        public string outgoingWebHookUrl { get; set; }
        /// <summary>
        /// Channel to post your test messages to
        /// </summary>
        public string testChannel { get; set; }
    }
}
