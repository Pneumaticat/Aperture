using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aperture.Parser.Miscellaneous;

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
            StringParser.SkipWhitespace(testStr, ref position);
            Assert.IsTrue(position >= testStr.Length);
        }

        [TestMethod]
        public void TestWhitespaceSkip()
        {
            int position = 0;
            string testStr = "        x";
            StringParser.SkipWhitespace(testStr, ref position);
            Assert.AreEqual('x', testStr[position]);
        }

        [TestMethod]
        public void TestCollectSequenceOfChars()
        {
            int position = 0;
            string Xes = StringParser.CollectSequenceOfCharacters(
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
            string collected = StringParser.CollectSequenceOfCharacters(
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
            Assert.IsTrue( StringParser.CompareASCIICaseInsensitive("AAAA", "aaaa"));
            // Unicode characters with different capitalization shouldn't 
            // match using ascii comparing
            Assert.IsFalse(StringParser.CompareASCIICaseInsensitive("AAAAÆ", "aaaaæ"));
        }

        [TestMethod]
        public void TestCompatibilityCaselessCompare()
        {
            Assert.IsTrue(StringParser.CompareCompatibilityCaseless("Æ", "æ"));
            // Compare composed e + ◌́ with é for equality. Should be equal if 
            // normalization is correctly performed.
            Assert.IsTrue(StringParser.CompareCompatibilityCaseless(
                "\u0065\u0301", "\u00e9"));
        }

        [TestMethod]
        public void TestConvertToASCIIUpperAndLowercase()
        {
            Assert.AreEqual("AAA", StringParser.ConvertToASCIIUppercase("aAa"));
            Assert.AreEqual("aaa", StringParser.ConvertToASCIILowercase("aAa"));
            // Unicode characters should not be able to be lowercased or 
            // uppercased with this method.
            Assert.AreNotEqual("aæa", StringParser.ConvertToASCIILowercase("aÆa"));
            Assert.AreNotEqual("AÆA", StringParser.ConvertToASCIIUppercase("aæa"));
        }

        [TestMethod]
        public void TestSplittingStrings()
        {
            CollectionAssert.AreEqual(
                new string[] { "x", "y", "c", "d" },
                StringParser.StrictlySplitString('|', "x|y|c|d"));
            CollectionAssert.AreEqual(
                new string[] { "csdf", "hi", "17" },
                StringParser.SplitStringOnCommas("  csdf, hi ,   17 ")
            );
        }

        [TestMethod]
        public void TestStripAndCollapseWhitespace()
        {
            Assert.AreEqual(
                "kittens are awesome",
                StringParser.StripAndCollapseWhitespace(" kittens    are \r\n awesome\t")
            );
        }

        [TestMethod]
        public void TestTrimmingWhitespace()
        {
            Assert.AreEqual(
                "x x",
                StringParser.TrimLeadingAndTrailingWhitespace("    x x   ")
            );
            Assert.AreEqual(
                string.Empty,
                StringParser.TrimLeadingAndTrailingWhitespace("  \r\n\t    ")
            );
        }
    }
}
