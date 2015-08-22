using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aperture.Parser.DataStructures;
using Aperture.Parser.HTML.Microsyntaxes.DatesAndTimes;

namespace Aperture.Parser.Tests.HTML.Microsyntaxes.DatesAndTimes
{
    [TestClass]
    public class TimesTests
    {
        [TestMethod]
        public void TestIsValidTimeString()
        {
            Assert.IsFalse(
                Times.IsValidTimeString("23:59:00.0151"),
                "Too precise: seconds in time strings can only have " +
                "3 digits after the decimal point.");
            Assert.IsFalse(
                Times.IsValidTimeString(" 07: 55: 07 .886"),
                "Incorrectly allows whitespace.");
            Assert.IsTrue(Times.IsValidTimeString("23:59:59.01"),
                "Incorrectly detects even a standard time string as invalid.");
            Assert.IsTrue(Times.IsValidTimeString("00:00:00"));
            Assert.IsFalse(Times.IsValidTimeString("       "));
        }

        [TestMethod]
        public void TestTimeStringParsing()
        {
            Assert.AreEqual(
                new Time(23, 59, 59),
                Times.ParseTimeString("23:59:59"));
            Assert.AreEqual(
                new Time(23, 59, 0),
                Times.ParseTimeString("23:59"),
                "Does not handle time string without seconds.");
            Assert.AreEqual(
                new Time(23, 59, 0.015d),
                Times.ParseTimeString("23:59:00.015"),
                "Does not handle fractional seconds.");
            Assert.IsNull(Times.ParseTimeString("23:59:0.015"),
                "Incorrectly handles a non-0-padded fractional second.");
            Assert.IsNull(Times.ParseTimeString(" "));
            Assert.IsNull(Times.ParseTimeString("24-60-60"));
        }
    }
}
