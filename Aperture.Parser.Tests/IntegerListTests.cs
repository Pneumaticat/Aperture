using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Aperture.Parser.HTML;
using Aperture.Parser.Exceptions;
using Aperture.Parser.HTML.Numbers;

namespace Aperture.Parser.Tests
{
    [TestClass]
    public class IntegerListTests
    {
        [TestMethod]
        public void TestIntegerListParsing()
        {
            CollectionAssert.AreEqual(
                new List<int>
                {
                    500000,
                    1,
                    -754,
                    334334343
                },
                ListsOfIntegers.ParseListOfIntegers("500000, 1, -754, 334334343")
            );
        }

        [TestMethod]
        public void TestIntegerListWhitespaceHandling()
        {
            CollectionAssert.AreEqual(
                new List<int>(),
                ListsOfIntegers.ParseListOfIntegers("")
            );
            CollectionAssert.AreEqual(
                new List<int>(),
                ListsOfIntegers.ParseListOfIntegers(" ")
            );
            CollectionAssert.AreEqual(
                new List<int>(),
                ListsOfIntegers.ParseListOfIntegers(" , ")
            );
            CollectionAssert.AreEqual(
                new List<int>(),
                ListsOfIntegers.ParseListOfIntegers(",,,")
            );
        }

        [TestMethod]
        public void TestInvalidIntegerList()
        {
            // Upon failure, the integer list parser just returns the list it 
            // had up to that point. If it fails at the start, it returns an 
            // empty list.
            CollectionAssert.AreEqual(
                new List<int>(),
                ListsOfIntegers.ParseListOfIntegers("xxx,xxx,xxx")
            );
            // Test what happens when one element is invalid, should return 
            // the list it had up to the point of the invalid element
            CollectionAssert.AreEqual(
                new List<int>
                {
                    777,
                    888
                },
                ListsOfIntegers.ParseListOfIntegers("777, 888, xxx")
            );
            CollectionAssert.AreEqual(
                new List<int>
                {
                    7
                },
                // There is slightly different handling for unicode chars in 
                // the parser (for ex. ☃) which should be tested.
                ListsOfIntegers.ParseListOfIntegers("7, ☃, [][]")
            );
        }
        
        /// <summary>
        /// Tests the list: 545, 3.623, 5. It should truncate all decimals.
        /// TODO: I think? Maybe it's not supposed to truncate decimals.
        /// </summary>
        [TestMethod]
        public void TestInvalidIntegerListDecimals()
        {
            CollectionAssert.AreEqual(
                new List<int>
                {
                    545,
                    3,
                    5
                },
                ListsOfIntegers.ParseListOfIntegers("545, 3.623, 5")
            );
        }
    }
}
