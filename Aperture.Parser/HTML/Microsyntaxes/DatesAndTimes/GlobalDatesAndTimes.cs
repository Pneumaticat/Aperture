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
    }
}
