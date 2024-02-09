using Grpc.Net.Client;
using GrpcClient;
using SensorReceiverServer.Controllers;

var builder = WebApplication.CreateBuilder(args);


builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddControllers();

//TODO: There is probably a better way to deal with this hardcoded path
var recordingsPath = @"C:\Users\flori\Documents\HumanMusicController\SensorReceiverServer\Recordings";
builder.Services.AddSingleton<IRecorder<IMUDataMessage>>(provider => new Recorder<IMUDataMessage>(recordingsPath));


var connectionFactory = new NamedPipesConnectionFactory("MyPipeName");
var socketsHttpHandler = new SocketsHttpHandler
{
    ConnectCallback = connectionFactory.ConnectAsync
};

var grpcChannel = GrpcChannel.ForAddress("http://localhost", new GrpcChannelOptions
{
    HttpHandler = socketsHttpHandler
});

builder.Services.AddSingleton<GrpcChannel>(grpcChannel);

var app = builder.Build();

app.MapControllers();

if (args.Length > 0 && args[0].ToLower() == "replay")
{
    app.Logger.LogInformation("Replaying with " + args[1]);
    if (args.Length <= 1)
        throw new ArgumentException("Please supply Recordings FileName, thanks");

    var recordingsFile = recordingsPath + @"\" + args[1];
    var replayer = new Replayer<IMUDataMessage>(new SensorIPCService.SensorIPCServiceClient(grpcChannel));

    while (true)
    {
        replayer.Play(recordingsFile);
    }
}

app.Logger.LogInformation("Running Server");


app.Run();