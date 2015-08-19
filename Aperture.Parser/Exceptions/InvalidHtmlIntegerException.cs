using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aperture.Parser.Exceptions
{
    public class InvalidHtmlIntegerException : HtmlParseException
    {
        public InvalidHtmlIntegerException() : base() { }
        public InvalidHtmlIntegerException(string message) : base(message) { }
    }
}
