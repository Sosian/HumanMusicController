using System.Text.Json;
using GrpcClient;

public static class MessageFactory
{
    public static IMUDataMessage FromStringArray(JsonElement root)
    {
        //TODO: This will have to be altered everytime IMUDataMessage is changed. Is there a better way?
        return new IMUDataMessage()
        {
            Name = root.GetProperty("name").GetString(),
            X = GetPropertyValueOrZero(root, "x"),
            Y = GetPropertyValueOrZero(root, "y"),
            Z = GetPropertyValueOrZero(root, "z"),
            Gx = GetPropertyValueOrZero(root, "gx"),
            Gy = GetPropertyValueOrZero(root, "gy"),
            Gz = GetPropertyValueOrZero(root, "gz"),
        };
    }

    private static float GetPropertyValueOrZero(JsonElement jsonElement, string propertyName)
    {
        if (jsonElement.TryGetProperty(propertyName, out var property))
        {
            return (float)property.GetDouble();
        }
        else
        {
            return 0;
        }
    }
}