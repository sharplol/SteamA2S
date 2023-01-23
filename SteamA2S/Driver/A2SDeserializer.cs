using SteamA2S.Helpers;
using SteamA2S.Models;
using System.Net;

namespace SteamA2S.Driver;

internal class A2SDeserializer : TrivialTypeDeserializer
{
    private readonly IPEndPoint _ipEndPoint;
    private readonly byte[] _infoBytes;
    private readonly byte[] _playerBytes;
    internal A2SDeserializer(IPEndPoint endPoint, byte[] infoBytes, byte[] playerBytes) : base(infoBytes)
    {
        _ipEndPoint = endPoint;
        _infoBytes = infoBytes;
        _playerBytes = playerBytes;
    }
    internal A2SInfoPacket DeserializeA2SInfoPacket()
    {
        LoadBuffer(_infoBytes);
        MoveRange(5);
        byte EDF;
        return new(
            _ipEndPoint,
            Byte(),
            String(),
            String(),
            String(),
            String(),
            UInt16(),
            Byte(),
            Byte(),
            Byte(),
            (ServerType)Byte(),
            (ServerEnvironment)Byte(),
            !Bool(),
            Bool(),
            String(),
            ((EDF = Byte()) & 0x80) != 0 ? new(_ipEndPoint.Address, UInt16()) : _ipEndPoint,
            (EDF & 0x10) != 0 ? UInt64() : 0,
            (EDF & 0x40) != 0 ? new(UInt16(), String()) : null,
            (EDF & 0x20) != 0 ? String() : null,
            (EDF & 0x01) != 0 ? UInt64() : null,
            GetA2SPlayers().ToList());
    }
    private IEnumerable<A2SPlayerPacket> GetA2SPlayers()
    {
        LoadBuffer(_playerBytes);
        MoveRange(6);
        while (!Eof())
        {
            Byte(); // index
            yield return new(String(), Int32(), Float());
        }
    }
    private void MoveRange(int length)
    {
        while (length-- > 0 && !Eof())
        {
            Move();
        }
    }
}
