using Grpc.Net.Client;
using GrpcClient;
using GrpcClient.Recorder;
using Microsoft.AspNetCore.Mvc;

namespace SensorReceiverServer.Controllers;

[ApiController]
public class IMUController : ControllerBase
{
    private readonly ILogger<IMUController> _logger;
    private GrpcChannel grpcChannel;

    private Recorder<IMUDataMessage> _recorder;

    public IMUController(ILogger<IMUController> logger)
    {
        _logger = logger;

        //TODO: This should probably be a singleton supplied by DependencyInjection
        var connectionFactory = new NamedPipesConnectionFactory("MyPipeName");
        var socketsHttpHandler = new SocketsHttpHandler
        {
            ConnectCallback = connectionFactory.ConnectAsync
        };

        grpcChannel = GrpcChannel.ForAddress("http://localhost", new GrpcChannelOptions
        {
            HttpHandler = socketsHttpHandler
        });

        //TODO: There is probably a better way to handle this hardcoded Path here
        _recorder = new Recorder<IMUDataMessage>(@"C:\Users\flori\Documents\HumanMusicController\SensorReceiverServer\Recordings");
    }


    [HttpPost]
    [Route("imu/receiveData")]
    public async Task<ActionResult> ReceiveData([FromForm] string justSomething)
    {
        var grpcService = new SensorIPCService.SensorIPCServiceClient(grpcChannel);
        var imuDataMessage = new IMUDataMessage() { Name = "test", X = 1, Y = 1, Z = 1, Gx = 1, Gy = 1, Gz = 1 };
        await grpcService.SendIMUDataAsync(imuDataMessage);
        _recorder.ReceiveData(imuDataMessage);
        return Ok();
    }
}
