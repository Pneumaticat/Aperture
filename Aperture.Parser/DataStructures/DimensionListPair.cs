using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aperture.Parser.DataStructures
{
    public enum DimensionListPairUnit
    {
        Percentage,
        Relative,
        Absolute
    }
    public struct DimensionListPair
    {
        public DimensionListPair(double number, DimensionListPairUnit unit)
        {
            Number = number;
            Unit = unit;
        }
        public double Number { get; }
        public DimensionListPairUnit Unit { get; }
    }
}
