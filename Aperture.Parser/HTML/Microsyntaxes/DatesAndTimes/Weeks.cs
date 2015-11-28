using Aperture.Parser.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aperture.Parser.HTML.Microsyntaxes.DatesAndTimes
{
    public static class Weeks
    {
        private static int WeeksInWeekYear(int weekYear)
        {
            DateTime firstDayOfYear = new DateTime(weekYear, 1, 1);
            // A week-year has 53 weeks if it begins on a Thursday, or if it 
            // begins on a Wednesday and is a leap year. Otherwise, it has 52 
            // weeks.
            if (firstDayOfYear.DayOfWeek == DayOfWeek.Thursday ||
                (firstDayOfYear.DayOfWeek == DayOfWeek.Wednesday &&
                (weekYear % 400 == 0 || (weekYear % 4 == 0 && weekYear % 100 != 0))))
                return 53;
            else
                return 52;
        }

        public static bool IsValidWeekString(string input)
        {
            int position = 0;
            string weekYearStr = ParserIdioms.CollectSequenceOfCharacters(
                input,
                ref position,
                ch => ParserIdioms.ASCIIDigits.Contains(ch));

            if (weekYearStr.Length < 4)
                return false;
            else
            {
                if (position < input.Length && input[position] == '-')
                    position++;
                else
                    return false;

                if (position < input.Length && input[position] == 'W')
                    position++;
                else
                    return false;

                string weekNumStr = ParserIdioms.CollectSequenceOfCharacters(
                    input,
                    ref position,
                    ch => ParserIdioms.ASCIIDigits.Contains(ch));
                int weekNum = int.Parse(weekNumStr);

                if (weekNumStr.Length != 2 ||
                    weekNum < 1 ||
                    weekNum > WeeksInWeekYear(int.Parse(weekYearStr)))
                    return false;
                else
                    return true;
            }
        }

        public static Week? ParseWeekString(string input)
        {
            int position = 0;

            string yearStr = ParserIdioms.CollectSequenceOfCharacters(
                input,
                ref position,
                ch => ParserIdioms.ASCIIDigits.Contains(ch));
            if (yearStr.Length < 4)
                return null;
            int year = int.Parse(yearStr);

            if (year < 1)
                return null;

            if (position >= input.Length || input[position] != '-')
                return null;
            else
                position++;

            if (position >= input.Length || input[position] != 'W')
                return null;
            else
                position++;

            string weekStr = ParserIdioms.CollectSequenceOfCharacters(
                input,
                ref position,
                ch => ParserIdioms.ASCIIDigits.Contains(ch));
            if (weekStr.Length != 2)
                return null;
            int week = int.Parse(weekStr);

            int maxweek = WeeksInWeekYear(year);

            if (week < 1 || week > maxweek)
                return null;

            if (position < input.Length)
                return null;

            return new Week(week, year);
        }
    }
}
