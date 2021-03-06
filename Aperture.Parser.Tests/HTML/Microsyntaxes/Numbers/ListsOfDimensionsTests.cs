﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aperture.Parser.DataStructures;
using System.Collections.Generic;
using Aperture.Parser.HTML.Microsyntaxes.Numbers;

namespace Aperture.Parser.Tests.HTML.Microsyntaxes.Numbers
{
    [TestClass]
    public class ListsOfDimensionsTests
    {
        [TestMethod]
        public void TestDimensionListParsing()
        {
            CollectionAssert.AreEqual(
                new List<DimensionListPair>
                {
                    new DimensionListPair(50, DimensionListPairUnit.Percentage),
                    new DimensionListPair(7,  DimensionListPairUnit.Relative),
                    new DimensionListPair(20, DimensionListPairUnit.Absolute),
                    new DimensionListPair(40, DimensionListPairUnit.Absolute)
                },
                ListsOfDimensions.ParseListOfDimensions("50%, 7*, 20, 40")
            );
        }

        [TestMethod]
        public void TestSpaceBeforeDimensionUnit()
        {
            CollectionAssert.AreEqual(
                new List<DimensionListPair>
                {
                    new DimensionListPair(5, DimensionListPairUnit.Percentage),
                    new DimensionListPair(7, DimensionListPairUnit.Relative)
                },
                ListsOfDimensions.ParseListOfDimensions("5  %, 7  *")
            );
        }

        [TestMethod]
        public void TestDimensionListWhitespace()
        {
            CollectionAssert.AreEqual(
                new List<DimensionListPair>
                {
                    new DimensionListPair(7, DimensionListPairUnit.Absolute),
                    new DimensionListPair(8, DimensionListPairUnit.Absolute),
                    new DimensionListPair(9, DimensionListPairUnit.Absolute)
                },
                ListsOfDimensions.ParseListOfDimensions(
                    "               7,           8,             9 ")
            );
            // Should return 0, relative when given an invalid whitespace-only 
            // value.
            CollectionAssert.AreEqual(
                new List<DimensionListPair>
                {
                    new DimensionListPair(0, DimensionListPairUnit.Relative)
                },
                ListsOfDimensions.ParseListOfDimensions("  ")
            );
        }

        [TestMethod]
        public void TestDimensionListDecimals()
        {
            CollectionAssert.AreEqual(
                new List<DimensionListPair>
                {
                    new DimensionListPair(1.5d,  DimensionListPairUnit.Absolute),
                    new DimensionListPair(7.55d, DimensionListPairUnit.Percentage),
                    new DimensionListPair(55,    DimensionListPairUnit.Relative)
                },
                ListsOfDimensions.ParseListOfDimensions("1.5, 7.55%, 55*")
            );
        }
    }
}
