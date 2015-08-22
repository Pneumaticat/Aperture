using Aperture.Parser.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aperture.Parser.HTML.Microsyntaxes.Numbers
{
    public static class ListsOfDimensions
    {
        // 2.4.4.6 Lists of dimensions
        // https://html.spec.whatwg.org/multipage/infrastructure.html#lists-of-dimensions
        public static List<DimensionListPair> ParseListOfDimensions(string rawInput)
        {
            // Remove optional last comma
            if (rawInput.Last() == ',')
                rawInput.Remove(rawInput.Length - 2, 1);

            string[] rawTokens = CommaSeparatedTokens.SplitStringOnCommas(rawInput);

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
                else if (ParserIdioms.ASCIIDigits.Contains(input[position]))
                {
                    value = value +
                        (int.Parse(
                            ParserIdioms.CollectSequenceOfCharacters(
                                input,
                                ref position,
                                (char ch) =>
                                 ParserIdioms.ASCIIDigits.Contains(ch))));
                }

                if (position < input.Length && input[position] == '.')
                {
                    position++; // Move past the .
                                // Not said in the spec, but perhaps implied?

                    // Collect a sequence of characters consisting of 
                    // space characters and ASCII digits.
                    string s = ParserIdioms.CollectSequenceOfCharacters(
                        input, ref position,
                        (char ch) =>
                            ParserIdioms.ASCIIDigits.Contains(ch) ||
                            ParserIdioms.SpaceCharacters.Contains(ch));

                    // Remove space chars from s
                    foreach (char ch in ParserIdioms.SpaceCharacters)
                        s = s.Replace(ch.ToString(), "");

                    if (s != string.Empty)
                    {
                        int length = s.Length;
                        // TODO: Correct type?
                        double fraction = int.Parse(s) / (Math.Pow(10, length));
                        value += fraction;
                    }
                }

                ParserIdioms.SkipWhitespace(input, ref position);

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
