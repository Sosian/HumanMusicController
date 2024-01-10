var builder = WebApplication.CreateBuilder(args);

//TODO: There is probably a better way to handle the path here
var recordingsPath = @"C:\Users\flori\Documents\HumanMusicController\SensorReceiverServer\Recordings";
if (args[0].ToLower() == "Replay")
{
    if (args.Length <= 1)
        throw new ArgumentException("Please supply Recordings FileName, thanks");

    var recordingsFile = recordingsPath + @"\" + args[1];


}

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
