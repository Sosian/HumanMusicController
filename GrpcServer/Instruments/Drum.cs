using CoreOSC;

public class Drum
{
    private readonly IOscSender oscSender;
    private readonly string drumKind;
    private readonly int triggerValue;
    private readonly int durationThreshold;

    private bool playedDrum = false;
    private int playedDrumCountsAgo = 0;

    public Drum(IOscSender oscSender, string drumKind, int triggerValue, int durationThreshold)
    {
        this.oscSender = oscSender;
        this.drumKind = drumKind;
        this.triggerValue = triggerValue;
        this.durationThreshold = durationThreshold;
    }

    public void PlayDrum(float value)
    {
        if (value > triggerValue)
        {
            if (playedDrum && playedDrumCountsAgo < durationThreshold)
            {
                playedDrumCountsAgo++;
            }
            else
            {
                playedDrum = true;
                playedDrumCountsAgo = 0;

                var oscMessage = new OscMessage(new Address("/trigger/" + drumKind), [1]);
                oscSender.SendMessageAsync(oscMessage);
            }
        }
    }
}