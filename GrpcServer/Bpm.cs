using CoreOSC;

public class Bpm
{
    private readonly IOscSender oscSender;

    public Bpm(IOscSender oscSender)
    {
        this.oscSender = oscSender;
    }

    public void ChangeSpeed(int value)
    {
        var oscMessage = new OscMessage(new Address("/speed"), [value]);
        oscSender.SendMessageAsync(oscMessage);
    }
}