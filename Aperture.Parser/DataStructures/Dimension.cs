using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aperture.Parser.DataStructures
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
