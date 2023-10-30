using System;
using HumanMusicController.Bluetooth;
using HumanMusicController.Connectors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace HumanMusicController
{
    public class Program
    {
        private const string midiPortName = "loopMIDI Port";
        private const string recordsDirectory = @"C:\Users\flori\Documents\HumanMusicController\Records";
        private const string visualizationServerUrl = "http://localhost:5253/heartbeatHub";
        private const string recordFileFullPath = @$"{recordsDirectory}\20230611T1138";

        public static async Task Main(string[] args)
        {
            if (args.Count() == 0)
                throw new Exception("Please specify mode 'Record', 'Live' or 'Replay'");

            using IHost host = Host.CreateDefaultBuilder(args).Build();

            var mode = args[0];
            var bluetoothDeviceRequired = mode == "Record" || mode == "Live";

            var midiSender = new MidiSender(midiPortName);
            var midiConnector = new MidiConnector(midiSender);
            var recordConnecter = new RecordConnector(recordsDirectory);
            var visualizationServerConnector = new VisualizationServerConnector(visualizationServerUrl);
            var midiVisualizationCompoundConnector = new CompoundConnector(new List<IConnector>() { midiConnector, visualizationServerConnector });

            if (bluetoothDeviceRequired)
            {
                var bluetoothDeviceFinder = new BluetoothDeviceFinder(host.Services.GetRequiredService<ILogger<BluetoothDeviceFinder>>());
                var bluetoothDevice = bluetoothDeviceFinder.GetDevice();
                var bluetoothDeviceSession = new BleDeviceSession(host.Services.GetRequiredService<ILogger<BleDeviceSession>>(), bluetoothDevice);

                PolarH10Bluetooth polarH10Bluetooth;

                if (mode == "Record")
                    polarH10Bluetooth = new PolarH10Bluetooth(host.Services.GetRequiredService<ILogger<PolarH10Bluetooth>>(), bluetoothDeviceSession, recordConnecter);
                else if (mode == "Live")
                    polarH10Bluetooth = new PolarH10Bluetooth(host.Services.GetRequiredService<ILogger<PolarH10Bluetooth>>(), bluetoothDeviceSession, midiVisualizationCompoundConnector);
                else
                    throw new Exception($"Argument '{args[0]}' not supported.");

                polarH10Bluetooth.Start();
            }
            else if (mode == "Replay")
            {
                var replayLogfile = new ReplayRecordfile(host.Services.GetRequiredService<ILogger<ReplayRecordfile>>(), midiVisualizationCompoundConnector);
                replayLogfile.Play(recordFileFullPath);
            }
            else
            {
                throw new Exception("Please specify mode 'Record', 'Live' or 'Replay'");
            }

            await host.RunAsync();
        }
    }
}