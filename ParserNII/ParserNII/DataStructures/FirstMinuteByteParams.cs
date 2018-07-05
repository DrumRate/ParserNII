using System;

namespace ParserNII.DataStructures
{
    [Flags]
    public enum FirstMinuteByteParams : short
    {
        DUTsInAPKFloaters = 1,
        IsLocomotiveMappresent = 8,
        IsDriverPresent = 16,
        FirstFileSource = 32,
        SecoondFileSource = 64

    }
}