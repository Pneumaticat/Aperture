using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aperture.Parser.HTML;

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
            ParserIdioms.SkipWhitespace(testStr, ref position);
            Assert.IsTrue(position >= testStr.Length);
        }

        [TestMethod]
        public void TestWhitespaceSkip()
        {
            int position = 0;
            string testStr = "        x";
            ParserIdioms.SkipWhitespace(testStr, ref position);
            Assert.AreEqual('x', testStr[position]);
        }

        [TestMethod]
        public void TestCollectSequenceOfChars()
        {
            int position = 0;
            string Xes = ParserIdioms.CollectSequenceOfCharacters(
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
            string collected = ParserIdioms.CollectSequenceOfCharacters(
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
            Assert.IsTrue( StringComparisons.CompareASCIICaseInsensitive("AAAA", "aaaa"));
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

        [TestMethod]
        public void TestSplittingStrings()
        {
            CollectionAssert.AreEqual(
                new string[] { "x", "y", "c", "d" },
                ParserIdioms.StrictlySplitString('|', "x|y|c|d"));
            CollectionAssert.AreEqual(
                new string[] { "csdf", "hi", "17" },
                CommaSeparatedTokens.SplitStringOnCommas("  csdf, hi ,   17 ")
            );
        }

        [TestMethod]
        public void TestStripAndCollapseWhitespace()
        {
            Assert.AreEqual(
                "kittens are awesome",
                ParserIdioms.StripAndCollapseWhitespace(" kittens    are \r\n awesome\t")
            );
        }

        [TestMethod]
        public void TestTrimmingWhitespace()
        {
            Assert.AreEqual(
                "x x",
                ParserIdioms.TrimLeadingAndTrailingWhitespace("    x x   ")
            );
            Assert.AreEqual(
                string.Empty,
                ParserIdioms.TrimLeadingAndTrailingWhitespace("  \r\n\t    ")
            );
        }
    }
}
