using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aperture.Parser.Exceptions
{
    class InvalidFloatNumberException : ParseException
    {
        public InvalidFloatNumberException() : base() { }
        public InvalidFloatNumberException(string message) : base() { }
    }
}
