using System;
using ParserLab.Types.Abstract;

namespace ParserLab.Types
{
    public class SingleValue : Record
    {
        public SingleValue(int orderNumber, int byteCount) : base(orderNumber, byteCount)
        {
            Buffer = new byte[byteCount];
        }

        public override void Read()
        {
            Fs = MyFileStream.GetFileStreamInstance();
            Fs.Read(Buffer, 0, ByteCount);
        }

        public override void Show()
        {
            Console.Write($"*\tValue: {OrderNumber}\t");
            switch (ByteCount)
            {
                case 1:
                    Console.WriteLine($"{Buffer[0]}");
                    break;
                case 2:
                    Console.WriteLine($"{Buffer[1] << 8 | (Buffer[0] & 0xFF)}");
                    break;
                case 4:
                    Console.WriteLine($"{(long) Buffer[3] << 24 | (long) (Buffer[2] & 0xFF) << 16 | (long) (Buffer[1] & 0xFF) << 8 | (long) (Buffer[0] & 0xFF) }");
                    break;
            }
        }

        public override void ShowBytesAsHex()
        {
            Console.WriteLine($"*\tValue: {OrderNumber}\tHEX readed: {BitConverter.ToString(Buffer)}\n\t next pos: {Fs.Position}");
        }
    }
}