## SteamA2S
Simple [steam query](https://developer.valvesoftware.com/wiki/Server_queries) wrapper for C#.
### Example Usage
```cs
A2SInfoPacket packet = await A2SDriver.GetA2SInfoAsync(new IPEndPoint(IPAddress.Parse("my-ip-address.1337"), 2303));
Console.WriteLine(packet.Name);
```
There are two main models, [A2SInfoPacket](https://github.com/sharplol/SteamA2S/blob/master/SteamA2S/Models/A2SInfoPacket.cs) and [A2SPlayerPacket](https://github.com/sharplol/SteamA2S/blob/master/SteamA2S/Models/A2SPlayerPacket.cs).
The first one represents a game server state, the latter represents some online player.<br/>
### Notes
The implementation is a simple fire and forget, if you're querying constantly, this library might not be suitable for you, as it does not allow you to keep a UDP socket alive (if I'm not lazy, someday I'll implement this "properly").<br/>
Currently there might be some games that are not supported, I primarily wrote this because I needed something quick to query Arma 3 servers.
