using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aperture.Parser.DataStructures
{
    public struct YearAndMonth
    {
        public YearAndMonth(int year, int month)
        {
            if (year > 0)
                _year = year;
            else
                throw new ArgumentOutOfRangeException(
                    nameof(year), "Year must be 1 or greater.");

            if (month >= 1 && month <= 12)
                _month = month;
            else
                throw new ArgumentOutOfRangeException(
                    nameof(month), "Month must be between 1 and 12.");
        }

        private int _year;
        public int Year
        {
            get { return _year; }
        }
        private int _month;
        public int Month
        {
            get { return _month; }
        }
    }
}
