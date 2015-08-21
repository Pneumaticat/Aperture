using Aperture.Parser.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aperture.Parser.Common
{
    public static class DateTimeUtils
    {
        public const int ArbitraryLeapYear = 4;

        private static readonly Regex timeStringRegex = new Regex(
            @"^(?:0[0-9]|1[0-9]|2[0-3]):(?:[0-5][0-9])(?::[0-5][0-9](?:\.[0-9][0-9]?[0-9]?)?)?$");

        public static int DaysInMonth(int month, int year)
        {
            // 31 if month is 1, 3, 5, 7, 8, 10, or 12; 30 if month is 4, 6, 
            // 9, or 11; 29 if month is 2 and year is a number divisible by 
            // 400, or if year is a number divisible by 4 but not by 100; and 
            // 28 otherwise.
            switch (month)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    return 31;
                case 4:
                case 6:
                case 9:
                case 11:
                    return 30;
                case 2:
                    if (year % 400 == 0 ||
                        (year % 4 == 0 && year % 100 != 0))
                        return 29;
                    else
                        return 28;
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(month), "Month must be between 1 and 12.");
            }
        }

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
            string yearChars = StringUtils.CollectSequenceOfCharacters(
                input,
                ref position,
                ch => StringUtils.ASCIIDigits.Contains(ch));
            
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
            string monthChars = StringUtils.CollectSequenceOfCharacters(
                input, 
                ref position,
                ch => StringUtils.ASCIIDigits.Contains(ch));
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
            YearAndMonth? yam = ParseMonthComponent(input, ref position);
            if (yam == null)
                return null;

            int maxday = DaysInMonth(yam.Value.Month, yam.Value.Year);
            if (position >= input.Length || input[position] != '-')
                return null;
            else
                position++;

            int day;
            string dayChars = StringUtils.CollectSequenceOfCharacters(input, ref position,
                ch => StringUtils.ASCIIDigits.Contains(ch));

            if (dayChars.Length != 2)
                return null;
            else
                day = int.Parse(dayChars);

            if (day < 1 || day > maxday)
                return null;
            else
                return new Date(yam.Value.Year, yam.Value.Month, day);
        }

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
            string hyphens = StringUtils.CollectSequenceOfCharacters(
                input,
                ref position,
                ch => ch == '-');
            if (hyphens.Length != 0 && hyphens.Length != 2)
                return null;

            int month;
            string monthChars = StringUtils.CollectSequenceOfCharacters(
                input,
                ref position,
                ch => StringUtils.ASCIIDigits.Contains(ch));

            if (monthChars.Length != 2)
                return null;
            else
                month = int.Parse(monthChars);

            if (month < 1 || month > 12)
                return null;

            int maxday = DaysInMonth(month, ArbitraryLeapYear);

            if (position >= input.Length || input[position] != '-')
                return null;
            else
                position++;

            int day;
            string dayChars = StringUtils.CollectSequenceOfCharacters(
                input,
                ref position,
                ch => StringUtils.ASCIIDigits.Contains(ch));
            if (dayChars.Length != 2)
                return null;
            else
                day = int.Parse(dayChars);

            if (day < 1 || day > maxday)
                return null;
            else
                return new MonthAndDay(month, day);
        }

        /// <summary>
        /// Uses a regex to determine whether or not a time string is valid.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsValidTimeString(string input)
        {
            // Uses a regex to simulate what the spec says - should be fine.
            return timeStringRegex.IsMatch(input);
        }

        public static Time? ParseTimeString(string input)
        {
            int position = 0;
            Time? time = ParseTimeComponent(input, ref position);
            if (time == null || position < input.Length)
                return null;
            else
                return time;
        }

        public static Time? ParseTimeComponent(string input, ref int position)
        {
            int hour;
            string hourChars = StringUtils.CollectSequenceOfCharacters(
                input,
                ref position,
                ch => StringUtils.ASCIIDigits.Contains(ch));
            if (hourChars.Length != 2)
                return null;
            else
                hour = int.Parse(hourChars);

            if (hour < 0 || hour > 23)
                return null;

            if (position >= input.Length || input[position] != ':')
                return null;
            else
                position++;

            int minute;
            string minuteChars = StringUtils.CollectSequenceOfCharacters(
                input,
                ref position,
                ch => StringUtils.ASCIIDigits.Contains(ch));
            if (minuteChars.Length != 2)
                return null;
            else
                minute = int.Parse(minuteChars);

            if (minute < 0 || minute > 59)
                return null;

            string second = "0";
            
            if (position < input.Length && input[position] == ':')
            {
                position++;
                // If position is past the end of input or is at the last 
                // character of input, or either of the next two characters 
                // are not ASCII digits, fail.
                if (position >= input.Length ||
                    position == input.Length - 1 ||
                    (!StringUtils.ASCIIDigits.Contains(input[position]) ||
                     !StringUtils.ASCIIDigits.Contains(input[position + 1])))
                    return null;

                string maybeSecondChars = StringUtils.CollectSequenceOfCharacters(
                    input,
                    ref position,
                    ch => StringUtils.ASCIIDigits.Contains(ch) || ch == '.');

                // If the collected sequence is three characters long, or if 
                // it is longer than three characters long and the third 
                // character is not a period, or if it has more than one 
                // period, then fail.
                if (maybeSecondChars.Length == 3 ||
                    (maybeSecondChars.Length > 3 && maybeSecondChars[2] != '.') ||
                    maybeSecondChars.Count(c => c == '.') > 1)
                    return null;
                else
                    second = maybeSecondChars;
            }

            // TODO: Should this be a double? The HTML spec says that it...
            // should be a base-10 integer, potentially with a fractional 
            // part. ...So, a a floating-point? Also, should the default 
            // double.Parse be used, or the homemade 
            // NumberUtils.ParseFloatingPointNumber? So many questions.
            double secondNum = NumberUtils.ParseFloatingPointNumber(second);
            if (secondNum < 0 || secondNum > 59)
                return null;
            else
                return new Time(hour, minute, secondNum);
        }
    }
}
