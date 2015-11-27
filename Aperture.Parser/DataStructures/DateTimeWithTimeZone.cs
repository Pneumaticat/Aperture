using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aperture.Parser.DataStructures
{
    public struct DateTimeWithTimeZone
    {
        private readonly DateTime utcDateTime;
        private readonly TimeZoneOffset timeZone;

        public DateTimeWithTimeZone(DateTime utcDateTime, TimeZoneOffset timeZone)
        {
            this.utcDateTime = utcDateTime;
            this.timeZone = timeZone;
        }

        public DateTime UniversalTime { get { return utcDateTime; } }

        public TimeZoneOffset TimeZone { get { return timeZone; } }
    }
}
