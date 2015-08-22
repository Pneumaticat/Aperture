using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aperture.Parser.HTML;
using Aperture.Parser.Exceptions;
using Aperture.Parser.HTML.Numbers;

namespace Aperture.Parser.Tests
{
    [TestClass]
    public class FloatingPointTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidFloatNumberException),
            "Whitespace was incorrectly allowed.")]
        public void TestWhitespaceInallowance()
        {
            NumberParser.ParseFloatingPointNumber(" ");
            NumberParser.ParseFloatingPointNumber("  ");
            NumberParser.ParseFloatingPointNumber("  E  ");
        }

        [TestMethod]
        public void TestFloatingPointSyntax()
        {
            Assert.AreEqual(1.555e7d, NumberParser.ParseFloatingPointNumber("1.555e7"),
                "Does not handle lowercase `e`.");
            Assert.AreEqual(1.2e-7d, NumberParser.ParseFloatingPointNumber("1.2e-7"),
                "Does not handle lowercase `e` with negative exponent.");
            Assert.AreEqual(-1.0d, NumberParser.ParseFloatingPointNumber("-1.0"),
                "Does not handle negative floating points.");
        }
        
        [TestMethod]
        public void TestFloatingPointAccuracy()
        {
            Assert.AreEqual(1.5d, NumberParser.ParseFloatingPointNumber("1.5"));
            Assert.AreEqual(1.0d, NumberParser.ParseFloatingPointNumber("1.0"));
            Assert.AreEqual(1.000001d, NumberParser.ParseFloatingPointNumber("1.000001"));
            Assert.AreEqual(1.555E7d, NumberParser.ParseFloatingPointNumber("1.555E7"),
                "Does not handle exponents in floating-points properly.");
            Assert.AreEqual(1.2E-7d, NumberParser.ParseFloatingPointNumber("1.2E-7"),
                "Does not handle negative exponents properly.");
            Assert.AreEqual(1.10000001d, NumberParser.ParseFloatingPointNumber("1.10000001"));
        }
    }
}
