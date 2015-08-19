using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aperture.Parser.Exceptions
{
    public abstract class ParseException : ApplicationException
    {
        public ParseException() : base() { }
        public ParseException(string message) : base(message) { }
    }
}
