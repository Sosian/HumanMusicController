using System.Diagnostics;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace GrpcClient
{
    public class Replayer<T>
    {
        private readonly SensorIPCService.SensorIPCServiceClient grpcClient;

        public Replayer(SensorIPCService.SensorIPCServiceClient grpcClient)
        {
            this.grpcClient = grpcClient;
        }

        public void Play(string recordFileFullPath)
        {
            var jsonDocument = JsonDocument.Parse(File.ReadAllText(recordFileFullPath));
            var listOfParsedReceivedPackages = new List<(long elapsedMilliseconds, T dataMessage)>();

            foreach (var package in jsonDocument.RootElement.EnumerateArray())
            {
                listOfParsedReceivedPackages.Add((package.GetProperty("time").GetInt64(), MessageResolver.FromJsonToObject<T>(package.GetProperty("data"))));
            }

            var currentPackage = listOfParsedReceivedPackages.First();
            var stopwatch = Stopwatch.StartNew();
            var waitIntervalMiliseconds = 50;
            var index = 0;

            while (index < listOfParsedReceivedPackages.Count)
            {
                Thread.Sleep(waitIntervalMiliseconds);

                if (index >= listOfParsedReceivedPackages.Count - 1)
                {
                    index = 0;
                    stopwatch = Stopwatch.StartNew();
                }

                if (currentPackage.elapsedMilliseconds <= stopwatch.ElapsedMilliseconds)
                {
                    MessageResolver.SendGrpcMessage(grpcClient, currentPackage.dataMessage);
                    index++;
                    currentPackage = listOfParsedReceivedPackages[index];
                }
            }
        }
    }
}