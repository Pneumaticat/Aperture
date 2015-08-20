using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aperture.Parser.Common;
using Aperture.Parser.Helpers;
using System.Collections.Generic;

namespace Aperture.Parser.Tests
{
    [TestClass]
    public class DimensionListUnitTests
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
