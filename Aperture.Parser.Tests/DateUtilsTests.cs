using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aperture.Parser.Common;

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
    }
}
