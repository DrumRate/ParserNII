namespace ParserLab.Types.Abstract
{
    public abstract class StructRecord : Record
    {
        protected StructRecord(int orderNumber, int byteCount) : base(orderNumber, byteCount)
        {
        }
    }
}