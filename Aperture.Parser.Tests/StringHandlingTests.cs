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
    }
}
