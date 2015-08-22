using Aperture.Parser.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aperture.Parser.HTML.Microsyntaxes.DatesAndTimes
{
    public static class Months
    {

        public static bool IsValidMonthString(string input)
        {
            return ParseMonthString(input) != null;
        }

        // TODO: Should ParseMonthString return null upon failure?
        /// <summary>
        /// Parses a month string into a year and month, e.g. 2007-10. Returns 
        /// null upon failure.
        /// </summary>
        public static YearAndMonth? ParseMonthString(string input)
        {
            int position = 0;
            // hehe yam
            YearAndMonth? yam = ParseMonthComponent(input, ref position);
            // Fail if yam is null or position is _not_ beyond the end of input
            if (yam == null || position < input.Length)
                return null;
            else
                return yam;
        }

        public static YearAndMonth? ParseMonthComponent(string input, ref int position)
        {
            int year;
            string yearChars = ParserIdioms.CollectSequenceOfCharacters(
                input,
                ref position,
                ch => ParserIdioms.ASCIIDigits.Contains(ch));

            if (yearChars.Length < 4)
                return null;
            else
                year = int.Parse(yearChars);

            if (year <= 0)
                return null;

            if (position >= input.Length ||
                input[position] != '-')
                return null;
            else
                position++;

            int month;
            string monthChars = ParserIdioms.CollectSequenceOfCharacters(
                input,
                ref position,
                ch => ParserIdioms.ASCIIDigits.Contains(ch));
            if (monthChars.Length != 2)
                return null;
            else
                month = int.Parse(monthChars);

            // Must be between range 1 ≤ month ≤ 12
            if (month < 1 || month > 12)
                return null;
            else
                return new YearAndMonth(year, month);
        }
    }
}
