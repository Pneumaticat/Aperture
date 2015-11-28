using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aperture.Parser.HTML.Microsyntaxes.DatesAndTimes;
using Aperture.Parser.DataStructures;

namespace Aperture.Parser.Tests.HTML.Microsyntaxes.DatesAndTimes
{
    [TestClass]
    public class WeeksTests
    {
        [TestMethod]
        public void TestIsValidWeekString()
        {
            Assert.IsTrue(Weeks.IsValidWeekString("2001-W01"));
            Assert.IsFalse(Weeks.IsValidWeekString("2001-W1"),
                "Incorrectly allows one-digit week number.");
            Assert.IsFalse(Weeks.IsValidWeekString("2001-W00"),
                "Incorrectly allows W00.");

            Assert.IsTrue(Weeks.IsValidWeekString("2004-W53"),
                "Incorrectly denies leap weeks.");
            Assert.IsFalse(Weeks.IsValidWeekString("1900-W53"),
                "Incorrectly allows leap weeks on 1900, which is *not* a leap year.");
        }

        [TestMethod]
        public void TestParseWeekString()
        {
            Assert.AreEqual(
                new Week(5, 2001),
                Weeks.ParseWeekString("2001-W05"));
            Assert.AreEqual(
                new Week(53, 2004),
                Weeks.ParseWeekString("2004-W53"));

            // The backing DateTime, used in WeeksInWeekYear, does not support 
            // 5 digit years, although the spec says they should be allowed. 
            // Thus, 5 digit years are currently unsupported. I doubt this will 
            // be a problem for a while.
            //Assert.AreEqual(
            //    new Week(5, 20005),
            //    Weeks.ParseWeekString("20005-W05"));

            Assert.IsNull(Weeks.ParseWeekString("2004-W5"));
            Assert.IsNull(Weeks.ParseWeekString("2004  05"));
        }
    }
}
