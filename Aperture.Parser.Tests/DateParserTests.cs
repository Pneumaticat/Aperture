using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aperture.Parser.HTML;
using Aperture.Parser.DataStructures;
using Aperture.Parser.HTML.Microsyntaxes.DatesAndTimes;

namespace Aperture.Parser.Tests
{
    [TestClass]
    public class DateParserTests
    {
        [TestMethod]
        public void TestIsValidDateString()
        {
            Assert.AreEqual(true, Dates.IsValidDateString("2000-12-01"));
            Assert.AreEqual(true, Dates.IsValidDateString("2000-01-01"));
            Assert.AreEqual(true, Dates.IsValidDateString("102000-05-01"),
                "Cannot handle year strings longer than 4 digits.");
            Assert.AreEqual(false, Dates.IsValidDateString("0000-01-01"),
                "Year string must be 1 or greater.");
            Assert.AreEqual(false, Dates.IsValidDateString("200011-14"),
                "Accepts year and month string without hyphen in between.");
            Assert.AreEqual(false, Dates.IsValidDateString("2000-17-01"),
                "Accepts month greater than 12.");
            Assert.AreEqual(false, Dates.IsValidDateString("2000-00-01"),
                "Accepts month string less than 1.");
            Assert.AreEqual(false, Dates.IsValidDateString("2000-01AAAAAA"));
            // Whitespace handling
            Assert.AreEqual(false, Dates.IsValidDateString(""));
            Assert.AreEqual(false, Dates.IsValidDateString("   "));
            Assert.AreEqual(false, Dates.IsValidDateString("2000-01"));
            Assert.AreEqual(false, Dates.IsValidDateString("2000-01-00"),
                "Incorrectly accepts days less than 1.");
        }

        [TestMethod]
        public void TestDateStringParsing()
        {
            Assert.AreEqual(new Date(2001, 12, 31), Dates.ParseDateString("2001-12-31"));
            // No February 29th on century boundaries!
            Assert.IsNull(Dates.ParseDateString("2100-02-29"));
            Assert.AreEqual(new Date(10500, 1, 1), Dates.ParseDateString("10500-01-01"));
            Assert.IsNull(Dates.ParseDateString("2000-1-1"),
                "Incorrectly handles 0-padding.");
            Assert.IsNull(Dates.ParseDateString("1-01-01"),
                "Incorrectly handles years with less than 4 characters.");
        }
    }
}
