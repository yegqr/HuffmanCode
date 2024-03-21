public class BitStream
{
    private readonly FileStream _stream;
    private byte _currentByte;
    private int _bitCount;

    public BitStream(FileStream stream)
    {
        _stream = stream;
        _currentByte = 0;
        _bitCount = 0;
    }

    public void WriteBit(bool bit)
    {
        if (bit)
        {
            _currentByte |= (byte)(1 << (7 - _bitCount));
        }

        _bitCount++;

        if (_bitCount == 8)
        {
            _stream.WriteByte(_currentByte);
            _currentByte = 0;
            _bitCount = 0;
        }
    }

    public void WriteBit(string bit)
    {
        foreach (var el in bit)
        {
            if (el == '1')
            {
                _currentByte |= (byte)(1 << (7 - _bitCount));
            }

            _bitCount++;

            if (_bitCount == 8)
            {
                _stream.WriteByte(_currentByte);
                _currentByte = 0;
                _bitCount = 0;
            }
        }
    }

    public void Flush()
    {
        if (_bitCount > 0)
        {
            _stream.WriteByte(_currentByte);
            _currentByte = 0;
            _bitCount = 0;
        }
    }
}