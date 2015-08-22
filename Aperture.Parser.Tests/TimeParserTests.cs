using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aperture.Parser.Common;
using Aperture.Parser.DataStructures;

namespace Aperture.Parser.Tests
{
    [TestClass]
    public class TimeParserTests
    {
        [TestMethod]
        public void TestIsValidTimeString()
        {
            Assert.IsFalse(
                TimeParser.IsValidTimeString("23:59:00.0151"),
                "Too precise: seconds in time strings can only have " +
                "3 digits after the decimal point.");
            Assert.IsFalse(
                TimeParser.IsValidTimeString(" 07: 55: 07 .886"),
                "Incorrectly allows whitespace.");
            Assert.IsTrue(TimeParser.IsValidTimeString("23:59:59.01"),
                "Incorrectly detects even a standard time string as invalid.");
            Assert.IsTrue(TimeParser.IsValidTimeString("00:00:00"));
            Assert.IsFalse(TimeParser.IsValidTimeString("       "));
        }

        [TestMethod]
        public void TestTimeStringParsing()
        {
            Assert.AreEqual(
                new Time(23, 59, 59),
                TimeParser.ParseTimeString("23:59:59"));
            Assert.AreEqual(
                new Time(23, 59, 0),
                TimeParser.ParseTimeString("23:59"),
                "Does not handle time string without seconds.");
            Assert.AreEqual(
                new Time(23, 59, 0.015d),
                TimeParser.ParseTimeString("23:59:00.015"),
                "Does not handle fractional seconds.");
            Assert.IsNull(TimeParser.ParseTimeString("23:59:0.015"),
                "Incorrectly handles a non-0-padded fractional second.");
            Assert.IsNull(TimeParser.ParseTimeString(" "));
            Assert.IsNull(TimeParser.ParseTimeString("24-60-60"));
        }
    }
}
