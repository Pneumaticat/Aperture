using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aperture.Parser.HTML.Microsyntaxes.DatesAndTimes;
using Aperture.Parser.DataStructures;

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

        [TestMethod]
        public void TestParseGlobalDateAndTimeString()
        {
            Assert.AreEqual(
                new DateTimeWithTimeZone(
                    new DateTime(2001, 01, 01, 01, 01, 01),
                    new TimeZoneOffset(00, 00)),
                GlobalDatesAndTimes.ParseGlobalDateAndTimeString(
                    "2001-01-01T01:01:01Z"));

            // With ' ' in place of 'T' and +00:00 in place of Z
            Assert.AreEqual(
                new DateTimeWithTimeZone(
                    new DateTime(2001, 01, 01, 01, 01, 01),
                    new TimeZoneOffset(00, 00)),
                GlobalDatesAndTimes.ParseGlobalDateAndTimeString(
                    "2001-01-01 01:01:01+00:00"));

            // Different timezone
            Assert.AreEqual(
                new DateTimeWithTimeZone(
                    new DateTime(2001, 01, 01, 01, 01, 01)
                     - new TimeSpan(0, 5, 15, 0),
                    new TimeZoneOffset(05, 15)),
                GlobalDatesAndTimes.ParseGlobalDateAndTimeString(
                    "2001-01-01 01:01:01+05:15"));

            // Milliseconds
            Assert.AreEqual(
                new DateTimeWithTimeZone(
                    new DateTime(2001, 01, 01, 01, 01, 01, 001),
                    new TimeZoneOffset(00, 00)),
                GlobalDatesAndTimes.ParseGlobalDateAndTimeString(
                    "2001-01-01 01:01:01.001+00:00"));
        }
    }
}
