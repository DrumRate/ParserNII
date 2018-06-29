using System;
using System.Collections;
using ParserLab.Types.Abstract;

namespace ParserLab.Types
{
    public class MinuteBitRecord1 : StructRecord
    {

        public MinuteBitRecord1(int orderNumber, int byteCount) : base(orderNumber, byteCount)
        {
            if (byteCount != 2)
                throw new Exception("MinuteRecord1 must be 2-byte size");
            Buffer = new byte[byteCount];
        }

        public override void Read()
        {
            Fs = MyFileStream.GetFileStreamInstance();
            Fs.Read(Buffer, 0, ByteCount);
        }

        public override void Show()
        {
            Console.WriteLine("\n---Start minute block record type 1");

            BitArray bits = new BitArray(Buffer);

            for (int i = 0; i < ByteCount * 8; i++)
            {
                Console.WriteLine($"\tValue: {i}\t{(bits[i] ? "1" : "0")}");
            }

            Console.WriteLine("---Exit block record\n");
        }

        public override void ShowBytesAsHex()
        {
            Console.WriteLine($"#\tMinuteRecord1\tHEX readed: {BitConverter.ToString(Buffer)}\n\t next pos: {Fs.Position}");
        }
    }
}