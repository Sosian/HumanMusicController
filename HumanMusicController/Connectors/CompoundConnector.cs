namespace HumanMusicController.Connectors
{
    public class CompoundConnector : IConnector
    {
        private readonly IEnumerable<IConnector> connectors;
        public CompoundConnector(IEnumerable<IConnector> connectors)
        {
            this.connectors = connectors;
        }

        public void ReceiveData(HrPayload hrPayload)
        {
            foreach (var connector in connectors)
                connector.ReceiveData(hrPayload);
        }
    }
}