using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aperture.Parser.Miscellaneous;
using Aperture.Parser.DataStructures;

namespace Aperture.Parser.Tests
{
    [TestClass]
    public class YearlessDateParserTests
    {
        [TestMethod]
        public void TestYearlessDateStringParsing()
        {
            Assert.AreEqual(
                new MonthAndDay(2, 29),
                YearlessDateParser.ParseYearlessDateString("02-29"));
            Assert.AreEqual(
                new MonthAndDay(12, 31),
                YearlessDateParser.ParseYearlessDateString("12-31"));
            Assert.IsNull(YearlessDateParser.ParseYearlessDateString("1-1"));
            Assert.IsNull(YearlessDateParser.ParseYearlessDateString("13-32"));
            Assert.IsNull(YearlessDateParser.ParseYearlessDateString("11"));
            Assert.IsNull(YearlessDateParser.ParseYearlessDateString("00-00"));
            Assert.IsNull(YearlessDateParser.ParseYearlessDateString("     "));
        }
    }
}
