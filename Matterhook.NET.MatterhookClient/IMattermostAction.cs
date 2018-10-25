namespace Matterhook.NET.MatterhookClient
{
    /// <summary>
    /// Interface for a mattermost message action
    /// </summary>
    public interface IMattermostAction
    {
        /// <summary>
        /// Integration used by this action.
        /// </summary>
        MattermostIntegration Integration { get; set; }

        /// <summary>
        /// Action's name.
        /// </summary>
        string Name { get; set; }
    }
}