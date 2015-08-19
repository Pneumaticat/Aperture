using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aperture.Parser.Exceptions
{
    public abstract class HtmlParseException : ApplicationException
    {
        public HtmlParseException() : base() { }
        public HtmlParseException(string message) : base(message) { }
    }
}
