using Aperture.Parser.Exceptions;
using Aperture.Parser.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aperture.Parser.HTML.Microsyntaxes.Numbers
{
    public static class SignedIntegers
    {
        // 2.4.4.1 Signed integers
        // https://html.spec.whatwg.org/multipage/infrastructure.html#signed-integers
        public static int ParseInteger(string input)
        {
            int position = 0;
            string sign = "positive";

            ParserIdioms.SkipWhitespace(input, ref position);

            if (position >= input.Length)
                // TODO: This may not be the kind of error they were looking for.
                throw new InvalidIntegerException("Integer string contains only whitespace.");

            if (input[position] == '-')
            {
                sign = "negative";
                position++;
                if (position >= input.Length)
                    throw new InvalidIntegerException("Integer string contains only a negative sign.");
            }
            else if (input[position] == '+')
            {
                ParserLogging.LogNCRecoverableError(
                    NonConformingError.PlusSignAtBeginningOfIntegerString);

                position++;
                if (position >= input.Length)
                    throw new InvalidIntegerException("Integer string contains only a positive sign.");
            }

            if (!ParserIdioms.ASCIIDigits.Contains(input[position]))
                throw new InvalidIntegerException("Character in integer string is not an ASCII digit.");

            string digits = ParserIdioms.CollectSequenceOfCharacters(
                input,
                ref position,
                (char ch) => ParserIdioms.ASCIIDigits.Contains(ch));

            int value = int.Parse(digits);
            if (sign == "positive")
                return value;
            else
                return 0 - value;
        }
    }
}
