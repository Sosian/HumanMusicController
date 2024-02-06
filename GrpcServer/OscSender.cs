using System.Net.Sockets;
using CoreOSC;
using CoreOSC.IO;

public class OscSender : IOscSender
{
    private readonly string ipaddress;
    private readonly int port;

    public OscSender(string ipaddress, int port)
    {
        this.ipaddress = ipaddress;
        this.port = port;
    }

    public void SendMessageAsync(OscMessage oscMessage)
    {
        using (var udpClient = new UdpClient(ipaddress, port))
        {
            udpClient.SendMessageAsync(oscMessage);
        }
    }
}
