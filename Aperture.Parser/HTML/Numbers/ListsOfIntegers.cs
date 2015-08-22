using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aperture.Parser.HTML.Numbers
{
    public static class ListsOfIntegers
    {
        // 2.4.4.5 Lists of integers
        // https://html.spec.whatwg.org/multipage/infrastructure.html#lists-of-integers
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
            else if (ParserIdioms.ASCIIDigits.Contains(input[position]))
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
    }
}
