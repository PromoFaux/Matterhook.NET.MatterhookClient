using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Matterhook.NET.MatterhookClient.Tests
{
    public class PayloadTests
    {
        private string GetExpectedJson(string fileName)
        {
            return File.ReadAllText($"./ExpectedJson/{fileName}.json");            
        }


        [Fact]
        public void BasicMessage()
        {
            var message = new MattermostMessage
            {
                Text = "Hello, I was posted using [Matterhook.NET.MatterhookClient](https://github.com/promofaux/Matterhook.NET.MatterhookClient)",
                Channel = "testChannel",
                Username = "Awesome-O-Matic"
            };
            var payload = message.SerializeToJson();
            var expected = GetExpectedJson(nameof(BasicMessage));

            Assert.Equal(payload, expected);
            
        }

        [Fact]
        public void BasicMessageWithCard()
        {
            var message = new MattermostMessage
            {
                Text = "Hello, I was posted using [Matterhook.NET.MatterhookClient](https://github.com/promofaux/Matterhook.NET.MatterhookClient)",
                Channel = "testChannel",
                Username = "Awesome-O-Matic",
                Props = new MattermostProps() { Card = "**THIS IS A CARD**\n\nIt came from [Matterhook.NET.MatterhookClient](https://github.com/promofaux/Matterhook.NET.MatterhookClient)" }
            };
            var payload = message.SerializeToJson();
            var expected = GetExpectedJson(nameof(BasicMessageWithCard));

            Assert.Equal(payload, expected);

        }

        [Fact]
        public void AdvancedMessage()
        {
            var message = new MattermostMessage
            {
                Text = "Hello, I was posted using [Matterhook.NET.MatterhookClient](https://github.com/promofaux/Matterhook.NET.MatterhookClient)",
                Channel = "testChannel",
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
            var payload = message.SerializeToJson();
            var expected = GetExpectedJson(nameof(AdvancedMessage));

            Assert.Equal(payload, expected);
        }

        [Fact]
        public void ButtonsMessage()
        {
            var message = new MattermostMessage
            {
                Text = "Message Text Example",
                Channel = "testChannel",
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
                                Integration = new MattermostIntegration("https://notarealoutgoingwebhook.justfortests",
                                    new Dictionary<string, object>
                                    {
                                        {"pr", 1234},
                                        {"action", "merge"}
                                    })
                            },
                            new MattermostAction
                            {
                                Name = "Notify",
                                Integration = new MattermostIntegration("https://notarealoutgoingwebhook.justfortests",
                                    new Dictionary<string, object>
                                    {
                                        {"text", "code was pushed."}
                                    })
                            }
                        }
                    }
                }
            };

            var payload = message.SerializeToJson();
            var expected = GetExpectedJson(nameof(ButtonsMessage));

            Assert.Equal(payload, expected);
        }

        [Fact]
        public void MenuMessage()
        {
            var message = new MattermostMessage
            {
                Text =
                    "This is a menu message posted using [Matterhook.NET.MatterhookClient](https://github.com/promofaux/Matterhook.NET.MatterhookClient)",
                Channel = "testChannel",
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
                                Integration = new MattermostIntegration("https://notarealoutgoingwebhook.justfortests",
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

            var payload = message.SerializeToJson();
            var expected = GetExpectedJson(nameof(MenuMessage));

            Assert.Equal(payload, expected);
        }

        [Fact]
        public void ChannelsMenuMessage()
        {
            var message = new MattermostMessage
            {
                Text =
                    "This is a message menu with channels source posted using [Matterhook.NET.MatterhookClient](https://github.com/promofaux/Matterhook.NET.MatterhookClient)",
                Channel = "testChannel",
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
                                Integration = new MattermostIntegration("https://notarealoutgoingwebhook.justfortests",
                                    new Dictionary<string, object>
                                    {
                                        {"active", "false"}
                                    })
                            }
                        }
                    }
                }
            };

            var payload = message.SerializeToJson();
            var expected = GetExpectedJson(nameof(ChannelsMenuMessage));

            Assert.Equal(payload, expected);
        }

        [Fact]
        public void UsersMenuMessage()
        {
            var message = new MattermostMessage
            {
                Text =
                    "This is a message menu with users source posted using [Matterhook.NET.MatterhookClient](https://github.com/promofaux/Matterhook.NET.MatterhookClient)",
                Channel = "testChannel",
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
                                Integration = new MattermostIntegration("https://notarealoutgoingwebhook.justfortests",
                                    new Dictionary<string, object>
                                    {
                                        {"mood", "sad"}
                                    })
                            }
                        }
                    }
                }
            };

            var payload = message.SerializeToJson();
            var expected = GetExpectedJson(nameof(UsersMenuMessage));

            Assert.Equal(payload, expected);
        }
    }
}
