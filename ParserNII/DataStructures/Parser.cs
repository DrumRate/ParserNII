using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ParserNII.DataStructures
{
    public abstract class Parser
    {
        public abstract List<DataFile> Parse(byte[] fileBytes);

        public DataArrays ToArray(List<DataFile> data)
        {
            var result = new DataArrays();

            var keys = data[0].Data.Keys;

            foreach (var key in keys)
            {
                result.Data.Add(key, data.Select(d => d.Data[key]).ToArray());
            }

            return result;
        }
    }
}