using Aperture.Parser.Exceptions;
using Aperture.Parser.DataStructures;
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
            double value = 1;
            double divisor = 1;
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
                value = value * Math.Pow(10, exponent);
            }

        Conversion:
            // TODO: Steps 15-18 are not implemented, in lieu of this. 
            // (Mainly because I'm not sure where to get the entire set of 
            // IEEE 754 double-precision floating points.)
            return value;
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
                return new Dimension(DimensionType.Length, value);

            if (input[position] == '.')
            {
                position++;
                if (position >= input.Length ||
                    !StringUtils.ASCIIDigits.Contains(input[position]))
                {
                    return new Dimension(DimensionType.Length, value);
                }

                int divisor = 1;
                do
                {
                    divisor = divisor * 10;
                    value = value + (int.Parse(input[position].ToString()) / divisor);

                    position++;
                    if (position >= input.Length)
                        return new Dimension(DimensionType.Length, value);
                }
                while (StringUtils.ASCIIDigits.Contains(input[position]));
            }

            if (position >= input.Length)
                return new Dimension(DimensionType.Length, value);
            else if (input[position] == '%')
                return new Dimension(DimensionType.Percentage, value);
            else
                return new Dimension(DimensionType.Length, value);
        }

        public static List<int> ParseListOfIntegers(string input)
        {
            int position = 0;
            List<int> numbers = new List<int>();

            // If position is in the string and it is a space, comma, 
            // or semicolon...
        CheckDelimiters:
            if (position < input.Length &&
                (input[position] == ' ' || 
                input[position] == ',' || 
                input[position] == ';'))
                position++;

            if (position >= input.Length)
                return numbers; // abort!

            if (input[position] == ' ' ||
                input[position] == ',' ||
                input[position] == ';')
                goto CheckDelimiters;

            bool negated = false;
            int value = 0;
            // Set to true when number or - is found
            bool started = false;
            // Set to true when parser sees number
            bool gotNumber = false;
            // Set to true to ignore characters until the next separator.
            bool finished = false;
            bool bogus = false;

        Parser:
            if (input[position] == '-')
            {
                if (gotNumber)
                    finished = true;

                if (finished)
                    goto EndParserLoop;

                if (started)
                    negated = false;
                else if (!started && !bogus)
                    negated = true;

                started = true;
            }
            else if (StringUtils.ASCIIDigits.Contains(input[position]))
            {
                if (finished)
                    goto EndParserLoop;

                value = value * 10;
                value = value + int.Parse(input[position].ToString());

                started = true;
                gotNumber = true;
            }
            else if (input[position] == ' ' ||
                        input[position] == ',' ||
                        input[position] == ';')
            {
                if (!gotNumber)
                    // This happens if an entry in the list has no digits, as 
                    // in "1,2,x,4".
                    return numbers;

                if (negated)
                    // TODO: This might not be what they mean by "negate value"?
                    value = 0 - value;

                numbers.Add(value);
                goto CheckDelimiters;
            }       // Also known as any non-alphabetic ASCII character.
            else if (('\u0001' <= input[position] && input[position] <= '\u001F') ||
                        ('\u0021' <= input[position] && input[position] <= '\u002B') ||
                        ('\u002D' <= input[position] && input[position] <= '\u002F') ||
                        input[position] == '\u003A' ||
                        ('\u003C' <= input[position] && input[position] <= '\u0040') ||
                        ('\u005B' <= input[position] && input[position] <= '\u0060') ||
                        ('\u007B' <= input[position] && input[position] <= '\u007F'))
            {
                if (gotNumber)
                    finished = true;
                if (finished)
                    goto EndParserLoop;
                negated = false;
            }
            else
            {
                if (finished)
                    goto EndParserLoop;
                negated = false;
                bogus = true;
                if (started)
                    return numbers;
            }

        EndParserLoop:
            position++;
            if (position < input.Length)
                goto Parser;
            if (negated)
                // TODO: May not be what they mean by "negate the value"
                value = 0 - value;
            if (gotNumber)
                numbers.Add(value);

            return numbers;
        }

        public static List<DimensionListPair> ParseListOfDimensions(string rawInput)
        {
            // Remove optional last comma
            if (rawInput.Last() == ',')
                rawInput.Remove(rawInput.Length - 2, 1);

            string[] rawTokens = StringUtils.SplitStringOnCommas(rawInput);

            List<DimensionListPair> result = new List<DimensionListPair>();

            foreach (string input in rawTokens)
            {
                int position = 0;
                double value = 0;
                DimensionListPairUnit unit = DimensionListPairUnit.Absolute;

                if (position >= input.Length)
                {
                    unit = DimensionListPairUnit.Relative;
                    goto AddToResults;
                }
                else if (StringUtils.ASCIIDigits.Contains(input[position]))
                {
                    value = value +
                        (int.Parse(
                            StringUtils.CollectSequenceOfCharacters(
                                input,
                                ref position,
                                (char ch) =>
                                 StringUtils.ASCIIDigits.Contains(ch))));
                }

                if (position < input.Length && input[position] == '.')
                {
                    position++; // Move past the .
                                // Not said in the spec, but perhaps implied?

                    // Collect a sequence of characters consisting of 
                    // space characters and ASCII digits.
                    string s = StringUtils.CollectSequenceOfCharacters(
                        input, ref position,
                        (char ch) => 
                            StringUtils.ASCIIDigits.Contains(ch) ||
                            StringUtils.SpaceCharacters.Contains(ch));

                    // Remove space chars from s
                    foreach (char ch in StringUtils.SpaceCharacters)
                        s = s.Replace(ch.ToString(), "");

                    if (s != string.Empty)
                    {
                        int length = s.Length;
                        // TODO: Correct type?
                        double fraction = int.Parse(s) / (Math.Pow(10, length));
                        value += fraction;
                    }
                }

                StringUtils.SkipWhitespace(input, ref position);

                // Check if position is past end of string, which isn't 
                // said in the spec, but I think otherwise there are some 
                // situations where it will throw an exception because 
                // position is past the end of the string.
                if (position < input.Length)
                {
                    if (input[position] == '%')
                        unit = DimensionListPairUnit.Percentage;
                    if (input[position] == '*')
                        unit = DimensionListPairUnit.Relative;
                }
            AddToResults:
                result.Add(new DimensionListPair(value, unit));
            }

            return result;
        }
    }
}
