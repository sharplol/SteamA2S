namespace SteamA2S.Models;
/// <summary>
/// Represents an <see href="https://developer.valvesoftware.com/wiki/Server_queries#A2S_PLAYER">A2S_PLAYER</see> packet.
/// </summary>
public record A2SPlayerPacket(string Name, long Score, float Playtime);