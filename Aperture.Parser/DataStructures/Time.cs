using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aperture.Parser.DataStructures
{
    public struct Time
    {
        public Time(int hour, int minute, double second)
        {
            if (hour < 0 || hour > 23)
                throw new ArgumentOutOfRangeException(
                    nameof(hour), "Hour must be between 0 and 23.");
            else
                Hour = hour;

            if (minute < 0 || minute > 59)
                throw new ArgumentOutOfRangeException(
                    nameof(minute), "Minute must be between 0 and 59.");
            else
                Minute = minute;

            if (second < 0 || second > 59)
                throw new ArgumentOutOfRangeException(
                    nameof(second), "Second must be between 0 and 59.");
            else
                Second = second;
        }
        public int Hour  { get; }
        public int Minute { get; }
        public double Second { get; }
    }
}
