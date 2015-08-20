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
    }
}
