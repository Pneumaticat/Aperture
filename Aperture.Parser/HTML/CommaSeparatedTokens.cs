using Aperture.Parser.HTML.Microsyntaxes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aperture.Parser.HTML
{
    public static class CommaSeparatedTokens
    {
        // 2.4.8 Comma-separated tokens
        // https://html.spec.whatwg.org/multipage/infrastructure.html#comma-separated-tokens
        public static string[] SplitStringOnCommas(string input)
        {
            // Not exactly following the spec, but the end result should be 
            // the same.
            return (from str
                    in input.Split(',')
                    select str.Trim(ParserIdioms.SpaceCharacters)
                   ).ToArray();
        }
    }
}
