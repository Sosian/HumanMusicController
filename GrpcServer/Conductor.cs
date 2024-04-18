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
    private readonly Speed speedY;
    private readonly Speed speedZ;

    private readonly int[,] mappingArraySpeed = {
            {0, 15, 50, 75, 100, 125, 150, 175, 200, 225, 250},
            {1000, 950, 900, 800, 700, 600, 500, 400, 300, 200, 100}
        };

    public Conductor(ILogger<Conductor> logger, IOscSender oscSender)
    {
        this.logger = logger;
        this.oscSender = oscSender;
        speedX = new Speed(oscSender, "gx", mappingArraySpeed);
        speedY = new Speed(oscSender, "gy", mappingArraySpeed);
        speedZ = new Speed(oscSender, "gz", mappingArraySpeed);
    }

    public void PlayManually()
    {

    }

    public void ReceiveIMUDataMessage(IMUDataMessage iMUDataMessage)
    {
        //speedX.SetSpeed(iMUDataMessage.Gx);
        speedY.SetSpeed(iMUDataMessage.Gy);
        speedZ.SetSpeed(iMUDataMessage.Gz);
    }

    public void ReceiveHeartrateDataMessage(HeartrateDataMessage heartrateDataMessage)
    {
    }
}
