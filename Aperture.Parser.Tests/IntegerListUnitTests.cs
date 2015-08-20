using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Aperture.Parser.Common;
using Aperture.Parser.Exceptions;

namespace Aperture.Parser.Tests
{
    [TestClass]
    public class IntegerListUnitTests
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
                NumberUtils.ParseListOfIntegers("500000, 1, -754, 334334343")
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
                NumberUtils.ParseListOfIntegers("545, 3.623, 5")
            );
        }
    }
}
