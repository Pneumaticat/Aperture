using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aperture.Parser.Common;
using Aperture.Parser.DataStructures;

namespace Aperture.Parser.Tests
{
    [TestClass]
    public class DateTimeUtilsTests
    {
        [TestMethod]
        public void TestLeapYearHandling()
        {
            // Leap years occur every 4 years...
            Assert.AreEqual(29, DateTimeUtils.DaysInMonth(2, 2004));
            // Except every century, when it is skipped...
            Assert.AreEqual(28, DateTimeUtils.DaysInMonth(2, 2100));
            // Unless the century is divisible by 4, when it isn't.
            Assert.AreEqual(29, DateTimeUtils.DaysInMonth(2, 2000));
            Assert.AreEqual(29, DateTimeUtils.DaysInMonth(2, 1600));
        }
    }
}
