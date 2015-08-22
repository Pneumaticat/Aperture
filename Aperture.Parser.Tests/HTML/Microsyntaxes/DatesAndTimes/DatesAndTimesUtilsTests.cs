using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aperture.Parser.HTML.Microsyntaxes.DatesAndTimes;

namespace Aperture.Parser.Tests.HTML.Microsyntaxes.DatesAndTimes
{
    [TestClass]
    public class DatesAndTimesUtilsTests
    {
        [TestMethod]
        public void TestLeapYearHandling()
        {
            // Leap years occur every 4 years...
            Assert.AreEqual(29, DatesAndTimesUtils.DaysInMonth(2, 2004));
            // Except every century, when it is skipped...
            Assert.AreEqual(28, DatesAndTimesUtils.DaysInMonth(2, 2100));
            // Unless the century is divisible by 4, when it isn't.
            Assert.AreEqual(29, DatesAndTimesUtils.DaysInMonth(2, 2000));
            Assert.AreEqual(29, DatesAndTimesUtils.DaysInMonth(2, 1600));
        }
    }
}
