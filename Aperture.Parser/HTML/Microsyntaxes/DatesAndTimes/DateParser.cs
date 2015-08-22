using Aperture.Parser.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aperture.Parser.HTML.Microsyntaxes.DatesAndTimes
{
    public static class DateParser
    {
        public static bool IsValidDateString(string input)
        {
            return ParseDateString(input) != null;
        }

        public static Date? ParseDateString(string input)
        {
            int position = 0;
            Date? date = ParseDateComponent(input, ref position);
            if (date == null || position < input.Length)
                return null;
            else
                return date;
        }

        public static Date? ParseDateComponent(string input, ref int position)
        {
            YearAndMonth? yam = MonthParser.ParseMonthComponent(input, ref position);
            if (yam == null)
                return null;

            int maxday = DateTimeUtils.DaysInMonth(yam.Value.Month, yam.Value.Year);
            if (position >= input.Length || input[position] != '-')
                return null;
            else
                position++;

            int day;
            string dayChars = ParserIdioms.CollectSequenceOfCharacters(input, ref position,
                ch => ParserIdioms.ASCIIDigits.Contains(ch));

            if (dayChars.Length != 2)
                return null;
            else
                day = int.Parse(dayChars);

            if (day < 1 || day > maxday)
                return null;
            else
                return new Date(yam.Value.Year, yam.Value.Month, day);
        }
    }
}
