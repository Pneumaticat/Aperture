using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aperture.Parser.HTML.Microsyntaxes.DatesAndTimes;

namespace Aperture.Parser.Tests.HTML.Microsyntaxes.DatesAndTimes
{
    [TestClass]
    public class GlobalDatesAndTimesTests
    {
        [TestMethod]
        public void TestIsValidGlobalDateAndTimeString()
        {
            Assert.IsTrue(GlobalDatesAndTimes.IsValidGlobalDateAndTimeString(
                "2001-12-31T23:59:59.000Z"));
            Assert.IsTrue(GlobalDatesAndTimes.IsValidGlobalDateAndTimeString(
                "2001-12-31 00:00:00.000+00:15"));
        }
    }
}
