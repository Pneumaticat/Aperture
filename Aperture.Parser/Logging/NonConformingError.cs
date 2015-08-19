using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aperture.Parser.Logging
{
    public enum NonConformingError
    {
        PlusSignAtBeginningOfIntegerString = 0001,
        PlusSignAtBeginningOfFloatingPointString = 0002
    }
}
