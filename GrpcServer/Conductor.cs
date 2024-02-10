using GrpcClient;

public interface IConductor
{
    void ReceiveIMUDataMessage(IMUDataMessage iMUDataMessage);
    void ReceiveHeartrateDataMessage(HeartrateDataMessage heartrateDataMessage);
}

public class Conductor : IConductor
{
    private readonly ILogger<Conductor> logger;
    private readonly IOscSender oscSender;
    private readonly Drum drum1;
    private readonly Drum drum2;
    private readonly Drum drum3;
    private readonly Synth synth;
    private readonly Bpm bpm;
    private readonly double[,] mappingArray = {
            {0, 0.01, 0.02, 0.03, 0.04, 0.05, 0.06, 0.07, 0.08, 0.09, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 0.91, 0.92, 0.93, 0.94, 0.95, 0.96, 0.97, 0.98, 0.99, 1},
            {35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63}
        };

    public Conductor(ILogger<Conductor> logger, IOscSender oscSender)
    {
        this.logger = logger;
        this.oscSender = oscSender;
        drum1 = new Drum(oscSender, "drum1", 100, 10);
        drum2 = new Drum(oscSender, "drum2", 100, 10);
        drum3 = new Drum(oscSender, "drum3", 100, 10);
        synth = new Synth(oscSender, logger, mappingArray);
        bpm = new Bpm(oscSender);
    }

    public void ReceiveIMUDataMessage(IMUDataMessage iMUDataMessage)
    {
        drum1.PlayDrum(iMUDataMessage.Gy);
        drum2.PlayDrum(iMUDataMessage.Gx);
        drum3.PlayDrum(iMUDataMessage.Gz);
        synth.PlaySynth(iMUDataMessage.X);
    }

    public void ReceiveHeartrateDataMessage(HeartrateDataMessage heartrateDataMessage)
    {
        bpm.ChangeSpeed(heartrateDataMessage.Heartrate);
    }
}
