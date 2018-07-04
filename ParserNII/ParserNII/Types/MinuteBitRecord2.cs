using System;
using System.Collections;
using ParserNII.Types.Abstract;

namespace ParserNII.Types
{
    public class MinuteBitRecord2 : StructRecord
    {

        public MinuteBitRecord2(int orderNumber, int byteCount) : base(orderNumber, byteCount)
        {
            if (byteCount != 1)
                throw new Exception("MinuteRecord2 must be 1-byte size");
            Buffer = new byte[byteCount];
        }

        public override void Read()
        {
            Fs = MyFileStream.GetFileStreamInstance();
            Fs.Read(Buffer, 0, ByteCount);
        }

        public override void Show()
        {
            Console.WriteLine("\n---Start minute block record type 2");

            BitArray bits = new BitArray(Buffer);

            for (int i = 0; i < ByteCount * 8; i++)
            {
                Console.WriteLine($"\tValue: {i}\t{(bits[i] ? "1" : "0")}");
            }

            Console.WriteLine("---Exit block record\n");
        }

        public override void ShowBytesAsHex()
        {
            Console.WriteLine($"#\tMinuteRecord2:\tHEX readed: {BitConverter.ToString(Buffer)}\n\t next pos: {Fs.Position}");
        }
    }
}