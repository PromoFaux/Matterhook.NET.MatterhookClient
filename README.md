[![Build](https://github.com/PromoFaux/Matterhook.NET.MatterhookClient/actions/workflows/build.yml/badge.svg)](https://github.com/PromoFaux/Matterhook.NET.MatterhookClient/actions/workflows/build.yml) [![NuGet](https://img.shields.io/nuget/v/Matterhook.NET.MatterhookClient.svg)](https://www.nuget.org/packages/Matterhook.NET.MatterhookClient/)
[![NuGet](https://img.shields.io/nuget/dt/Matterhook.NET.MatterhookClient.svg)](https://www.nuget.org/packages/Matterhook.NET.MatterhookClient/)

# Matterhook.NET.MatterhookClient

Matterhook.NET.MatterhookClient is a simple webhook client to post messages to your Mattermost server using Webhooks. It supports message buttons and menus (menus are only supported in Mattermost v5.4+).

## Installation

You can install the latest release by installing the package from NuGet using `Install-Package Matterhook.NET.MatterhookClient`.

Alternatively, clone/fork this repo and compile the source yourself.

## Basic usage

Using this library is really easy. Just create a new MatterhookClient
```csharp
var client = new MatterhookClient("https://your.webhook.url/0892340923432");
```

Create your message

```csharp
var message = new MattermostMessage()
{
    //MessageOptions
};
```

Post your message

```csharp
Task.WaitAll(client.PostAsync(message));
```

## Message Types

### Simple Message

```C#
var message = new MattermostMessage
{
    Text = "Hello, I was posted using [Matterhook.NET](https://github.com/promofaux/Matterhook.NET)",
    Channel = "general",
    Username = "Awesome-O-Matic"
};
```

![](http://i.imgur.com/jLZsP4E.png)

### Advanced Message with attachment:

> Using example template from [Mattermost docs](https://docs.mattermost.com/developer/message-attachments.html#example-message-attachment)

```C#
var message = new MattermostMessage
{
    Text = "Hello, I was posted using [Matterhook.NET](https://github.com/promofaux/Matterhook.NET)",
    Channel = "offtopic",
    Username = "Awesome-O-Matic",
    IconUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/0/05/Robot_icon.svg/2000px-Robot_icon.svg.png",

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
```

![](https://i.imgur.com/n5ecwYb.png)

### Message with interactive buttons

```C#
var message = new MattermostMessage()
{
    Text = "Message Text Example",
    Username = "Awesome-O-Matic",
    IconUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/0/05/Robot_icon.svg/2000px-Robot_icon.svg.png",
    Attachments = new List<MattermostAttachment>()
    {
        new MattermostAttachment()
        {
            Text = "Attachment Text Example",
            Actions = new List<MattermostAction>()
            {
                new MattermostAction()
                {
                    Name = "Merge",
                    Integration = new MattermostIntegration("https://matterhook.example.com/merge",new Dictionary<string, object>()
                    {
                        {"pr",1234 },
                        {"action","merge"}
                    })
                },
                new MattermostAction()
                {
                    Name = "Notify",
                    Integration = new MattermostIntegration("https://matterhook.example.com/notify", new Dictionary<string, object>()
                    {
                        {"text","code was pushed." }
                    })
                }
            }
        }
    }
};
```

![](https://i.imgur.com/Eb8Ne2g.png)

Clicking `Merge` will trigger a POST request to `https://matterhook.example.com/merge` with following body

```json
{
  "user_id": "{userid}",
  "context": {
    "action": "merge",
    "pr": 1234
  }
}
```

and clicking `Notify` will trigger a POST request to `https://matterhook.example.com/notify` with body

```json
{
  "user_id": "{userid}",
  "context": {
    "text": "New code was pushed."
  }
}
```

### Message with menu buttons (supported in Mattermost 5.4+)

You also can post messages with menu buttons. It will post a message with a dropdown button where the users can select a value, which will be posted to the target integration.

Just add an attachment to your message as follows:

```csharp
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
```

You can add as many attachments of any type to a message as you want. Message menus and buttons can be used to for ex.:

* Mark a task complete in your project management tracker
* Conduct a customer survey or a poll
* Initiate a command to merge a branch into a release

## Contributing

We welcome everyone who wants to contribute to this repo!
Just open an issue with your intention or make a comment in some open issue you would like to work on.
