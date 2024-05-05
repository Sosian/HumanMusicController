using System.Diagnostics;
using CoreOSC;

public class Speed
{
    private readonly IOscSender oscSender;
    private readonly string name;
    private readonly int[,] mappingArray;

    Stopwatch stopwatch;
    int millisecondsWait = 0;

    public Speed(IOscSender oscSender, string name, int[,] mappingArray)
    {
        this.oscSender = oscSender;
        this.name = name;
        this.mappingArray = mappingArray;
    }

    public int SetSpeed(float value)
    {
        return SendSpeed(value);
    }

    public void SetSpeedWithWait(float value)
    {
        if (millisecondsWait == 0 || stopwatch?.ElapsedMilliseconds > millisecondsWait)
        {
            millisecondsWait = SendSpeed(value);
            stopwatch = Stopwatch.StartNew();
        }
    }

    private int SendSpeed(float value)
    {
        //So we dont have to care which direction it goes
        if (value < 0)
            value *= -1;

        for (int i = 0; i < mappingArray.GetLength(1); i++)
        {
            if (i >= mappingArray.GetLength(1) - 1)
            {
                var speed = mappingArray[1, i - 1];
                var oscMessage = new OscMessage(new Address($"/speed/{name}"), [speed]);
                oscSender.SendMessageAsync(oscMessage);
                return speed;
            }
            else if ((mappingArray[0, i] < value) && (mappingArray[0, i + 1] > value))
            {
                var speed = mappingArray[1, i];
                var oscMessage = new OscMessage(new Address($"/speed/{name}"), [speed]);
                oscSender.SendMessageAsync(oscMessage);
                return speed;
            }
        }

        return 0;
    }
}