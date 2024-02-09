using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using BluetoothReceiver.Bluetooth;
using Grpc.Net.Client;
using GrpcClient;


using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("System", LogLevel.Warning)
                .AddFilter("BluetoothReceiver.Program", LogLevel.Debug)
                .AddFilter("BluetoothReceiver.PolarH10Bluetooth", LogLevel.Debug)
                .AddConsole();
        });
ILogger logger = loggerFactory.CreateLogger<Program>();

//TODO: There is probably a better way to deal with this hardcoded path
var recordingsPath = @"C:\Users\flori\Documents\HumanMusicController\BluetoothReceiver\Recordings";
var recorder = new Recorder<HeartrateDataMessage>(recordingsPath);


var connectionFactory = new NamedPipesConnectionFactory("MyPipeName");
var socketsHttpHandler = new SocketsHttpHandler
{
    ConnectCallback = connectionFactory.ConnectAsync
};

var grpcChannel = GrpcChannel.ForAddress("http://localhost", new GrpcChannelOptions
{
    HttpHandler = socketsHttpHandler
});

if (args.Length > 0 && args[0].ToLower() == "replay")
{
    logger.LogInformation("Start Replay");

    if (args.Length <= 1)
        throw new ArgumentException("Please supply Recordings FileName, thanks");

    var recordingsFile = recordingsPath + @"\" + args[1];
    var replayer = new Replayer<HeartrateDataMessage>(new SensorIPCService.SensorIPCServiceClient(grpcChannel));

    while (true)
    {
        replayer.Play(recordingsFile);
    }
}
else
{
    logger.LogInformation("Start Live");

    var bluetoothDeviceFinder = new BluetoothDeviceFinder(loggerFactory.CreateLogger<BluetoothDeviceFinder>());
    var bluetoothDevice = bluetoothDeviceFinder.GetDevice();
    var bluetoothDeviceSession = new BleDeviceSession(loggerFactory.CreateLogger<BleDeviceSession>(), bluetoothDevice);
    var polarH10Bluetooth = new PolarH10Bluetooth(loggerFactory.CreateLogger<PolarH10Bluetooth>(), bluetoothDeviceSession, recorder, grpcChannel);

    await polarH10Bluetooth.Start();
}

Console.ReadLine();