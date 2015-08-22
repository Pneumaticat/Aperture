using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aperture.Parser.HTML;

namespace Aperture.Parser.Tests.HTML
{
    [TestClass]
    public class CommaSeparatedTokensTests
    {
        [TestMethod]
        public void TestSplittingStrings()
        {
            CollectionAssert.AreEqual(
                new string[] { "csdf", "hi", "17" },
                CommaSeparatedTokens.SplitStringOnCommas("  csdf, hi ,   17 ")
            );
        }
    }
}
