using System.Globalization;
using Grpc.Net.Client;
using GrpcClient;
using Microsoft.AspNetCore.Mvc;

namespace SensorReceiverServer.Controllers;

[ApiController]
public class IMUController : ControllerBase
{
    private readonly ILogger<IMUController> _logger;
    private GrpcChannel _grpcChannel;

    private IRecorder<IMUDataMessage> _recorder;

    public IMUController(ILogger<IMUController> logger, IRecorder<IMUDataMessage> recorder, GrpcChannel grpcChannel)
    {
        _logger = logger;
        _recorder = recorder;
        _grpcChannel = grpcChannel;
    }


    [HttpPost]
    [Route("imu/receiveData")]
    public async Task<ActionResult> ReceiveData([FromForm] string justSomething)
    {
        _logger.LogWarning(justSomething);
        var data = justSomething.Split(',');
        var grpcService = new SensorIPCService.SensorIPCServiceClient(_grpcChannel);
        var imuDataMessage = new IMUDataMessage() { Name = "test", X = float.Parse(data[0], CultureInfo.InvariantCulture.NumberFormat), Y = float.Parse(data[1], CultureInfo.InvariantCulture.NumberFormat), Z = float.Parse(data[2], CultureInfo.InvariantCulture.NumberFormat), Gx = float.Parse(data[3], CultureInfo.InvariantCulture.NumberFormat), Gy = float.Parse(data[4], CultureInfo.InvariantCulture.NumberFormat), Gz = float.Parse(data[5], CultureInfo.InvariantCulture.NumberFormat) };
        await grpcService.SendIMUDataAsync(imuDataMessage);
        _recorder.ReceiveData(imuDataMessage);
        return Ok();
    }
}
