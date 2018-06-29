using System;
using ParserLab.Types.Abstract;

namespace ParserLab.Types
{
    public class CrcRecord : Record
    {

        public CrcRecord(int orderNumber, int byteCount) : base (orderNumber, byteCount)
        {
            if (byteCount != 2)
                throw new Exception("CrcRecord must be 2-byte size");
            Buffer = new byte[byteCount];
        }

        public override void Read()
        {
            Fs = MyFileStream.GetFileStreamInstance();
            Fs.Read(Buffer, 0, ByteCount);
        }

        public override void Show()
        {
            Console.WriteLine($"???\tCRC VALUE:\t{Buffer[0] << 8 | Buffer[1] & 0xFF}");
        }

        public override void ShowBytesAsHex()
        {
            Console.WriteLine($"???\tCRC VALUE:\tHEX readed: {BitConverter.ToString(Buffer)}\n\t next pos: {Fs.Position}");
        }
    }
}