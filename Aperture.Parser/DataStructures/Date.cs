using Aperture.Parser.HTML;
using Aperture.Parser.HTML.DatesAndTimes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aperture.Parser.DataStructures
{
    public struct Date
    {
        public Date(int year, int month, int day)
        {
            if (year > 0)
                Year = year;
            else
                throw new ArgumentOutOfRangeException(
                    nameof(year), "Year must be 1 or greater.");

            if (month >= 1 && month <= 12)
                Month = month;
            else
                throw new ArgumentOutOfRangeException(
                    nameof(month), "Month must be between 1 and 12.");

            if (day > 0 && day <= DateTimeUtils.DaysInMonth(month, year))
                Day = day;
            else
                throw new ArgumentOutOfRangeException(
                    nameof(day),
                    "Day must be between 1 and the max possible day for this month.");
        }

        public int Year { get; }
        public int Month { get; }
        public int Day { get; }
    }
}
