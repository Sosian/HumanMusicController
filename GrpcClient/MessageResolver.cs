using System.Text.Json;
using Grpc.Net.Client;
using GrpcClient;

namespace GrpcClient
{
    //Feels like bad design, but not sure how to do it differently with those generated types

    public static class MessageResolver
    {
        public static void SendGrpcMessage(SensorIPCService.SensorIPCServiceClient grpcClient, object message)
        {
            if (message is IMUDataMessage)
            {
                grpcClient.SendIMUData((IMUDataMessage)message);
            }
            else if (message is HeartrateDataMessage)
            {
                grpcClient.SendHeartrateData((HeartrateDataMessage)message);
            }
            else
            {
                throw new Exception("Type is not yet implemented");
            }
        }

        public static T FromJsonToObject<T>(JsonElement root)
        {

            if (typeof(T).FullName == "GrpcClient.IMUDataMessage")
            {
                return (T)(object)new IMUDataMessage()
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
            else if (typeof(T).FullName == "GrpcClient.HeartrateDataMessage")
            {
                return (T)(object)new HeartrateDataMessage()
                {
                    Name = root.GetProperty("name").GetString(),
                    Heartrate = (int)GetPropertyValueOrZero(root, "heartrate")
                };
            }
            else
            {
                throw new Exception("Type is not yet implemented");
            }

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
}
