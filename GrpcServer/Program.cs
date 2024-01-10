using GrpcServer.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenNamedPipe("MyPipeName", listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<SensorIPCService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
