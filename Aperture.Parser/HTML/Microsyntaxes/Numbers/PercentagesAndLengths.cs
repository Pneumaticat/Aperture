using Aperture.Parser.DataStructures;
using Aperture.Parser.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aperture.Parser.HTML.Microsyntaxes.Numbers
{
    public static class PercentagesAndLengths
    {
        // 2.4.4.4 Percentages and lengths
        // https://html.spec.whatwg.org/multipage/infrastructure.html#percentages-and-dimensions
        public static Dimension ParseDimensionValue(string input)
        {
            int position = 0;

            ParserIdioms.SkipWhitespace(input, ref position);

            if (position >= input.Length)
                throw new InvalidDimensionValueException("Value is only whitespace.");

            if (input[position] == '+')
                position++;

            ParserIdioms.CollectSequenceOfCharacters(input, ref position, (char ch) => ch == '0');

            if (position >= input.Length)
                throw new InvalidDimensionValueException("Invalid dimension value.");

            if ("123456789".ToCharArray().Contains(input[position]) == false)
                throw new InvalidDimensionValueException("Value contains a character that is not 1-9.");

            int value = int.Parse(ParserIdioms.CollectSequenceOfCharacters(
                input,
                ref position,
                (char ch) => ParserIdioms.ASCIIDigits.Contains(ch)));

            if (position >= input.Length)
                return new Dimension(DimensionType.Length, value);

            if (input[position] == '.')
            {
                position++;
                if (position >= input.Length ||
                    !ParserIdioms.ASCIIDigits.Contains(input[position]))
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
                while (ParserIdioms.ASCIIDigits.Contains(input[position]));
            }

            if (position >= input.Length)
                return new Dimension(DimensionType.Length, value);
            else if (input[position] == '%')
                return new Dimension(DimensionType.Percentage, value);
            else
                return new Dimension(DimensionType.Length, value);
        }
    }
}
