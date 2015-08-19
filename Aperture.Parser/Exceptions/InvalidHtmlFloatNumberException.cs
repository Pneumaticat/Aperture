using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aperture.Parser.Exceptions
{
    class InvalidHtmlFloatNumberException : HtmlParseException
    {
        public InvalidHtmlFloatNumberException() : base() { }
        public InvalidHtmlFloatNumberException(string message) : base() { }
    }
}
