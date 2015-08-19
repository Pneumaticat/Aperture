using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aperture.Parser.Exceptions
{
    public class InvalidIntegerException : ParseException
    {
        public InvalidIntegerException() : base() { }
        public InvalidIntegerException(string message) : base(message) { }
    }
}
