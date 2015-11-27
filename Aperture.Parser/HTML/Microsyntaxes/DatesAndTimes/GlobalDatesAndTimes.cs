using Aperture.Parser.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aperture.Parser.HTML.Microsyntaxes.DatesAndTimes
{
    public static class GlobalDatesAndTimes
    {
        public static bool IsValidGlobalDateAndTimeString(string input)
        {
            int position = 0;
            if (Dates.ParseDateComponent(input, ref position) == null)
                return false;

            if (position >= input.Length ||
                (input[position] != 'T' && input[position] != ' '))
                return false;
            else
                position++;

            if (Times.ParseTimeComponent(input, ref position) == null)
                return false;

            if (TimeZones.ParseTimeZoneOffsetComponent(input, ref position) == null)
                return false;

            return true;
        }

        public static DateTimeWithTimeZone? ParseGlobalDateAndTimeString(string input)
        {
            int position = 0;

            Date? date = Dates.ParseDateComponent(input, ref position);
            if (date == null)
                return null;

            if (position >= input.Length ||
                (input[position] != 'T' && input[position] != ' '))
                return null;
            else
                position++;

            Time? time = Times.ParseTimeComponent(input, ref position);
            if (time == null)
                return null;

            TimeZoneOffset? offset =
                TimeZones.ParseTimeZoneOffsetComponent(input, ref position);
            if (offset == null)
                return null;

            if (position < input.Length)
                return null;

            DateTime localDateTime = new DateTime(
                date.Value.Year,
                date.Value.Month,
                date.Value.Day,
                time.Value.Hour,
                time.Value.Minute,
                // Truncates and drops decimals
                (int)time.Value.Second,
                // Complex code because doubles to int is hard.
                (int)Math.Round((time.Value.Second - Math.Truncate(time.Value.Second)) * 1000));

            DateTime utcDateTime =
                localDateTime - new TimeSpan(
                    offset.Value.Hours,
                    offset.Value.Minutes, 0);

            return new DateTimeWithTimeZone(utcDateTime, offset.Value);
        }
    }
}
