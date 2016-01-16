using System;

namespace MaxControl
{
    public class ParseException : Exception
    {
        public ParseException(string s)
            : base(s)
        {

        }
    }
}