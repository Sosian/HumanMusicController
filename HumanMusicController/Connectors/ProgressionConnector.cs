using Windows.Media.Capture.Core;
using System.Media;

namespace HumanMusicController.Connectors
{
    public class ProgressionConnector : IConnector
    {
        private readonly MidiSender midiSender;
        private readonly VisualizationServerConnection visualizationServerConnection;

        private static int maxHeartrateSum = 32500;

        //The numbers for the levels are shared context between Connector and VisualizationServer
        private double firstLevel = 0.3;
        private double secondLevel = 0.75;
        private int currentLevel = 0;
        private SoundPlayer soundPlayer;

        private List<int> listOfHeartrates = new List<int>();

        public ProgressionConnector(MidiSender midiSender, VisualizationServerConnection visualizationServerConnection)
        {
            this.midiSender = midiSender;
            this.visualizationServerConnection = visualizationServerConnection;
            soundPlayer = new SoundPlayer
            {
                SoundLocation = @"C:\Users\flori\Documents\HumanMusicController\HumanMusicController\thirdLevelBreak.wav" // I know thats dirty, will be fixed
            };

            soundPlayer.Load();
        }

        public void ReceiveData(HrPayload hrPayload)
        {
            //Final level reached, so stop sending anything
            if (currentLevel == 3)
            {
                visualizationServerConnection.SendHeartrateToVisualizationServer(999, 999, currentLevel); //Just in case the last message does not reach the server, we send backup
                return;
            }

            var heartrate = hrPayload.Heartrate;
            listOfHeartrates.Add(heartrate);

            var sumOfHeartrates = listOfHeartrates.Sum();
            double currentProgress = (double)sumOfHeartrates / maxHeartrateSum;

            if (currentLevel == 0 && sumOfHeartrates >= maxHeartrateSum * firstLevel)
            {
                currentLevel = 1;
            }
            else if (currentLevel == 1 && sumOfHeartrates >= maxHeartrateSum * secondLevel)
            {
                currentLevel = 2;
            }
            else if (currentLevel == 2 && sumOfHeartrates >= maxHeartrateSum)
            {
                currentLevel = 3;
                Task.Delay(25000).ContinueWith((_) => midiSender.NoteOffEventForLastPlayedNote());
                soundPlayer.Play();
                Task.Delay(1000).ContinueWith((_) => midiSender.SendMidiMsg(30));

                return;
            }

            //To make the notes start lower than the heartrate
            var key = heartrate - 65;
            midiSender.SendMidiMsg(key);
            visualizationServerConnection.SendHeartrateToVisualizationServer(heartrate, currentProgress, currentLevel);
        }
    }
}