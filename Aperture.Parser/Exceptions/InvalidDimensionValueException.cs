using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aperture.Parser.Exceptions
{
    public class InvalidDimensionValueException : ParseException
    {
        public InvalidDimensionValueException() : base() { }
        public InvalidDimensionValueException(string message) : base() { }
    }
}
