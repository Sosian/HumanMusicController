using System.Diagnostics;
using GrpcClient;

public interface IConductor
{
    void PlayManually();
    void ReceiveIMUDataMessage(IMUDataMessage iMUDataMessage);
    void ReceiveHeartrateDataMessage(HeartrateDataMessage heartrateDataMessage);
}

public class Conductor : IConductor
{
    private readonly ILogger<Conductor> logger;
    private readonly IOscSender oscSender;
    private readonly Speed speedX;
    private readonly Synth synthX;
    private readonly Speed speedY;
    private readonly Speed speedZ;

    private int millisecondsWait = 0;
    private Stopwatch stopwatch;

    private readonly int[,] mappingArraySpeedX = {
            {0, 15, 50, 75, 100, 125, 150, 175, 200, 225, 250},
            {1000, 900, 800, 700, 600, 500, 400, 300, 20, 100, 50}
        };

    private readonly double[,] mappingArraySynthX = {
        {0, 0.01, 0.02, 0.03, 0.04, 0.05, 0.06, 0.07, 0.08, 0.09, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 0.91, 0.92, 0.93, 0.94, 0.95, 0.96, 0.97, 0.98, 0.99, 1},
        {150, 175, 200, 225, 250, 275, 300, 325, 350, 375, 400, 425, 450, 475, 500, 525, 550, 575, 600, 625, 650, 675, 700, 725, 750, 775, 800, 850, 900}
    };

    private readonly int[,] mappingArraySpeedY = {
            {0, 15, 50, 75, 100, 125, 150, 175, 200, 225, 250},
            {4000, 3600, 3200, 3000, 2750, 2500, 2000, 1500, 1000, 500, 250}
        };

    private readonly int[,] mappingArraySpeedZ = {
            {0, 15, 50, 75, 100, 125, 150, 175, 200, 225, 250},
            {3000, 2750, 2000, 1750, 1500, 1250, 1000, 750, 500, 250, 100}
        };

    public Conductor(ILogger<Conductor> logger, IOscSender oscSender)
    {
        this.logger = logger;
        this.oscSender = oscSender;
        speedX = new Speed(oscSender, "gx", mappingArraySpeedX);
        synthX = new Synth(oscSender, logger, "x", mappingArraySynthX);

        speedY = new Speed(oscSender, "gy", mappingArraySpeedY);
        speedZ = new Speed(oscSender, "gz", mappingArraySpeedZ);
    }

    public void PlayManually()
    {

    }

    public void ReceiveIMUDataMessage(IMUDataMessage iMUDataMessage)
    {
        if (millisecondsWait == 0 || stopwatch?.ElapsedMilliseconds > millisecondsWait)
        {

            millisecondsWait = speedX.SetSpeed(iMUDataMessage.Gx);
            synthX.PlaySynth(iMUDataMessage.X);
            stopwatch = Stopwatch.StartNew();
        }


        // speedY.SetSpeed(iMUDataMessage.Gy);
        // speedZ.SetSpeed(iMUDataMessage.Gz);
    }

    public void ReceiveHeartrateDataMessage(HeartrateDataMessage heartrateDataMessage)
    {
    }
}
