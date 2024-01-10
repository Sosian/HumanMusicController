using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace GrpcClient
{
    public class Replayer
    {
        private readonly SensorIPCService.SensorIPCServiceClient grpcClient;

        public Replayer(GrpcClient.SensorIPCService.SensorIPCServiceClient grpcClient)
        {
            this.grpcClient = grpcClient;
        }

        public void Play(string recordFileFullPath)
        {
            var listOfReceivedPackages = File.ReadAllLines(recordFileFullPath).ToList<string>();
            var listOfParsedReceivedPackages = new List<(long elapsedMilliseconds, IMUDataMessage imuDataMessage)>();

            foreach (var package in listOfReceivedPackages)
            {
                var split = package.Split(";");
                listOfParsedReceivedPackages.Add((long.Parse(split[0]), MessageFactory.FromStringArray(split.Skip(1).ToArray())));
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
                    grpcClient.SendIMUData(currentPackage.imuDataMessage);
                    index++;
                    currentPackage = listOfParsedReceivedPackages[index];
                }
            }
        }
    }
}