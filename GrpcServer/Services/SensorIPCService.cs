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
    private readonly IConductor conductor;

    public SensorIPCService(ILogger<SensorIPCService> logger, IConductor conductor)
    {
        _logger = logger;
        this.conductor = conductor;
    }

    public override Task<Empty> SendIMUData(IMUDataMessage request, ServerCallContext context)
    {
        conductor.ReceiveIMUDataMessage(request);

        return Task.FromResult(new Empty());
    }
}
