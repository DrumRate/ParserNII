using System;
using ParserLab.Types.Abstract;

namespace ParserLab.Types
{
    public class Time : Record
    {
        private DateTime _dt;
        private long _bufferTime;

        public DateTime Dt => _dt;

        public Time(int orderNumber, int byteCount) : base(orderNumber, byteCount)
        {
            _dt = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            if (byteCount != 4)
                throw new Exception("Time must be 4-byte size");
            Buffer = new byte[byteCount];
        }

        public override void Read()
        {
            Fs = MyFileStream.GetFileStreamInstance();
            Fs.Read(Buffer, 0, ByteCount);
            _bufferTime = (long) Buffer[3] << 24 | (long)(Buffer[2] & 0xFF) << 16 | (long)(Buffer[1] & 0xFF) << 8 | Buffer[0];
            _dt = _dt.AddSeconds(_bufferTime).AddHours(3);
        }

        public override void Show()
        {
            Console.WriteLine($"&\tTime:\t{_dt}");
            Console.WriteLine($"&\tTime (Unix Timestamp):\t{_bufferTime}\n");
        }

        public override void ShowBytesAsHex()
        {
            Console.WriteLine($"&\tTime:\t\tHEX readed: {BitConverter.ToString(Buffer)}\n\t next pos: {Fs.Position}");
        }
        
    }
}