using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aperture.Parser.HTML;
using Aperture.Parser.DataStructures;
using Aperture.Parser.HTML.Microsyntaxes.DatesAndTimes;

namespace Aperture.Parser.Tests.HTML.Microsyntaxes.DatesAndTimes
{
    [TestClass]
    public class TimeZonesTests
    {
        [TestMethod]
        public void TestIsValidTimeZoneOffsetString()
        {
            Assert.IsTrue(TimeZones.IsValidTimeZoneOffsetString(
                "+00:00", new TimeZoneOffset(0, 0)));
            Assert.IsTrue(TimeZones.IsValidTimeZoneOffsetString(
                "Z", new TimeZoneOffset(0, 0)));
            Assert.IsTrue(TimeZones.IsValidTimeZoneOffsetString(
                "-05:00", new TimeZoneOffset(-5, -0)));

            Assert.IsFalse(TimeZones.IsValidTimeZoneOffsetString(
                "+001:00", new TimeZoneOffset(1, 0)));
            Assert.IsFalse(TimeZones.IsValidTimeZoneOffsetString(
                "+0115randomcharacters", new TimeZoneOffset(1, 15)),
                "Incorrectly accepts time zones with characters after the offset.");
            Assert.IsFalse(TimeZones.IsValidTimeZoneOffsetString(
                "-00:00", new TimeZoneOffset(-0, -0)),
                "Incorrectly accepts a negative 0 timezone offset (-00:00).");

            // Don't need to test time zone offset strings out of range (like -24:60) 
            // because the constructor for TimeZoneOffset will throw an 
            // exception if those kinds of values are passed to it, and if you 
            // use other, valid values in the constructor, like -23, -59, then 
            // IsValidTimeZoneOffsetString will reject the -24:60 offset string 
            // anyway because it does not match `new TimeZoneOffset(-23, -59)`.
        }

        [TestMethod]
        public void TestParseTimeZoneOffsetString()
        {
            Assert.AreEqual(
                new TimeZoneOffset(1, 00),
                TimeZones.ParseTimeZoneOffsetString("+01:00"));
            Assert.AreEqual(
                new TimeZoneOffset(-1, -00),
                TimeZones.ParseTimeZoneOffsetString("-01:00"),
                "Cannot handle negative offsets.");
            Assert.AreEqual(
                new TimeZoneOffset(0, 00),
                TimeZones.ParseTimeZoneOffsetString("Z"),
                "Cannot handle UTC represented as 'Z'.");
            Assert.AreEqual(
                new TimeZoneOffset(0, 00),
                TimeZones.ParseTimeZoneOffsetString("+0000"),
                "Cannot handle offsets without a separating colon.");
            Assert.IsNull(TimeZones.ParseTimeZoneOffsetString("+1:00"),
                "Incorrectly handles non-zero-padded hour.");
        }
    }
}
