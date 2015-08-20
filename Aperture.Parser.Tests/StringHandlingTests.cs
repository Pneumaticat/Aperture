using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aperture.Parser.Common;

namespace Aperture.Parser.Tests
{
    [TestClass]
    public class StringHandlingTests
    {
        /// <summary>
        /// Tests skipping whitespace in a string composed entirely of
        /// whitespace. The `position` variable should point past the end 
        /// of the string.
        /// </summary>
        [TestMethod]
        public void TestWhitespaceSkipInEmptyString()
        {
            int position = 0;
            string testStr = "         ";
            StringUtils.SkipWhitespace(testStr, ref position);
            Assert.IsTrue(position >= testStr.Length);
        }

        [TestMethod]
        public void TestWhitespaceSkip()
        {
            int position = 0;
            string testStr = "        x";
            StringUtils.SkipWhitespace(testStr, ref position);
            Assert.AreEqual('x', testStr[position]);
        }

        [TestMethod]
        public void TestCollectSequenceOfChars()
        {
            int position = 0;
            string Xes = StringUtils.CollectSequenceOfCharacters(
                "xxxxx other characters that don't really matter...",
                ref position,
                ch => ch == 'x');

            Assert.AreEqual("xxxxx", Xes);
            Assert.AreEqual(5, position); // Where position should end up in the string
        }

        [TestMethod]
        public void TestCollectCharsFromMiddleOfStringUntilEOS()
        {
            int position = 5;
            const string testStr = "01234xxx";
            string collected = StringUtils.CollectSequenceOfCharacters(
                testStr,
                ref position,
                ch => ch == 'x');

            Assert.AreEqual("xxx", collected);
            Assert.IsTrue(position == testStr.Length); // position should be past end of string
                                                       // (remember: position is 0-based, length 
                                                       // is 1-based)
        }

        [TestMethod]
        public void TestASCIICaseInsensitiveCompare()
        {
            Assert.IsTrue( StringUtils.CompareASCIICaseInsensitive("AAAA", "aaaa"));
            // Unicode characters with different capitalization shouldn't 
            // match using ascii comparing
            Assert.IsFalse(StringUtils.CompareASCIICaseInsensitive("AAAAÆ", "aaaaæ"));
        }

        [TestMethod]
        public void TestCompatibilityCaselessCompare()
        {
            Assert.IsTrue(StringUtils.CompareCompatibilityCaseless("Æ", "æ"));
            // Compare composed e + ◌́ with é for equality. Should be equal if 
            // normalization is correctly performed.
            Assert.IsTrue(StringUtils.CompareCompatibilityCaseless(
                "\u0065\u0301", "\u00e9"));
        }

        [TestMethod]
        public void TestConvertToASCIIUpperAndLowercase()
        {
            Assert.AreEqual("AAA", StringUtils.ConvertToASCIIUppercase("aAa"));
            Assert.AreEqual("aaa", StringUtils.ConvertToASCIILowercase("aAa"));
            // Unicode characters should not be able to be lowercased or 
            // uppercased with this method.
            Assert.AreNotEqual("aæa", StringUtils.ConvertToASCIILowercase("aÆa"));
            Assert.AreNotEqual("AÆA", StringUtils.ConvertToASCIIUppercase("aæa"));
        }

        [TestMethod]
        public void TestSplittingStrings()
        {
            CollectionAssert.AreEqual(
                new string[] { "x", "y", "c", "d" },
                StringUtils.StrictlySplitString('|', "x|y|c|d"));
            CollectionAssert.AreEqual(
                new string[] { "csdf", "hi", "17" },
                StringUtils.SplitStringOnCommas("  csdf, hi ,   17 ")
            );
        }

        [TestMethod]
        public void TestStripAndCollapseWhitespace()
        {
            Assert.AreEqual(
                "kittens are awesome",
                StringUtils.StripAndCollapseWhitespace(" kittens    are \r\n awesome\t")
            );
        }

        [TestMethod]
        public void TestTrimmingWhitespace()
        {
            Assert.AreEqual(
                "x x",
                StringUtils.TrimLeadingAndTrailingWhitespace("    x x   ")
            );
            Assert.AreEqual(
                string.Empty,
                StringUtils.TrimLeadingAndTrailingWhitespace("  \r\n\t    ")
            );
        }
    }
}
