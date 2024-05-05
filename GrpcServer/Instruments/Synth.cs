using CoreOSC;

public class Synth
{
    private readonly IOscSender oscSender;
    private readonly ILogger<Conductor> logger;
    private readonly string synth;
    private readonly double[,] mappingArray;

    public Synth(IOscSender oscSender, ILogger<Conductor> logger, string synth, double[,] mappingArray)
    {
        this.oscSender = oscSender;
        this.logger = logger;
        this.synth = synth;
        this.mappingArray = mappingArray;
    }

    public void PlaySynth(float value)
    {
        if (value > 1)
            value = 1;
        else if (value < 0)
            value *= -1;

        var oscMessage = new OscMessage(new Address($"/note/{synth}"), [(int)(value * 1000)]);
        oscSender.SendMessageAsync(oscMessage);
        //logger.LogWarning("oscMessageArguments " + (int)(value * 1000));

        // for (int i = 0; i < mappingArray.GetLength(1); i++)
        // {
        //     if (i >= mappingArray.GetLength(1) - 1)
        //     {
        //         var oscMessage = new OscMessage(new Address($"/note/{synth}"), [(int)mappingArray[1, i - 1]]);
        //         oscSender.SendMessageAsync(oscMessage);
        //     }
        //     else if ((mappingArray[0, i] < value) && (mappingArray[0, i + 1] > value))
        //     {
        //         var oscMessage = new OscMessage(new Address($"/note/{synth}"), [(int)mappingArray[1, i]]);
        //         oscSender.SendMessageAsync(oscMessage);
        //         break;
        //     }
        // }
    }
}