using Aperture.Parser.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aperture.Parser.HTML.Microsyntaxes.DatesAndTimes
{
    public static class DatesAndTimes
    {
        public const int ArbitraryLeapYear = 4;

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
    }
}
