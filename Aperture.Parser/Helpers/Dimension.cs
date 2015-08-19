using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aperture.Parser.Helpers
{
    public enum DimensionType
    {
        Percentage,
        Length
    }

    public struct Dimension
    {
        public decimal value;
        public DimensionType type;
    }
}
