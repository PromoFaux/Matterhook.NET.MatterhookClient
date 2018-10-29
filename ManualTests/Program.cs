using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Matterhook.NET.MatterhookClient;
using Newtonsoft.Json;

namespace ManualTests
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //Make sure the file `config.json` exists with a `webhookURL` and `testChannel` properties (copy always)
            var _config = LoadConfig();

            PostBasicMessage(_config);
            PostAdvancedMessage(_config);
            PostButtonsMessage(_config);
            PostMenuMessage(_config);
            PostChannelsMenuMessage(_config);
            PostUsersMenuMessage(_config);
        }

        public static void PostBasicMessage(Config config)
        {
            var client = new MatterhookClient(config.incomingWebHookUrl);
            var message = new MattermostMessage
            {
                Text = "Hello, I was posted using [Matterhook.NET](https://github.com/promofaux/Matterhook.NET)",
                Channel = config.testChannel,
                Username = "Awesome-O-Matic"
            };
            Task.WaitAll(client.PostAsync(message));
        }

        public static void PostAdvancedMessage(Config config)
        {
            var client = new MatterhookClient(config.incomingWebHookUrl);
            var message = new MattermostMessage
            {
                Text = "Hello, I was posted using [Matterhook.NET](https://github.com/promofaux/Matterhook.NET)",
                Channel = config.testChannel,
                Username = "Awesome-O-Matic",
                IconUrl =
                    "https://upload.wikimedia.org/wikipedia/commons/thumb/0/05/Robot_icon.svg/2000px-Robot_icon.svg.png",

                Attachments = new List<MattermostAttachment>
                {
                    new MattermostAttachment
                    {
                        Fallback = "test",
                        Color = "#FF8000",
                        Pretext = "This is optional pretext that shows above the attachment.",
                        Text =
                            "This is the text of the attachment. It should appear just above an image of the Mattermost logo. The left border of the attachment should be colored orange, and below the image it should include additional fields that are formatted in columns. At the top of the attachment, there should be an author name followed by a bolded title. Both the author name and the title should be hyperlinks.",
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

        public static void PostButtonsMessage(Config config)
        {
            var client = new MatterhookClient(config.incomingWebHookUrl);
            var message = new MattermostMessage
            {
                Text = "Message Text Example",
                Channel = config.testChannel,
                Username = "Awesome-O-Matic",
                IconUrl =
                    "https://upload.wikimedia.org/wikipedia/commons/thumb/0/05/Robot_icon.svg/2000px-Robot_icon.svg.png",
                Attachments = new List<MattermostAttachment>
                {
                    new MattermostAttachment
                    {
                        Text = "Attachment Text Example",
                        Actions = new List<IMattermostAction>
                        {
                            new MattermostAction
                            {
                                Name = "Merge",
                                Integration = new MattermostIntegration(config.outgoingWebHookUrl,
                                    new Dictionary<string, object>
                                    {
                                        {"pr", 1234},
                                        {"action", "merge"}
                                    })
                            },
                            new MattermostAction
                            {
                                Name = "Notify",
                                Integration = new MattermostIntegration(config.outgoingWebHookUrl,
                                    new Dictionary<string, object>
                                    {
                                        {"text", "code was pushed."}
                                    })
                            }
                        }
                    }
                }
            };
            Task.WaitAll(client.PostAsync(message));
        }

        public static void PostMenuMessage(Config config)
        {
            var client = new MatterhookClient(config.incomingWebHookUrl);
            var message = new MattermostMessage
            {
                Text =
                    "This is a menu message posted using [Matterhook.NET](https://github.com/promofaux/Matterhook.NET)",
                Channel = config.testChannel,
                Username = "Awesome-O-Matic",
                IconUrl =
                    "https://upload.wikimedia.org/wikipedia/commons/thumb/0/05/Robot_icon.svg/2000px-Robot_icon.svg.png",

                Attachments = new List<MattermostAttachment>
                {
                    new MattermostAttachment
                    {
                        Pretext = "This is optional pretext that shows above the attachment.",
                        Text = "This is the text of the attachment. ",
                        Actions = new List<IMattermostAction>
                        {
                            new MattermostMessageMenu
                            {
                                Integration = new MattermostIntegration(config.outgoingWebHookUrl,
                                    new Dictionary<string, object>
                                    {
                                        {"text", "Some data to send always."}
                                    }),
                                Name = "Test",
                                Options = new List<MessageMenuOption>
                                {
                                    new MessageMenuOption("Option1", "value1"),
                                    new MessageMenuOption("Option2", "value2"),
                                    new MessageMenuOption("Option3", "value3")
                                }
                            }
                        }
                    }
                }
            };
            Task.WaitAll(client.PostAsync(message));
        }

        public static void PostChannelsMenuMessage(Config config)
        {
            var client = new MatterhookClient(config.incomingWebHookUrl);
            var message = new MattermostMessage
            {
                Text =
                    "This is a message menu with channels source posted using [Matterhook.NET](https://github.com/promofaux/Matterhook.NET)",
                Channel = config.testChannel,
                Username = "Awesome-O-Matic",
                IconUrl =
                    "https://upload.wikimedia.org/wikipedia/commons/thumb/0/05/Robot_icon.svg/2000px-Robot_icon.svg.png",

                Attachments = new List<MattermostAttachment>
                {
                    new MattermostAttachment
                    {
                        Pretext = "This is optional pretext that shows above the attachment.",
                        Text = "This is the text of the attachment. ",
                        Actions = new List<IMattermostAction>
                        {
                            new MattermostMessageMenuChannels
                            {
                                Name = "channels",
                                Integration = new MattermostIntegration(config.outgoingWebHookUrl,
                                    new Dictionary<string, object>
                                    {
                                        {"active", "false"}
                                    })
                            }
                        }
                    }
                }
            };
            Task.WaitAll(client.PostAsync(message));
        }

        public static void PostUsersMenuMessage(Config config)
        {
            var client = new MatterhookClient(config.incomingWebHookUrl);
            var message = new MattermostMessage
            {
                Text =
                    "This is a message menu with users source posted using [Matterhook.NET](https://github.com/promofaux/Matterhook.NET)",
                Channel = config.testChannel,
                Username = "Awesome-O-Matic",
                IconUrl =
                    "https://upload.wikimedia.org/wikipedia/commons/thumb/0/05/Robot_icon.svg/2000px-Robot_icon.svg.png",

                Attachments = new List<MattermostAttachment>
                {
                    new MattermostAttachment
                    {
                        Pretext = "This is optional pretext that shows above the attachment.",
                        Text = "This is the text of the attachment. ",
                        Actions = new List<IMattermostAction>
                        {
                            new MattermostMessageMenuUsers
                            {
                                Name = "Users",
                                Integration = new MattermostIntegration(config.outgoingWebHookUrl,
                                    new Dictionary<string, object>
                                    {
                                        {"mood", "sad"}
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
            if (File.Exists(configFile))
            {
                Console.WriteLine("No Config file found, make sure it exists first");
                Environment.Exit(1);
                return null;
            }

            var jsonStr = File.ReadAllText(configFile);
            return JsonConvert.DeserializeObject<Config>(jsonStr);
        }
    }
}