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
    }
}
