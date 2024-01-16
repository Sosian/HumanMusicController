using System.Net.Sockets;
using CoreOSC;
using CoreOSC.IO;
using Grpc.Core;
using GrpcClient;
using GrpcServer;

namespace GrpcServer.Services;

public class SensorIPCService : GrpcClient.SensorIPCService.SensorIPCServiceBase
{
    private readonly ILogger<SensorIPCService> _logger;
    public SensorIPCService(ILogger<SensorIPCService> logger)
    {
        _logger = logger;
    }

    public override Task<Empty> SendIMUData(IMUDataMessage request, ServerCallContext context)
    {
        using (var udpClient = new UdpClient("127.0.0.1", 4560))
        {
            var message = new OscMessage(new Address("/trigger/prophet"), [request.X, request.Y]);
            udpClient.SendMessageAsync(message);
        }

        return Task.FromResult(new Empty());
    }
}
