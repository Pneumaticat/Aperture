using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aperture.Parser.Common;
using Aperture.Parser.DataStructures;
using System.Collections.Generic;

namespace Aperture.Parser.Tests
{
    [TestClass]
    public class DimensionListTests
    {
        [TestMethod]
        public void TestDimensionListParsing()
        {
            CollectionAssert.AreEqual(
                new List<DimensionListPair>
                {
                    new DimensionListPair
                    {
                        number = 50,
                        unit = DimensionListPairUnit.Percentage
                    },
                    new DimensionListPair
                    {
                        number = 7,
                        unit = DimensionListPairUnit.Relative
                    },
                    new DimensionListPair
                    {
                        number = 20,
                        unit = DimensionListPairUnit.Absolute
                    },
                    new DimensionListPair
                    {
                        number = 40,
                        unit = DimensionListPairUnit.Absolute
                    }
                },
                NumberUtils.ParseListOfDimensions("50%, 7*, 20, 40")
            );
        }

        [TestMethod]
        public void TestSpaceBeforeDimensionUnit()
        {
            CollectionAssert.AreEqual(
                new List<DimensionListPair>
                {
                    new DimensionListPair
                    {
                        number = 5,
                        unit = DimensionListPairUnit.Percentage
                    },
                    new DimensionListPair
                    {
                        number = 7,
                        unit = DimensionListPairUnit.Relative
                    }
                },
                NumberUtils.ParseListOfDimensions("5  %, 7  *")
            );
        }

        [TestMethod]
        public void TestDimensionListWhitespace()
        {
            CollectionAssert.AreEqual(
                new List<DimensionListPair>
                {
                    new DimensionListPair
                    {
                        number = 7,
                        unit = DimensionListPairUnit.Absolute
                    },
                    new DimensionListPair
                    {
                        number = 8,
                        unit = DimensionListPairUnit.Absolute
                    },
                    new DimensionListPair
                    {
                        number = 9,
                        unit = DimensionListPairUnit.Absolute
                    }
                },
                NumberUtils.ParseListOfDimensions(
                    "               7,           8,             9 ")
            );
            // Should return 0, relative when given an invalid whitespace-only 
            // value.
            CollectionAssert.AreEqual(
                new List<DimensionListPair>
                {
                    new DimensionListPair
                    {
                        number = 0,
                        unit = DimensionListPairUnit.Relative
                    }
                },
                NumberUtils.ParseListOfDimensions("  ")
            );
        }

        [TestMethod]
        public void TestDimensionListDecimals()
        {
            CollectionAssert.AreEqual(
                new List<DimensionListPair>
                {
                    new DimensionListPair
                    {
                        number = 1.5d,
                        unit = DimensionListPairUnit.Absolute
                    },
                    new DimensionListPair
                    {
                        number = 7.55d,
                        unit = DimensionListPairUnit.Percentage
                    },
                    new DimensionListPair
                    {
                        number = 55,
                        unit = DimensionListPairUnit.Relative
                    }
                },
                NumberUtils.ParseListOfDimensions("1.5, 7.55%, 55*")
            );
        }
    }
}
