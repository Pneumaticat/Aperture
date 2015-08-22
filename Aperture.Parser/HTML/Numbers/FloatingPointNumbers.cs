using Aperture.Parser.Exceptions;
using Aperture.Parser.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aperture.Parser.HTML.Numbers
{
    public static class FloatingPointNumbers
    {
        // 2.4.4.3 Floating-point numbers
        // https://html.spec.whatwg.org/multipage/infrastructure.html#floating-point-numbers

        /// <summary>
        /// So maybe I completely reimplemented a floating point parser. Don't 
        /// judge me!
        /// </summary>
        /// <param name="input"></param>
        /// <returns>A double-precision floating point.</returns>
        public static double ParseFloatingPointNumber(string input)
        {
            int position = 0;
            // TODO: Not sure what type this value should have.
            double value = 1;
            double divisor = 1;
            int exponent = 1;

            ParserIdioms.SkipWhitespace(input, ref position);

            if (position >= input.Length)
                throw new InvalidFloatNumberException("Floating-point number consists only of whitespace.");

            if (input[position] == '-')
            {
                value = -1;
                divisor = -1;
                position++;
                if (position >= input.Length)
                    throw new InvalidFloatNumberException("Floating-point number consists only of a minus sign.");
            }
            else if (input[position] == '+')
            {
                ParserLogging.LogNCRecoverableError(
                    NonConformingError.PlusSignAtBeginningOfFloatingPointString);
                position++;
                if (position >= input.Length)
                    throw new InvalidFloatNumberException("Floating-point number consists only of a plus sign.");
            }

            if (input[position] == '.' &&
                // And position is not at end of string
                position != input.Length - 1 &&
                // And next char is an ASCII digit
                ParserIdioms.ASCIIDigits.Contains(input[position + 1]))
            {
                value = 0;
                goto Fraction;
            }

            if (!ParserIdioms.ASCIIDigits.Contains(input[position]))
                throw new InvalidFloatNumberException("Floating-point number string contains a non-ASCII-digit character.");

            string digits = ParserIdioms.CollectSequenceOfCharacters(
                input,
                ref position,
                (char ch) => ParserIdioms.ASCIIDigits.Contains(ch));
            value = value * int.Parse(digits);

            if (position >= input.Length)
                goto Conversion;

            Fraction:
            if (input[position] == '.')
            {
                position++;
                if (position >= input.Length ||
                    // or the char is neither an ASCII digit, nor an e, nor an E
                    !(ParserIdioms.ASCIIDigits.Contains(input[position]) ||
                    input[position] == 'e' ||
                    input[position] == 'E'))
                    goto Conversion;

                if (input[position] != 'e' && input[position] != 'E')
                {
                    do
                    {
                        divisor = divisor * 10;
                        value = value + (int.Parse(input[position].ToString()) / divisor);
                        position++;
                        if (position >= input.Length)
                            goto Conversion;
                    }
                    while (ParserIdioms.ASCIIDigits.Contains(input[position]));
                }
            }

            if (input[position] == 'e' || input[position] == 'E')
            {
                position++;
                if (position >= input.Length)
                    goto Conversion;

                if (input[position] == '-')
                {
                    exponent = -1;
                    position++;
                    if (position >= input.Length)
                        goto Conversion;
                }
                else if (input[position] == '+')
                {
                    position++;
                    if (position >= input.Length)
                        goto Conversion;
                }

                if (!ParserIdioms.ASCIIDigits.Contains(input[position]))
                    goto Conversion;

                int digitSequence = int.Parse(
                    ParserIdioms.CollectSequenceOfCharacters(
                        input,
                        ref position,
                        (char ch) => ParserIdioms.ASCIIDigits.Contains(ch)));

                exponent = exponent * digitSequence;
                value = value * Math.Pow(10, exponent);
            }

            Conversion:
            // TODO: Steps 15-18 are not implemented, in lieu of this. 
            // (Mainly because I'm not sure where to get the entire set of 
            // IEEE 754 double-precision floating points.)
            return value;
        }
    }
}
