using System;
using ParserNII.Types.Abstract;

namespace ParserNII.Types
{
    public class SecondRecord : StructRecord
    {
        private int _inBufferPosition;
        private Time _time;

        public SecondRecord(int orderNumber, int byteCount, Time time) : base(orderNumber, byteCount)
        {
            if (byteCount != 22)
                throw new Exception("SecondRecord must be 22-byte size");
            Buffer = new byte[byteCount];
            _time = time;
            
            _inBufferPosition = 0;
        }

        public override void Read()
        {
            Fs = MyFileStream.GetFileStreamInstance();
            Fs.Read(Buffer, 0, ByteCount);
        }

        public override void Show()
        {
            /*
             * offset   val     len
             * 0        1       2
             * 2        2       2
             * 4        3       2
             * 6        4       2
             * 8        5       2
             * 10       6       2
             * 12       7       2
             * 14       8       1
             * 15       9       2
             * 17       10      1
             * 18       11      1
             * 19       12      1
             * 20       13      1
             * 21       14      1
             */

            Console.WriteLine($"\n---Start second block record #{OrderNumber}");

            Console.WriteLine($"\tTime:\t{_time.Dt.AddSeconds((OrderNumber - 1) * 3)}");

            for (int i = 0; i < 14; i++)
            {
                if (i == 7 || i > 8)
                {
                    Console.WriteLine($"\tValue: {i}\t{Buffer[_inBufferPosition]}");
                    _inBufferPosition++;
                }
                else
                {
                    Console.WriteLine($"\tValue: {i}\t{Buffer[_inBufferPosition] << 8 | Buffer[_inBufferPosition+1] & 0xFF}");
                    _inBufferPosition += 2;
                }
            }

            Console.WriteLine("---Exit block record\n");
        }

        public override void ShowBytesAsHex()
        {
            Console.WriteLine($"#\tSecondRecord:\tHEX readed: {BitConverter.ToString(Buffer)}\n\t next pos: {Fs.Position}");
        }
    }
}