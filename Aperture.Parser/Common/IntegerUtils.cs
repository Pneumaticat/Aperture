using Aperture.Parser.Exceptions;
using Aperture.Parser.Helpers;
using Aperture.Parser.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aperture.Parser.Common
{
    public static class NumberUtils
    {
        // 2.4.4.1 Signed integers
        public static int ParseInteger(string input)
        {
            int position = 0;
            string sign = "positive";

            StringUtils.SkipWhitespace(input, ref position);

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

            if (!StringUtils.ASCIIDigits.Contains(input[position]))
                throw new InvalidIntegerException("Character in integer string is not an ASCII digit.");

            string digits = StringUtils.CollectSequenceOfCharacters(
                input, 
                ref position, 
                (char ch) => StringUtils.ASCIIDigits.Contains(ch));

            int value = int.Parse(digits);
            if (sign == "positive")
                return value;
            else
                return 0 - value;
        }

        // 2.4.4.2 Non-negative integers
        // TODO: Add "is non negative integer" method?
        public static int ParseNonNegativeInteger(string input)
        {
            int value = ParseInteger(input);
            if (value < 0)
                // TODO: Maybe use another, "InvalidHtmlNonNegativeIntegerException"?
                throw new InvalidIntegerException("A non-negative integer cannot be less than zero.");
            return value;
        }

        // 2.4.4.3 Floating-point numbers

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
            decimal value = 1;
            int divisor = 1;
            int exponent = 1;

            StringUtils.SkipWhitespace(input, ref position);

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
                StringUtils.ASCIIDigits.Contains(input[position + 1]))
            {
                value = 0;
                goto Fraction;
            }

            if (!StringUtils.ASCIIDigits.Contains(input[position]))
                throw new InvalidFloatNumberException("Floating-point number string contains a non-ASCII-digit character.");

            string digits = StringUtils.CollectSequenceOfCharacters(
                input,
                ref position,
                (char ch) => StringUtils.ASCIIDigits.Contains(ch));
            value = value * int.Parse(digits);

            if (position >= input.Length)
                goto Conversion;

        Fraction:
            if (input[position] == '.')
            {
                position++;
                if (position >= input.Length ||
                    // or the char is neither an ASCII digit, nor an e, nor an E
                    !(StringUtils.ASCIIDigits.Contains(input[position]) ||
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
                    while (StringUtils.ASCIIDigits.Contains(input[position]));
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

                if (!StringUtils.ASCIIDigits.Contains(input[position]))
                    goto Conversion;

                int digitSequence = int.Parse(
                    StringUtils.CollectSequenceOfCharacters(
                        input, 
                        ref position, 
                        (char ch) => StringUtils.ASCIIDigits.Contains(ch)));

                exponent = exponent * digitSequence;
                value = value * (10 ^ exponent);
            }

        Conversion:
            // TODO: Steps 15-18 are not implemented, in lieu of this. 
            // (Mainly because I'm not sure where to get the entire set of 
            // IEEE 754 double-precision floating points.)
            return double.Parse(value.ToString());
        }

        public static Dimension ParseDimensionValue(string input)
        {
            int position = 0;

            StringUtils.SkipWhitespace(input, ref position);

            if (position >= input.Length)
                throw new InvalidDimensionValueException("Value is only whitespace.");

            if (input[position] == '+')
                position++;

            StringUtils.CollectSequenceOfCharacters(input, ref position, (char ch) => ch == '0');

            if (position >= input.Length)
                throw new InvalidDimensionValueException("Invalid dimension value.");

            if ("123456789".ToCharArray().Contains(input[position]) == false)
                throw new InvalidDimensionValueException("Value contains a character that is not 1-9.");

            int value = int.Parse(StringUtils.CollectSequenceOfCharacters(
                input,
                ref position,
                (char ch) => StringUtils.ASCIIDigits.Contains(ch)));

            if (position >= input.Length)
                return new Dimension
                {
                    type = DimensionType.Length,
                    value = value
                };

            if (input[position] == '.')
            {
                position++;
                if (position >= input.Length ||
                    !StringUtils.ASCIIDigits.Contains(input[position]))
                {
                    return new Dimension
                    {
                        type = DimensionType.Length,
                        value = value
                    };
                }

                int divisor = 1;
                do
                {
                    divisor = divisor * 10;
                    value = value + (int.Parse(input[position].ToString()) / divisor);

                    position++;
                    if (position >= input.Length)
                        return new Dimension
                        {
                            type = DimensionType.Length,
                            value = value
                        };
                }
                while (StringUtils.ASCIIDigits.Contains(input[position]));
            }

            if (position >= input.Length)
                return new Dimension
                {
                    type = DimensionType.Length,
                    value = value
                };
            else if (input[position] == '%')
                return new Dimension
                {
                    type = DimensionType.Percentage,
                    value = value
                };
            else
                return new Dimension
                {
                    type = DimensionType.Length,
                    value = value
                };
        }
    }
}
