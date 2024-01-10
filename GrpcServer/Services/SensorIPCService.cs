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
        _logger.LogWarning("DoesthisWork?");
        return Task.FromResult(new Empty());
    }
}
