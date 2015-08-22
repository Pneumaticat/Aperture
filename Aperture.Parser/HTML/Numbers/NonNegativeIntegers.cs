using Aperture.Parser.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aperture.Parser.HTML.Numbers
{
    public static class NonNegativeIntegers
    {
        // 2.4.4.2 Non-negative integers
        // https://html.spec.whatwg.org/multipage/infrastructure.html#non-negative-integers
        // TODO: Add "is non negative integer" method?
        public static int ParseNonNegativeInteger(string input)
        {
            int value = SignedIntegers.ParseInteger(input);
            if (value < 0)
                // TODO: Maybe use another, "InvalidHtmlNonNegativeIntegerException"?
                throw new InvalidIntegerException("A non-negative integer cannot be less than zero.");
            return value;
        }
    }
}
