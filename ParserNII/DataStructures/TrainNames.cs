﻿using System.Collections.Generic;

namespace ParserNII.DataStructures
{
    public class TrainNames
    {
        public static readonly Dictionary<byte, string> NamesDictionary = new Dictionary<byte, string>
        {
            { 113, "2М62" },
            { 127, "2М62АКБ" },
            { 114, "2М62У" },
            { 128, "2М62УС" },
            { 108, "2ТЭ10В" },
            { 109, "2ТЭ10Л" },
            { 106, "2ТЭ10М" },
            { 115, "2ТЭ10МК" },
            { 117, "2ТЭ10С" },
            { 107, "2ТЭ10У" },
            { 123, "2ТЭ10УК" },
            { 111, "2ТЭ116" },
            { 119, "3ТЭ10В" },
            { 110, "3ТЭ10М" },
            { 116, "3ТЭ10МК" },
            { 120, "3ТЭ10С" },
            { 118, "3ТЭ10У" },
            { 124, "3ТЭ10УК" },
            { 121, "4ТЭ10МК" },
            { 122, "4ТЭ10С" },
            { 125, "4ТЭ10УК" },
            { 112, "ДМ62/М62" },
            { 126, "М62АКБ" },
            { 99, "ТЭМ1" },
            { 95, "ТЭМ18" },
            { 93, "ТЭМ18В" },
            { 101, "ТЭМ18Д" },
            { 92, "ТЭМ18ДМ" },
            { 102, "ТЭМ2" },
            { 98, "ТЭМ2У" },
            { 100, "ТЭМ3" },
            { 96, "ТЭМ7" },
            { 94, "ТЭМ7А" },
            { 97, "ТЭП70" },
            { 103, "ЧМЭ3" },
            { 104, "ЧМЭ3Т" },
            { 105, "ЧМЭ3Э" }
        };
    }
}