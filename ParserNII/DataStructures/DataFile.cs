using System.Collections.Generic;

namespace ParserNII.DataStructures
{
    public class DataFile
    {
        public Dictionary<string, DataElement> Data = new Dictionary<string, DataElement>();

        public DataFile Clone()
        {
            return new DataFile
            {
                Data = new Dictionary<string, DataElement>(Data)
            };
        }

    }
}