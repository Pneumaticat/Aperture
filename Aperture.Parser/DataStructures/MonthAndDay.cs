using Aperture.Parser.HTML.Microsyntaxes.DatesAndTimes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aperture.Parser.DataStructures
{
    public struct MonthAndDay
    {
        public MonthAndDay(int month, int day)
        {
            if (month >= 1 && month <= 12)
                Month = month;
            else
                throw new ArgumentOutOfRangeException(
                    nameof(month), "Month must be between 1 and 12.");

            // We don't know the year, so just always accept leap days, 
            // using any arbitrary leap year.
            Day = DatesAndTimes.DaysInMonth(month, DatesAndTimes.ArbitraryLeapYear);
        }
        public int Month { get; }
        public int Day { get; }
    }
}
