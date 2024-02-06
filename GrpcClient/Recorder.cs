using System.Diagnostics;

namespace GrpcClient
{
    public class Recorder<T> : IRecorder<T>
    {
        private readonly Stopwatch stopwatch;
        private readonly string fullPath;

        public Recorder(string parentPath)
        {
            this.stopwatch = new Stopwatch();
            this.fullPath = Path.Combine(parentPath, DateTime.Now.ToString("yyyyMMddTHHmm"));
        }

        public void ReceiveData(T data)
        {
            if (data == null)
                return;

            if (!stopwatch.IsRunning)
                stopwatch.Start();

            var logLine = $"{{\"time\": {stopwatch.ElapsedMilliseconds}, \"data\": {data.ToString()}}},";
            File.AppendAllLines(fullPath, new string[] { logLine });
        }
    }
}