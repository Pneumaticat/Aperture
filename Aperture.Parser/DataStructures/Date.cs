using Aperture.Parser.Common;
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

            Day = DateTimeUtils.DaysInMonth(month, year);
        }

        public int Year { get; }
        public int Month { get; }
        public int Day { get; }
    }
}
