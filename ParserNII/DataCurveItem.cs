using System.Drawing;
using System.Runtime.Serialization;
using ParserNII.DataStructures;
using ZedGraph;

namespace ParserNII
{
    public class DataCurveItem : LineItem

    {
        public DataCurveItem(string label) : base(label)
        {
        }

        public DataCurveItem(string label, double[] x, double[] y, Color color, SymbolType symbolType, float lineWidth) : base(label, x, y, color, symbolType, lineWidth)
        {
        }

        public DataCurveItem(string label, double[] x, double[] y, Color color, SymbolType symbolType) : base(label, x, y, color, symbolType)
        {
        }

        public DataCurveItem(string label, IPointList points, Color color, SymbolType symbolType, float lineWidth) : base(label, points, color, symbolType, lineWidth)
        {
        }

        public DataCurveItem(string label, IPointList points, Color color, SymbolType symbolType, DataFile data) : base(label, points, color, symbolType)
        {

        }

        public DataCurveItem(LineItem rhs) : base(rhs)
        {
        }

        protected DataCurveItem(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}