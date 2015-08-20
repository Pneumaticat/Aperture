using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aperture.Parser.Common;
using Aperture.Parser.Exceptions;

namespace Aperture.Parser.Tests
{
    [TestClass]
    public class FloatingPointUnitTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidFloatNumberException),
            "Whitespace was incorrectly allowed.")]
        public void TestWhitespaceInallowance()
        {
            NumberUtils.ParseFloatingPointNumber(" ");
            NumberUtils.ParseFloatingPointNumber("  ");
            NumberUtils.ParseFloatingPointNumber("  E  ");
        }

        [TestMethod]
        public void TestFloatingPointAccuracy()
        {
            Assert.AreEqual(1.0d, NumberUtils.ParseFloatingPointNumber("1.0"));
            Assert.AreEqual(1.1d, NumberUtils.ParseFloatingPointNumber("1.1"));
        }
    }
}
