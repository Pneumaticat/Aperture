﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aperture.Parser.HTML;
using Aperture.Parser.Exceptions;
using Aperture.Parser.HTML.Microsyntaxes.Numbers;

namespace Aperture.Parser.Tests.HTML.Microsyntaxes.DatesAndTimes
{
    [TestClass]
    public class FloatingPointNumbersTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidFloatNumberException),
            "Whitespace was incorrectly allowed.")]
        public void TestWhitespaceInallowance()
        {
            FloatingPointNumbers.ParseFloatingPointNumber(" ");
            FloatingPointNumbers.ParseFloatingPointNumber("  ");
            FloatingPointNumbers.ParseFloatingPointNumber("  E  ");
        }

        [TestMethod]
        public void TestFloatingPointSyntax()
        {
            Assert.AreEqual(1.555e7d, FloatingPointNumbers.ParseFloatingPointNumber("1.555e7"),
                "Does not handle lowercase `e`.");
            Assert.AreEqual(1.2e-7d, FloatingPointNumbers.ParseFloatingPointNumber("1.2e-7"),
                "Does not handle lowercase `e` with negative exponent.");
            Assert.AreEqual(-1.0d, FloatingPointNumbers.ParseFloatingPointNumber("-1.0"),
                "Does not handle negative floating points.");
        }
        
        [TestMethod]
        public void TestFloatingPointAccuracy()
        {
            Assert.AreEqual(1.5d, FloatingPointNumbers.ParseFloatingPointNumber("1.5"));
            Assert.AreEqual(1.0d, FloatingPointNumbers.ParseFloatingPointNumber("1.0"));
            Assert.AreEqual(1.000001d,
                FloatingPointNumbers.ParseFloatingPointNumber("1.000001"));
            Assert.AreEqual(1.555E7d,
                FloatingPointNumbers.ParseFloatingPointNumber("1.555E7"),
                "Does not handle exponents in floating-points properly.");
            Assert.AreEqual(1.2E-7d, FloatingPointNumbers.ParseFloatingPointNumber("1.2E-7"),
                "Does not handle negative exponents properly.");
            Assert.AreEqual(1.10000001d,
                FloatingPointNumbers.ParseFloatingPointNumber("1.10000001"));
        }
    }
}
