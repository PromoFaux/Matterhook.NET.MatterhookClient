namespace Matterhook.NET.MatterhookClient
{
    public interface IMattermostAction
    {
        MattermostIntegration Integration { get; set; }

        string Name { get; set; }
    }
}