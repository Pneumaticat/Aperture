using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aperture.Parser.Logging
{
    public static class ParserLogging
    {
        public static void LogNonConformingError(NonConformingError error)
        {
            Debug.Print("Non-conforming HTML error: NC" + error.ToString());
        }
    }
}
