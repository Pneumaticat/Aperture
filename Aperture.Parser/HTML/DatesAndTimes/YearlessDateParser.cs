using Aperture.Parser.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aperture.Parser.HTML.DatesAndTimes
{
    public static class YearlessDateParser
    {
        public static MonthAndDay? ParseYearlessDateString(string input)
        {
            int position = 0;
            // these variable names are awesome
            MonthAndDay? mad = ParseYearlessDateComponent(input, ref position);
            if (mad == null || position < input.Length)
                return null;
            else
                return mad;
        }

        public static MonthAndDay? ParseYearlessDateComponent(string input, ref int position)
        {
            string hyphens = ParserIdioms.CollectSequenceOfCharacters(
                input,
                ref position,
                ch => ch == '-');
            if (hyphens.Length != 0 && hyphens.Length != 2)
                return null;

            int month;
            string monthChars = ParserIdioms.CollectSequenceOfCharacters(
                input,
                ref position,
                ch => ParserIdioms.ASCIIDigits.Contains(ch));

            if (monthChars.Length != 2)
                return null;
            else
                month = int.Parse(monthChars);

            if (month < 1 || month > 12)
                return null;

            int maxday = DateTimeUtils.DaysInMonth(month, DateTimeUtils.ArbitraryLeapYear);

            if (position >= input.Length || input[position] != '-')
                return null;
            else
                position++;

            int day;
            string dayChars = ParserIdioms.CollectSequenceOfCharacters(
                input,
                ref position,
                ch => ParserIdioms.ASCIIDigits.Contains(ch));
            if (dayChars.Length != 2)
                return null;
            else
                day = int.Parse(dayChars);

            if (day < 1 || day > maxday)
                return null;
            else
                return new MonthAndDay(month, day);
        }
    }
}
