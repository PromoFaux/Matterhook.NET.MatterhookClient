[![NuGet](https://img.shields.io/nuget/v/Matterhook.NET.MatterhookClient.svg)](https://www.nuget.org/packages/Matterhook.NET/)
[![NuGet](https://img.shields.io/nuget/dt/Matterhook.NET.MatterhookClient.svg)](https://www.nuget.org/packages/Matterhook.NET/)

# Matterhook.NET

Matterhook.NET is a simple webhook client to post messages to your Mattermost server using Webhooks.

## Usage

You can install the package from nuget using `Install-Package Matterhook.NET.MatterhookClient`,

Alternatively, clone/fork this repo and compile the source yourself.

#### Simple Message:

```C#

var client = new MatterhookClient("https://your.webhook.url/0892340923432");

var message = new MattermostMessage
{
    Text = "Hello, I was posted using [Matterhook.NET](https://github.com/promofaux/Matterhook.NET)",
    Channel = "general",
    Username = "Awesome-O-Matic"
};

Task.WaitAll(client.PostAsync(message));

```

![](http://i.imgur.com/jLZsP4E.png)

#### Advanced Message with attachment:

> Using example template from [Mattermost docs](https://docs.mattermost.com/developer/message-attachments.html#example-message-attachment)

```C#
 var client = new MatterhookClient("https://your.webhook.url/0892340923432");

var message = new MattermostMessage
{
    Text = "Hello, I was posted using [Matterhook.NET](https://github.com/promofaux/Matterhook.NET)",
    Channel = "offtopic",
    Username = "Awesome-O-Matic",
    IconUrl = new Uri(
        "https://upload.wikimedia.org/wikipedia/commons/thumb/0/05/Robot_icon.svg/2000px-Robot_icon.svg.png"),

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

```

![](https://i.imgur.com/n5ecwYb.png)
