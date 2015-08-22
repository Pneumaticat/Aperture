using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aperture.Parser.HTML;
using Aperture.Parser.DataStructures;
using Aperture.Parser.HTML.DatesAndTimes;

namespace Aperture.Parser.Tests
{
    [TestClass]
    public class TimeZoneParserTests
    {
        [TestMethod]
        public void TestIsValidTimeZoneOffsetString()
        {
            Assert.IsTrue(TimeZoneParser.IsValidTimeZoneOffsetString(
                "+00:00", new TimeZoneOffset(0, 0)));
            Assert.IsTrue(TimeZoneParser.IsValidTimeZoneOffsetString(
                "Z", new TimeZoneOffset(0, 0)));
            Assert.IsTrue(TimeZoneParser.IsValidTimeZoneOffsetString(
                "-05:00", new TimeZoneOffset(-5, -0)));

            Assert.IsFalse(TimeZoneParser.IsValidTimeZoneOffsetString(
                "+001:00", new TimeZoneOffset(1, 0)));
            Assert.IsFalse(TimeZoneParser.IsValidTimeZoneOffsetString(
                "+0115randomcharacters", new TimeZoneOffset(1, 15)),
                "Incorrectly accepts time zones with characters after the offset.");
            Assert.IsFalse(TimeZoneParser.IsValidTimeZoneOffsetString(
                "-00:00", new TimeZoneOffset(-0, -0)),
                "Incorrectly accepts a negative 0 timezone offset (-00:00).");

            // Don't need to test time zone offset strings out of range (like -24:60) 
            // because the constructor for TimeZoneOffset will throw an 
            // exception if those kinds of values are passed to it, and if you 
            // use other, valid values in the constructor, like -23, -59, then 
            // IsValidTimeZoneOffsetString will reject the -24:60 offset string 
            // anyway because it does not match `new TimeZoneOffset(-23, -59)`.
        }
    }
}
