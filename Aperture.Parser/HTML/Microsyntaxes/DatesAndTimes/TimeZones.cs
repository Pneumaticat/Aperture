﻿using Aperture.Parser.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aperture.Parser.HTML.Microsyntaxes.DatesAndTimes
{
    public static class TimeZones
    {
        public static bool IsValidTimeZoneOffsetString(
            string input,
            TimeZoneOffset representedOffset)
        {
            if (input == "Z" && representedOffset.IsUTC)
                return true;
            else
            {
                int position = 0;
                if (position >= input.Length)
                    return false; // Empty string

                if (input[position] == '+' || input[position] == '-')
                {
                    char sign = input[position];
                    if (representedOffset.Hours == 0 &&
                        representedOffset.Minutes == 0 &&
                        sign == '-')
                        // Cannot have a negative 00:00 offset.
                        // That would just be weird.
                        return false;

                    position++;
                    if (position >= input.Length || position + 1 >= input.Length)
                        return false; // Just a plus sign, or only 1 char after it

                    string hourChars = new string(
                        new char[] { input[position], input[position + 1] });
                    position = position + 2; // Move past hour chars

                    if (position >= input.Length)
                        return false; // Nothing after hour chars
                    else if (input[position] == ':')
                        position++;

                    if (position >= input.Length || position + 1 >= input.Length)
                        return false; // Nothing after hour chars/colon, or 
                                      // only one char after it

                    string minuteChars = new string(
                        new char[] { input[position], input[position + 1] });
                    position = position + 2; // Move past minute chars.

                    if (position < input.Length)
                        // Position should be past end of string; if it is 
                        // not, the offset string is invalid.
                        return false;

                    if (hourChars.All(ch => ParserIdioms.ASCIIDigits.Contains(ch)) &&
                        minuteChars.All(ch => ParserIdioms.ASCIIDigits.Contains(ch)))
                    {
                        // Hour chars and minute chars are all ASCII digits
                        int hours = int.Parse(hourChars);
                        int minutes = int.Parse(minuteChars);

                        if (sign == '-')
                        {
                            // Negate hours and minutes if timezone offset 
                            // sign is negative
                            hours = 0 - hours;
                            minutes = 0 - minutes;
                        }

                        return hours == representedOffset.Hours &&
                             minutes == representedOffset.Minutes;
                    }
                    else
                        return false;
                }
                else
                    // Time zone offset did not begin with a + or -
                    return false;
            }
        }

        public static TimeZoneOffset? ParseTimeZoneOffsetString(string input)
        {
            int position = 0;
            TimeZoneOffset? tz = ParseTimeZoneOffsetComponent(input, ref position);
            if (position < input.Length || tz == null)
                return null;
            else
                return tz;
        }

        public static TimeZoneOffset? ParseTimeZoneOffsetComponent(string input, ref int position)
        {
            int hours;
            int minutes;

            if (input[position] == 'Z')
            {
                hours = 0;
                minutes = 0;
                position++;
            }
            else if (input[position] == '+' || input[position] == '-')
            {
                string sign;
                if (input[position] == '+')
                    sign = "positive";
                else
                    sign = "negative";

                position++;

                string s = ParserIdioms.CollectSequenceOfCharacters(
                    input, ref position,
                    ch => ParserIdioms.ASCIIDigits.Contains(ch));

                if (s.Length == 2)
                {
                    hours = int.Parse(s);

                    if (position >= input.Length || input[position] != ':')
                        return null;
                    else
                        position++;

                    string minutesChars = ParserIdioms.CollectSequenceOfCharacters(
                        input, ref position,
                        ch => ParserIdioms.ASCIIDigits.Contains(ch));
                    if (minutesChars.Length != 2)
                        return null;
                    else
                        minutes = int.Parse(minutesChars);
                }
                else if (s.Length == 4)
                {
                    hours = int.Parse(s.Substring(0, 2));
                    minutes = int.Parse(s.Substring(2, 2));
                }
                else
                    return null;

                if (hours < 0 || hours > 23)
                    return null;

                if (sign == "negative")
                    hours = 0 - hours;

                if (minutes < 0 || minutes > 59)
                    return null;

                if (sign == "negative")
                    minutes = 0 - minutes;
            }
            else
                return null;

            return new TimeZoneOffset(hours, minutes);
        }
    }
}
