using Aperture.Parser.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aperture.Parser.HTML.Microsyntaxes.DatesAndTimes
{
    public static class LocalDateAndTimeParser
    {
        public static bool IsValidLocalDateAndTimeString(string input)
        {
            string[] items = input.Split('T', ' ');
            // If first half of string split by T/space is valid date string 
            // and second half is time string, it is valid.
            // Not really what they meant, I don't think, with the splitting,
            // but it works!
            return items.Length == 2 &&
                   DateParser.IsValidDateString(items[0]) &&
                   TimeParser.IsValidTimeString(items[1]);
        }

        public static bool IsValidNormalisedLocalDateAndTimeString(string input)
        {
            // Requirements:
            // 1. A valid date string representing the date
            // A T separator (only T)
            // A valid time string representing the time, as short as possible
            // (eg. if seconds is :00, do not specify seconds explicitly).
            string[] items = input.Split('T');
            if (items.Length == 2)
            {
                bool validDate = DateParser.IsValidDateString(items[0]);
                // If second half of input is valid time string...
                if (TimeParser.TimeStringRegex.IsMatch(items[1]))
                {
                    // Group 3 of regex: optional seconds
                    // Group 4: optional fractional part of seconds
                    GroupCollection groups = TimeParser.TimeStringRegex.Match(items[1]).Groups;

                    if (groups[3].Value == "00")
                        // Seconds is just 0, but is explicitly written out as
                        // :00, which is not as short as it can be.
                        return false;
                    else if (groups[4].Value != string.Empty &&
                             groups[4].Value.TrimEnd('0') != groups[4].Value)
                        // Fractional second is specified and there are 0s you 
                        // can trim off the end. Not as short as it could beee.
                        return false;
                    else if (validDate)
                        // Time string is as short as possible, date must also 
                        // be valid for it to return true.
                        return true;
                    else
                        // Date is not valid, return false.
                        return false;
                }
                else
                {
                    // Time regex did not match the second half of the string, 
                    // not a valid string.
                    return false;
                }
            }
            else
            {
                // Could not split the string by a 'T' and get 2 substrings, 
                // invalid string.
                return false;
            }
        }

        public static DateTime? ParseLocalDateAndTimeString(string input)
        {
            int position = 0;

            Date? date = DateParser.ParseDateComponent(input, ref position);
            if (date == null)
                return null;

            if (position >= input.Length ||
                (input[position] != 'T' && input[position] != ' '))
                return null;
            else
                position++;

            Time? time = TimeParser.ParseTimeComponent(input, ref position);
            if (time == null || position < input.Length)
                return null;
            else
                return new DateTime(
                    date.Value.Year,
                    date.Value.Month,
                    date.Value.Day,

                    time.Value.Hour,
                    time.Value.Minute,
                    // Truncates and drops decimals
                    (int)time.Value.Second,
                    (int)(time.Value.Second - Math.Truncate(time.Value.Second)),
                    DateTimeKind.Local);
            // Maybe shouldn't specify local here? I mean, we are 
            // parsing a _local_ string, but still...
        }
    }
}
