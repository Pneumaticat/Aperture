using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aperture.Parser.Common
{
    public static class DateUtils
    {
		public static bool IsValidMonthString(string input)
        {
            int i = 0;
            string yearDigits = StringUtils.CollectSequenceOfCharacters(
				input, 
				ref i, 
				(char ch) => StringUtils.ASCIIDigits.Contains(ch));

            if (yearDigits.Length < 4)
                return false;

            int yearNum = int.Parse(yearDigits);

            if (i >= input.Length)
				// Hit end of string after year string - not valid
                return false;

            if (input[i] == '-')
                i++;
            else
				// Next character after year is not a hyphen - not valid
                return false;

            // If next two characters are ASCII digits (month section)...
            if (input.Substring(i, 2).All(c => StringUtils.ASCIIDigits.Contains(c)))
                return true;
            else
                return false;
        }
    }
}
