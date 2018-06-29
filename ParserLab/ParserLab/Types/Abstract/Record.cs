using System.IO;

namespace ParserLab.Types.Abstract
{
    public abstract class Record : IReflexible
    {
        protected int ByteCount;

        protected FileStream Fs;

        protected byte[] Buffer;

        protected int OrderNumber;

        protected Record(int orderNumber, int byteCount)
        {
            ByteCount = byteCount;
            OrderNumber = orderNumber;
        }

        public abstract void Read();

        public abstract void Show();

        public void Operate()
        {
            Read();
            Show();
        }

        public abstract void ShowBytesAsHex();

        public void OperateAsHex()
        {
            Read();
            ShowBytesAsHex();
        }

    }
}