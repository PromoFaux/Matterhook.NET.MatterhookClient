﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace Matterhook.NET.MatterhookClient
{
    /// <summary>
    /// https://docs.mattermost.com/developer/message-attachments.html#message-attachments
    /// </summary>
    public class MattermostAttachment
    {
        #region Fields

        /// <summary>
        ///     Fields can be included as an optional array within attachments, and are used to display information in a table
        ///     format inside the attachment.
        /// </summary>
        [JsonProperty(PropertyName = "fields")]
        public List<MattermostField> Fields { get; set; }

        #endregion

        public MattermostAttachment Clone()
        {
            var outAtt = new MattermostAttachment
            {
                AuthorIcon = AuthorIcon,
                AuthorLink = AuthorLink,
                AuthorName = AuthorName,
                Color = Color,
                Fallback = Fallback,
                Fields = Fields,
                ImageUrl = ImageUrl,
                Pretext = Pretext,
                ThumbUrl = ThumbUrl,
                Title = Title,
                TitleLink = TitleLink,
                Text = "",
                Actions = Actions
            };
            return outAtt;
        }

        //https://docs.mattermost.com/developer/message-attachments.html#attachment-options

        #region AttachmentOptions

        /// <summary>
        ///     A required plain-text summary of the post. This is used in notifications, and in clients that don’t support
        ///     formatted text (eg IRC).
        /// </summary>
        [JsonProperty(PropertyName = "fallback")]
        public string Fallback { get; set; }

        /// <summary>
        ///     A hex color code that will be used as the left border color for the attachment. If not specified, it will default
        ///     to match the left hand sidebar header background color.
        /// </summary>
        [JsonProperty(PropertyName = "color")]
        public string Color { get; set; }

        /// <summary>
        ///     Optional text that should appear above the formatted data
        /// </summary>
        [JsonProperty(PropertyName = "pretext")]
        public string Pretext { get; set; }

        /// <summary>
        ///     The text to be included in the attachment. It can be formatted using Markdown. If it includes more than 300
        ///     characters or more than 5 line breaks, the message will be collapsed and a “Show More” link will be added to expand
        ///     the message.
        /// </summary>
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        #endregion

        //https://docs.mattermost.com/developer/message-attachments.html#author-details

        #region AuthorDetails

        /// <summary>
        ///     An optional name used to identify the author. It will be included in a small section at the top of the attachment.
        /// </summary>
        [JsonProperty(PropertyName = "author_name")]
        public string AuthorName { get; set; }

        /// <summary>
        ///     An optional URL used to hyperlink the author_name. If no author_name is specified, this field does nothing.
        /// </summary>
        [JsonProperty(PropertyName = "author_link")]
        public string AuthorLink { get; set; }

        /// <summary>
        ///     An optional URL used to display a 16x16 pixel icon beside the author_name.
        /// </summary>
        [JsonProperty(PropertyName = "author_icon")]
        public string AuthorIcon { get; set; }

        #endregion

        //https://docs.mattermost.com/developer/message-attachments.html#titles

        #region Titles

        /// <summary>
        ///     An optional title displayed below the author information in the attachment.
        /// </summary>
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        /// <summary>
        ///     An optional URL used to hyperlink the title. If no title is specified, this field does nothing.
        /// </summary>
        [JsonProperty(PropertyName = "title_link")]
        public string TitleLink { get; set; }

        #endregion

        //https://docs.mattermost.com/developer/message-attachments.html#images

        #region Images

        /// <summary>
        ///     An optional URL to an image file (GIF, JPEG, PNG, or BMP) that is displayed inside a message attachment.
        ///     Large images are resized to a maximum width of 400px or a maximum height of 300px, while still maintaining the
        ///     original aspect ratio.
        /// </summary>
        [JsonProperty(PropertyName = "image_url")]
        public string ImageUrl { get; set; }

        /// <summary>
        ///     An optional URL to an image file(GIF, JPEG, PNG, or BMP) that is displayed as a 75x75 pixel thumbnail on the right
        ///     side of an attachment.
        ///     We recommend using an image that is already 75x75 pixels, but larger images will be scaled down with the aspect
        ///     ratio maintained.
        /// </summary>
        [JsonProperty(PropertyName = "thumb_url")]
        public string ThumbUrl { get; set; }

        #endregion

        /// <summary>
        ///     Mattermost supports interactive buttons as actions in your integration message attachments.
        /// </summary>
        [JsonProperty(PropertyName = "actions")]
        public List<IMattermostAction> Actions { get; set; }
    }
}
