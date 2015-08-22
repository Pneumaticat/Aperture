using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aperture.Parser.HTML.Microsyntaxes;

namespace Aperture.Parser.Tests.HTML.Microsyntaxes
{
    [TestClass]
    public class ParserIdiomsTests
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

        [TestMethod]
        public void TestStrictlySplittingStrings()
        {
            CollectionAssert.AreEqual(
                new string[] { "x", "y", "c", "d" },
                ParserIdioms.StrictlySplitString('|', "x|y|c|d"));
        }
    }
}
