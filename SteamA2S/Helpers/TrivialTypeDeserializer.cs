using System.Text;

namespace SteamA2S.Helpers;

internal abstract class TrivialTypeDeserializer
{
    private byte[] _buffer;
    private int _offset;
    private int _length;
    private protected TrivialTypeDeserializer(byte[] buffer)
    {
        _buffer = buffer;
        _length = buffer.Length;
    }
    private protected byte Byte() => Pop();
    private protected sbyte SByte() => (sbyte)Pop();
    private protected bool Bool() => Pop() == 0x01;
    private protected ushort UInt16() => BitConverter.ToUInt16(Pop(2), 0);
    private protected uint UInt32() => BitConverter.ToUInt32(Pop(4), 0);
    private protected ulong UInt64() => BitConverter.ToUInt64(Pop(8), 0);
    private protected int Int32() => BitConverter.ToInt32(Pop(4), 0);
    private protected float Float() => BitConverter.ToSingle(Pop(4), 0);
    private protected bool Eof() => _offset >= _length;
    private protected bool IsNonZero() => CurrentByte() != 0x00;
    private protected byte CurrentByte() => _buffer[_offset];
    private protected int Move() => _offset++;
    private protected byte Pop() => _buffer[Move()];
    private protected string String()
    {
        int offset = _offset;
        while (CurrentByte() != 0x00)
        {
            Move();
        }

        Move();
        return Encoding.UTF8.GetString(_buffer.Skip(offset).Take(_offset - offset - 1).ToArray());
    }
    private protected byte[] Pop(int size)
    {
        byte[] buffer = new byte[size];
        for (int i = 0; i < size; i++)
        {
            buffer[i] = Pop();
        }
        return buffer;
    }
    private protected void LoadBuffer(byte[] buffer)
    {
        _buffer = buffer;
        _offset = 0;
        _length = buffer.Length;
    }
}