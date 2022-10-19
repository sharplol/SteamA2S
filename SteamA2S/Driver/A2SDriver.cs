using SteamA2S.Models;
using System.Net.Sockets;
using System.Net;

namespace SteamA2S.Driver;
/// <summary>
/// Represents a query driver for use with querying steam servers.
/// </summary>
public static class A2SDriver
{
    private static readonly byte[] _sourceEngineQueryBytes = new byte[] { 
        0xFF, 0xFF, 0xFF, 0xFF, 
        0x54, 0x53, 0x6F, 0x75, 
        0x72, 0x63, 0x65, 0x20, 
        0x45, 0x6e, 0x67, 0x69, 
        0x6e, 0x65, 0x20, 0x51, 
        0x75, 0x65, 0x72, 0x79, 
        0x00 
    };
    private const byte SOURCE_ENGINE_QUERY_SIZE = 25;
    private static readonly byte[] _challengeNumberBytes = new byte[]
    {
        0xFF, 0xFF, 0xFF, 0xFF,
        0x55, 0xFF, 0xFF, 0xFF, 
        0xFF
    };
    private const int CHALLENGE_NUMBER_SIZE = 9;
    /// <summary>
    /// Query a given <paramref name="endPoint"/>, the port must be the query port, not game port!
    /// </summary>
    /// <param name="endPoint"><see cref="IPEndPoint"/> to query.</param>
    /// <param name="recieveTimeout">Recieve timeout ms.</param>
    /// <param name="sendTimeout">Send timeout in ms.</param>
    /// <returns></returns>
    /// <exception cref="SocketException"></exception>
    public static async Task<A2SInfoPacket> GetA2SInfoAsync(IPEndPoint endPoint, int recieveTimeout = 5000, int sendTimeout = 5000)
    {
        UdpClient udpClient = new();
        udpClient.Client.ReceiveTimeout = recieveTimeout;
        udpClient.Client.SendTimeout = sendTimeout;
        udpClient.Connect(endPoint);
        await udpClient.SendAsync(_sourceEngineQueryBytes, SOURCE_ENGINE_QUERY_SIZE);
        Task<UdpReceiveResult> udlRecieveResult = udpClient.ReceiveAsync();
        if (!udlRecieveResult.Wait(recieveTimeout))
        {
            throw new SocketException(10060);
        }

        UdpReceiveResult infoResult = udlRecieveResult.Result;
        await udpClient.SendAsync(_challengeNumberBytes, CHALLENGE_NUMBER_SIZE);
        UdpReceiveResult challengeResult = await udpClient.ReceiveAsync();
        List<byte> requestInfo = new() { 0xFF, 0xFF, 0xFF, 0xFF, 0x55 };
        requestInfo.AddRange(challengeResult.Buffer.Skip(5).Take(4));
        await udpClient.SendAsync(requestInfo.ToArray(), requestInfo.Count);
        UdpReceiveResult playerResult = await udpClient.ReceiveAsync();
        while (playerResult.Buffer[4] != 0x44)
        {
            playerResult.Buffer[4] = 0x55;
            await udpClient.SendAsync(playerResult.Buffer, playerResult.Buffer.Length);
            playerResult = await udpClient.ReceiveAsync();
        }

        udpClient.Close();
        udpClient.Dispose();
        return new A2SDeserializer(endPoint, infoResult.Buffer, playerResult.Buffer).DeserializeA2SInfoPacket();
    }
}
