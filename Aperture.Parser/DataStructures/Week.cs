using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aperture.Parser.DataStructures
{
    public struct Week
    {
        public Week(int weekNumber, int weekYear)
        {
            WeekNumber = weekNumber;
            WeekYear = weekYear;
        }
        public int WeekNumber { get; }
        public int WeekYear { get; }
    }
}
