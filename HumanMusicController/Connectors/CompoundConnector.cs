using HumanMusicController.Connectors;


namespace HumanMusicController
{
    internal class CompoundConnector : IConnector
    {
        private readonly List<IConnector> connectors;

        public CompoundConnector(List<IConnector> connectors)
        {
            this.connectors = connectors;
        }

        public void ReceiveData(HrPayload hrPayload)
        {
            foreach (var connector in connectors)
            {
                connector.ReceiveData(hrPayload);
            }
        }
    }
}