using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aperture.Parser.Common;
using Aperture.Parser.DataStructures;

namespace Aperture.Parser.Tests
{
    [TestClass]
    public class DateUtilsTests
    {
        [TestMethod]
        public void TestIsValidMonthString()
        {
            Assert.AreEqual(true, DateUtils.IsValidMonthString("2000-12"));
            Assert.AreEqual(true, DateUtils.IsValidMonthString("2000-01"));
            Assert.AreEqual(true, DateUtils.IsValidMonthString("102000-05"),
                "Cannot handle year strings longer than 4 digits.");
            Assert.AreEqual(false, DateUtils.IsValidMonthString("0000-01"),
                "Year string must be 1 or greater.");
            Assert.AreEqual(false, DateUtils.IsValidMonthString("200011"),
                "Accepts year and month string without hyphen in between.");
            Assert.AreEqual(false, DateUtils.IsValidMonthString("2000-17"),
                "Accepts month greater than 12.");
            Assert.AreEqual(false, DateUtils.IsValidMonthString("2000-00"),
                "Accepts month string less than 1.");
            Assert.AreEqual(false, DateUtils.IsValidMonthString("2000-01AAAAAA"));
        }

        [TestMethod]
        public void TestMonthStringParsing()
        {
            Assert.AreEqual(
                new YearAndMonth(2000, 1),
                DateUtils.ParseMonthString("2000-01"),
                "Incorrectly handles standard month string format.");
            Assert.AreEqual(
                new YearAndMonth(10500, 1),
                DateUtils.ParseMonthString("10500-01"),
                "Incorrectly handles years with 5+ digits.");

            // Failure cases

            // When no 0-padding is provided for month
            Assert.IsNull(DateUtils.ParseMonthString("2000-1"),
                "Incorrectly handles non-0-padded month.");
            // When month > 1 & is 3 digits
            Assert.IsNull(DateUtils.ParseMonthString("2000-100"),
                "Incorrectly handles large month number.");
            // When more chars are after month
            Assert.IsNull(DateUtils.ParseMonthString("2000-01-05"),
                "Incorrectly handles chars after month.");
            // When month > 1
            Assert.IsNull(DateUtils.ParseMonthString("2000-13"),
                "Incorrectly handles months greater than 12");
            // When month < 1
            Assert.IsNull(DateUtils.ParseMonthString("2000-00"),
                "Incorrectly handles months less than 1.");
            // When year <= 0
            Assert.IsNull(DateUtils.ParseMonthString("0000-01"),
                "Incorrectly handles years less than 1.");
            // Whitespace handling
            Assert.IsNull(DateUtils.ParseMonthString("       "),
                "Incorrectly handles strings with only whitespace.");
        }

        [TestMethod]
        public void TestLeapYearHandling()
        {
            // Leap years occur every 4 years...
            Assert.AreEqual(29, DateUtils.DaysInMonth(2, 2004));
            // Except every century, when it is skipped...
            Assert.AreEqual(28, DateUtils.DaysInMonth(2, 2100));
            // Unless the century is divisible by 4, when it isn't.
            Assert.AreEqual(29, DateUtils.DaysInMonth(2, 2000));
            Assert.AreEqual(29, DateUtils.DaysInMonth(2, 1600));
        }

        [TestMethod]
        public void TestIsValidDateString()
        {
            Assert.AreEqual(true, DateUtils.IsValidDateString("2000-12-01"));
            Assert.AreEqual(true, DateUtils.IsValidDateString("2000-01-01"));
            Assert.AreEqual(true, DateUtils.IsValidDateString("102000-05-01"),
                "Cannot handle year strings longer than 4 digits.");
            Assert.AreEqual(false, DateUtils.IsValidDateString("0000-01-01"),
                "Year string must be 1 or greater.");
            Assert.AreEqual(false, DateUtils.IsValidDateString("200011-14"),
                "Accepts year and month string without hyphen in between.");
            Assert.AreEqual(false, DateUtils.IsValidDateString("2000-17-01"),
                "Accepts month greater than 12.");
            Assert.AreEqual(false, DateUtils.IsValidDateString("2000-00-01"),
                "Accepts month string less than 1.");
            Assert.AreEqual(false, DateUtils.IsValidDateString("2000-01AAAAAA"));
            // Whitespace handling
            Assert.AreEqual(false, DateUtils.IsValidDateString(""));
            Assert.AreEqual(false, DateUtils.IsValidDateString("   "));
            Assert.AreEqual(false, DateUtils.IsValidDateString("2000-01"));
            Assert.AreEqual(false, DateUtils.IsValidDateString("2000-01-00"),
                "Incorrectly accepts days less than 1.");
        }

        [TestMethod]
        public void TestDateStringParsing()
        {
            Assert.AreEqual(new Date(2001, 12, 31), DateUtils.ParseDateString("2001-12-31"));
            // No February 29th on century boundaries!
            Assert.IsNull(DateUtils.ParseDateString("2100-02-29"));
            Assert.AreEqual(new Date(10500, 1, 1), DateUtils.ParseDateString("10500-01-01"));
            Assert.IsNull(DateUtils.ParseDateString("2000-1-1"),
                "Incorrectly handles 0-padding.");
            Assert.IsNull(DateUtils.ParseDateString("1-01-01"),
                "Incorrectly handles years with less than 4 characters.");
        }
    }
}
