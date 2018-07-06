using System;
using System.Collections.Generic;
using System.IO;

namespace ParserNII.DataStructures
{
    public class BinFileParser
    {
        public List<BinFile> Parse(Stream stream)
        {
            List<BinFile> result = new List<BinFile>();
            double timeNowEpoch = Convert.ToInt64(DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds);
            using (BinaryReader reader = new BinaryReader(stream))
            {
                while (stream.Position < stream.Length)
                {
                    long time = reader.ReadInt64() * 1000;
                    int uid = reader.ReadInt32();
                    double value = reader.ReadDouble();


                    if (time > (timeNowEpoch * 1000))
                        continue;

                    if ((uid == 2 || uid == 6 || uid == 9 || uid == 19
                        || uid == 20 || uid == 50 || uid == 101
                        || uid == 3101 || uid == 3102 || uid == 3104)
                        && (value > 127))
                    {
                        value -= 256;
                    }

                    result.Add(new BinFile
                    {
                        Date = time,
                        Uid = uid,
                        Value = value
                    });
                }
            }

            return result;
        }
    }
}