using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aperture.Parser.HTML.Microsyntaxes.DatesAndTimes;

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
    }
}
