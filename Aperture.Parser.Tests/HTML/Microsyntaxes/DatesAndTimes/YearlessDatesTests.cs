using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aperture.Parser.DataStructures;
using Aperture.Parser.HTML.Microsyntaxes.DatesAndTimes;

namespace Aperture.Parser.Tests.HTML.Microsyntaxes.DateAndTimes
{
    [TestClass]
    public class YearlessDatesTests
    {
        [TestMethod]
        public void TestYearlessDateStringParsing()
        {
            Assert.AreEqual(
                new MonthAndDay(2, 29),
                YearlessDates.ParseYearlessDateString("02-29"));
            Assert.AreEqual(
                new MonthAndDay(12, 31),
                YearlessDates.ParseYearlessDateString("12-31"));
            Assert.IsNull(YearlessDates.ParseYearlessDateString("1-1"));
            Assert.IsNull(YearlessDates.ParseYearlessDateString("13-32"));
            Assert.IsNull(YearlessDates.ParseYearlessDateString("11"));
            Assert.IsNull(YearlessDates.ParseYearlessDateString("00-00"));
            Assert.IsNull(YearlessDates.ParseYearlessDateString("     "));
        }
    }
}
