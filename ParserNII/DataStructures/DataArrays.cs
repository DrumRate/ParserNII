﻿using System.Collections.Generic;

namespace ParserNII.DataStructures
{
    public class DataArrays
    {
        public readonly Dictionary<string, DataElement[]> Data = new Dictionary<string, DataElement[]>();

        // time data
        public readonly SecondBlock[][] SecondsBlock = new SecondBlock[20][];

    }
}