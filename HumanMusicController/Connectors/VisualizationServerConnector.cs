using Microsoft.AspNetCore.SignalR.Client;

namespace HumanMusicController.Connectors
{
    public class VisualizationServerConnection
    {
        private readonly HubConnection connection;

        public VisualizationServerConnection(string url)
        {
            connection = new HubConnectionBuilder()
                .WithUrl(new Uri(url))
                .WithAutomaticReconnect()
                .Build();

            connection.StartAsync().GetAwaiter().GetResult();
        }

        public async void SendHeartrateToVisualizationServer(int heartrate, double currentProgress)
        {
            await connection.SendAsync("SendHeartbeat", heartrate, currentProgress);
        }

        public async void SendLevelToVisualizationServer(int level)
        {
            await connection.SendAsync("LevelReached", level);
        }
    }
}