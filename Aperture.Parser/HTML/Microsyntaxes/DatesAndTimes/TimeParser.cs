using Aperture.Parser.DataStructures;
using Aperture.Parser.HTML.Microsyntaxes.Numbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aperture.Parser.HTML.Microsyntaxes.DatesAndTimes
{
    public static class TimeParser
    {
        // HTML spec 2.4.5.4 Times

        /// <summary>
        /// Group 1: Hours
        /// Group 2: Minutes
        /// Group 3: Seconds
        /// Group 4: Fractional part of seconds (may be empty, in which it is 
        /// .000)
        /// </summary>
        public static readonly Regex TimeStringRegex = new Regex(
            @"^(0[0-9]|1[0-9]|2[0-3]):([0-5][0-9])(?::([0-5][0-9])(?:\.([0-9][0-9]?[0-9]?))?)?$");

        /// <summary>
        /// Uses a regex to determine whether or not a time string is valid.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsValidTimeString(string input)
        {
            // Uses a regex to simulate what the spec says - should be fine.
            return TimeStringRegex.IsMatch(input);
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
            string hourChars = ParserIdioms.CollectSequenceOfCharacters(
                input,
                ref position,
                ch => ParserIdioms.ASCIIDigits.Contains(ch));
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
            string minuteChars = ParserIdioms.CollectSequenceOfCharacters(
                input,
                ref position,
                ch => ParserIdioms.ASCIIDigits.Contains(ch));
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
                    (!ParserIdioms.ASCIIDigits.Contains(input[position]) ||
                     !ParserIdioms.ASCIIDigits.Contains(input[position + 1])))
                    return null;

                string maybeSecondChars = ParserIdioms.CollectSequenceOfCharacters(
                    input,
                    ref position,
                    ch => ParserIdioms.ASCIIDigits.Contains(ch) || ch == '.');

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
            double secondNum = FloatingPointNumbers.ParseFloatingPointNumber(second);
            if (secondNum < 0 || secondNum > 59)
                return null;
            else
                return new Time(hour, minute, secondNum);
        }
    }
}
