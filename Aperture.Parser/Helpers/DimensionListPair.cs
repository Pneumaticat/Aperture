using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aperture.Parser.Helpers
{
    public enum DimensionListPairUnit
    {
        Percentage,
        Relative,
        Absolute
    }
    public struct DimensionListPair
    {
        public int number;
        public DimensionListPairUnit unit;
    }
}
