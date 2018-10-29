namespace ManualTests
{
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
