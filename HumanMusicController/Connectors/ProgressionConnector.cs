using Windows.Media.Capture.Core;

namespace HumanMusicController.Connectors
{
    public class ProgressionConnector : IConnector
    {
        private readonly MidiSender midiSender;
        private readonly VisualizationServerConnection visualizationServerConnection;

        private static int maxHeartrateSum = 30000;

        //The numbers for the levels are shared context between Connector and VisualizationServer
        private double firstLevel = 0.3;
        private double secondLevel = 0.75;
        private int currentLevel = 0;

        private List<int> listOfHeartrates = new List<int>();

        public ProgressionConnector(MidiSender midiSender, VisualizationServerConnection visualizationServerConnection)
        {
            this.midiSender = midiSender;
            this.visualizationServerConnection = visualizationServerConnection;
        }

        public void ReceiveData(HrPayload hrPayload)
        {
            //Final level reached, so stop sending anything
            if (currentLevel == 3)
                return;

            var heartrate = hrPayload.Heartrate;
            listOfHeartrates.Add(heartrate);

            //To make the notes start lower than the heartrate
            var key = heartrate - 50;
            midiSender.SendMidiMsg(key);

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
                Task.Delay(20000).ContinueWith((_) => midiSender.NoteOffEventForLastPlayedNote());
            }

            visualizationServerConnection.SendHeartrateToVisualizationServer(heartrate, currentProgress, currentLevel);
        }
    }
}