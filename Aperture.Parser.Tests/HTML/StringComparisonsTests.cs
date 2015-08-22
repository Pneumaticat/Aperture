using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aperture.Parser.HTML;

namespace Aperture.Parser.Tests.HTML
{
    [TestClass]
    public class StringComparisonsTests
    {
        [TestMethod]
        public void TestASCIICaseInsensitiveCompare()
        {
            Assert.IsTrue(StringComparisons.CompareASCIICaseInsensitive("AAAA", "aaaa"));
            // Unicode characters with different capitalization shouldn't 
            // match using ascii comparing
            Assert.IsFalse(StringComparisons.CompareASCIICaseInsensitive("AAAAÆ", "aaaaæ"));
        }

        [TestMethod]
        public void TestCompatibilityCaselessCompare()
        {
            Assert.IsTrue(StringComparisons.CompareCompatibilityCaseless("Æ", "æ"));
            // Compare composed e + ◌́ with é for equality. Should be equal if 
            // normalization is correctly performed.
            Assert.IsTrue(StringComparisons.CompareCompatibilityCaseless(
                "\u0065\u0301", "\u00e9"));
        }

        [TestMethod]
        public void TestConvertToASCIIUpperAndLowercase()
        {
            Assert.AreEqual("AAA", StringComparisons.ConvertToASCIIUppercase("aAa"));
            Assert.AreEqual("aaa", StringComparisons.ConvertToASCIILowercase("aAa"));
            // Unicode characters should not be able to be lowercased or 
            // uppercased with this method.
            Assert.AreNotEqual("aæa", StringComparisons.ConvertToASCIILowercase("aÆa"));
            Assert.AreNotEqual("AÆA", StringComparisons.ConvertToASCIIUppercase("aæa"));
        }
    }
}
