using CoreOSC;

public interface IOscSender
{
    public void SendMessageAsync(OscMessage oscMessage);
}
