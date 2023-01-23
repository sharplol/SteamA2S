using System.Net;

namespace SteamA2S.Models;

/// <summary>
/// Represents an <see href="https://developer.valvesoftware.com/wiki/Server_queries#A2S_INFO">A2S_INFO packet.</see>.
/// </summary>
public record A2SInfoPacket(
    IPEndPoint QueryIPEndPoint,
    byte Protocol,
    string Name,
    string Map,
    string Game,
    string Gamemode,
    ushort AppId,
    byte OnlinePlayers,
    byte MaxPlayers,
    byte Bots,
    ServerType Type,
    ServerEnvironment Environment,
    bool Visiblity,
    bool VacEnabled,
    string Version,
    IPEndPoint GameIPEndPoint,
    ulong? SteamId,
    A2SSourceTVSpectator? Spectator,
    string? Keywords,
    ulong? AppIdFull,
    IReadOnlyList<A2SPlayerPacket> PlayerData) : IBasicServerInfoPacket
{
    IReadOnlyList<IBasicPlayerInfoPacket> IBasicServerInfoPacket.PlayerData => PlayerData;
}