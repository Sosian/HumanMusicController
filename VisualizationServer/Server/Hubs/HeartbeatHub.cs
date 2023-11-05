using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace VisualizationServer.Server.Hubs
{
    public class HeartbeatHub : Hub
    {
        public async Task SendHeartbeat(int heartbeat, double currentProgress, int currentLevel)
        {
            await Clients.All.SendAsync("ReceiveHeartbeat", heartbeat, currentProgress, currentLevel);
        }
    }
}