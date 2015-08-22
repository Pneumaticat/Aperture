using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aperture.Parser.DataStructures
{
    public struct TimeZoneOffset
    {
        public TimeZoneOffset(int hours, int minutes)
        {
            if (hours >= -23 && hours <= 23)
                Hours = hours;
            else
                throw new ArgumentOutOfRangeException(
                    nameof(hours), "Hours must be between -23 and 23.");

            if (minutes >= -59 && minutes <= 59)
                Minutes = minutes;
            else
                throw new ArgumentOutOfRangeException(
                    nameof(minutes), "Minutes must be between -59 and 59.");
        }
        public int Hours { get; }
        public int Minutes { get; }
        public bool IsUTC
        {
            get
            {
                return Hours == 0 && Minutes == 0;
            }
        }
    }
}
