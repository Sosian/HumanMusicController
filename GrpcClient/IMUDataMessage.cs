using GrpcClient;

public static class MessageFactory
{
    public static IMUDataMessage FromStringArray(string[] imuDataMessageParameters)
    {
        if (imuDataMessageParameters.Length != 7)
            throw new ArgumentException("imuDataMessageParameters seems to not have the required 7 fields. Something is wrong");
        //TODO: This will have to be altered everytime IMUDataMessage is changed. Is there a better way?
        return new IMUDataMessage()
        {
            Name = imuDataMessageParameters[0],
            X = float.Parse(imuDataMessageParameters[1]),
            Y = float.Parse(imuDataMessageParameters[2]),
            Z = float.Parse(imuDataMessageParameters[3]),
            Gx = float.Parse(imuDataMessageParameters[4]),
            Gy = float.Parse(imuDataMessageParameters[5]),
            Gz = float.Parse(imuDataMessageParameters[6]),
        };
    }
}