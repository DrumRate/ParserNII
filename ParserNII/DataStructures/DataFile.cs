using System.Collections.Generic;

namespace ParserNII.DataStructures
{
    public class DataFile
    {
        public Dictionary<string, DataElement> Data = new Dictionary<string, DataElement>();

        
        // time data
        public readonly SecondBlock[] SecondsBlock = new SecondBlock[20];

        public DataFile Clone()
        {
            return new DataFile
            {
                Data = new Dictionary<string, DataElement>(Data)
            };
        }

    }
}