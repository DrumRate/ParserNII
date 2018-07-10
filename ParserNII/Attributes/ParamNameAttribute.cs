using System;

namespace ParserNII.Attributes
{
    public class ParamNameAttribute : Attribute
    {
        public readonly string Value;

        public ParamNameAttribute(string value)
        {
            Value = value;
        }
    }
}