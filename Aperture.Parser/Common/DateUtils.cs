using Aperture.Parser.DataStructures;
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
            if (yearNum < 1)
                return false;

            if (i >= input.Length)
				// Hit end of string after year string - not valid
                return false;

            if (input[i] == '-')
                i++;
            else
				// Next character after year is not a hyphen - not valid
                return false;

            // If next two characters are ASCII digits (month section)...
            string potentialMonthString = input.Substring(i, 2);
            if (potentialMonthString.All(c => StringUtils.ASCIIDigits.Contains(c)) &&
                // and it is between 1 and 12
                int.Parse(potentialMonthString) >= 1 &&
                int.Parse(potentialMonthString) <= 12)
            {
                i = i + 2;
                if (i >= input.Length)
                    return true;
                else
					// Have not reached end of string after the month segment - not valid
                    return false;
            }
            else
                return false;
        }

        // TODO: Should ParseMonthString return null upon failure?
        /// <summary>
        /// Parses a month string into a year and month, e.g. 2007-10. Returns 
        /// null upon failure.
        /// </summary>
        public static YearAndMonth? ParseMonthString(string input)
        {
            int position = 0;
            // hehe yam
            YearAndMonth? yam = ParseMonthComponent(input, ref position);
            // Fail if yam is null or position is _not_ beyond the end of input
            if (yam == null || position < input.Length)
                return null;
            else
                return yam;
        }

        public static YearAndMonth? ParseMonthComponent(string input, ref int position)
        {
            int year;
            string yearChars = StringUtils.CollectSequenceOfCharacters(
                input,
                ref position,
                ch => StringUtils.ASCIIDigits.Contains(ch));
            
            if (yearChars.Length < 4)
                return null;
            else
                year = int.Parse(yearChars);

            if (year <= 0)
                return null;

            if (position >= input.Length ||
                input[position] != '-')
                return null;
            else
                position++;
            
            int month;
            string monthChars = StringUtils.CollectSequenceOfCharacters(
                input, 
                ref position,
                ch => StringUtils.ASCIIDigits.Contains(ch));
            if (monthChars.Length != 2)
                return null;
            else
                month = int.Parse(monthChars);

            // Must be between range 1 ≤ month ≤ 12
            if (month < 1 || month > 12)
                return null;
            else
                return new YearAndMonth(year, month);
        }
    }
}
