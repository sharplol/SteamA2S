using System.Net;

namespace SteamA2S.Models;
public interface IBasicServerInfoPacket
{
    IPEndPoint GameIPEndPoint { get; }
    string Name { get; }
    IReadOnlyList<IBasicPlayerInfoPacket> PlayerData { get; }
    byte OnlinePlayers { get; }
    byte MaxPlayers { get; }
}
