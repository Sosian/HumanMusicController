using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace VisualizationServer.Server.Hubs
{
    public class HeartbeatHub : Hub
    {
        public async Task SendHeartbeat(int heartbeat, double currentProgress)
        {
            await Clients.All.SendAsync("ReceiveHeartbeat", heartbeat, currentProgress);
        }

        public async Task LevelReached(int level)
        {
            await Clients.All.SendAsync("LevelReached", level);
        }
    }
}