﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Aperture.Parser.Common
{
    public static class StringUtils
    {
        // HTML spec 2.3 Case-sensitivity and string comparisons
        
        public static bool CompareCaseSensitive(string str1, string str2)
        {
            return str1 == str2;
        }
        /// <summary>
        /// Compares strings, where A-Z == a-z.
        /// </summary>
        public static bool CompareASCIICaseInsensitive(string str1, string str2)
        {
            return ConvertToASCIILowercase(str1) == ConvertToASCIILowercase(str2);
        }

        /// <summary>
        /// Unicode string comparing, with no regard for capitalization.
        /// </summary>
        public static bool CompareCompatibilityCaseless(string str1, string str2)
        {
            // Unicode spec version 7.0, page 158, D146
            // TODO: ToLowerInvariant() may not match the spec's toCasefold method exactly.
            return str1.Normalize(NormalizationForm.FormD)
                .ToLowerInvariant()
                .Normalize(NormalizationForm.FormKD)
                .ToLowerInvariant()
                .Normalize(NormalizationForm.FormKD)

                ==

                str2.Normalize(NormalizationForm.FormD)
                .ToLowerInvariant()
                .Normalize(NormalizationForm.FormKD)
                .ToLowerInvariant()
                .Normalize(NormalizationForm.FormKD);
        }

        /// <summary>
        /// Converts a-z to A-Z.
        /// </summary>
        public static string ConvertToASCIIUppercase(string input)
        {
            char[] inputArr = input.ToCharArray();
            for (int i = 0; i < inputArr.Length; i++)
            {
                if (LowercaseASCIILetters.Contains(inputArr[i]))
                {
                    int charIndex = Array.IndexOf(LowercaseASCIILetters, inputArr[i]);
                    inputArr[i] = UppercaseASCIILetters[charIndex];
                }
            }

            return new string(inputArr);
        }

        /// <summary>
        /// Converts A-Z to a-z.
        /// </summary>
        public static string ConvertToASCIILowercase(string input)
        {
            char[] inputArr = input.ToCharArray();
            for (int i = 0; i < inputArr.Length; i++)
            {
                if (UppercaseASCIILetters.Contains(inputArr[i]))
                {
                    int charIndex = Array.IndexOf(UppercaseASCIILetters, inputArr[i]);
                    inputArr[i] = LowercaseASCIILetters[charIndex];
                }
            }

            return new string(inputArr);
        }

        // HTML spec 2.4.1 Common parser idioms

        /// <summary>
        /// White space characters, for the purpose of HTML.
        /// </summary>
        public static readonly char[] SpaceCharacters =
        {
            '\u0020', // SPACE
            '\u0009', // CHARACTER TABULATION (tab)
            '\u000A', // LINE FEED (LF)
            '\u000C', // FORM FEED (FF)
            '\u000D'  // CARRIAGE RETURN (CR)
        };

        /// <summary>
        /// White space characters, according to the Unicode spec.
        /// </summary>
        public static readonly char[] White_SpaceCharacters =
        {
            '\u0009',         // <control-0009>..<control-000D>
            '\u000A',
            '\u000B',
            '\u000C',
            '\u000D',
            ' ',               // ... it's a space.
            '\u0085',          // <control-0085>
            '\u00A0',          // NO BREAK SPACE
            '\u1680',          // OGHAM SPACE MARK
            '\u2000',          // EN QUAD..HAIR SPACE
            '\u2001',
            '\u2002',
            '\u2003',
            '\u2004',
            '\u2005',
            '\u2006',
            '\u2007',
            '\u2008',
            '\u2009',
            '\u200A',
            '\u2028',          // LINE SEPARATOR
            '\u2029',          // PARAGRAPH SEPARATOR
            '\u202F',          // NARROW NO-BREAK SPACE
            '\u205F',          // MEDIUM MATHEMATICAL SPACE
            '\u3000',          // IDEOGRAPHIC SPACE
        };

        public static readonly char[] UppercaseASCIILetters =
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
            'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
        };

        public static readonly char[] LowercaseASCIILetters =
        {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
            'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
        };

        public static readonly char[] ASCIIDigits =
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
        };

        public static readonly char[] ASCIIHexDigits =
            "0123456789ABCDEFabcdef".ToCharArray();
        public static readonly char[] UppercaseASCIIHexDigits =
            "0123456789ABCDEF".ToCharArray();
        public static readonly char[] LowercaseASCIIHexDigits =
            "0123456789abcdef".ToCharArray();
        
        /// <summary>
        /// Collects a seqence of characters for as long as <em>predicate</em> 
        /// is true.
        /// </summary>
        /// <param name="input">The string to collect from.</param>
        /// <param name="position">
        /// The position to begin collecting from in the string; this is 
        /// modified to point to the character after the collected sequence. 
        /// (May point past the end of the string, if the sequence of 
        /// characters reached the end of the string.)
        /// </param>
        /// <param name="predicate">
        /// Collects characters for as long as this returns true.
        /// </param>
        /// <returns>The sequence collected.</returns>
        public static string CollectSequenceOfCharacters(string input, ref int position, Func<char, bool> predicate)
        {
            string result = string.Empty;
            while (position < input.Length && predicate(input[position]) == true)
            {
                result += input[position];
                position++;
            }

            return result;
        }

        /// <summary>
        /// Skips whitespace in a string, and changes the position to the next 
        /// non-whitespace character.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="position">
        /// The current position at which to skip whitespace.
        /// </param>
        /// <returns>The space characters obtained.</returns>
        public static string SkipWhitespace(string input, ref int position)
        {
            return CollectSequenceOfCharacters(
                input,
                ref position,
                (char ch) => SpaceCharacters.Contains(ch));
        }

        /// <summary>
        /// Removes all line breaks (\r and \n) from a string.
        /// </summary>
        public static string StripLineBreaks(string input)
        {
            return input.Replace("\r", "").Replace("\n", "");
        }

        /// <summary>
        /// Removes all space characters from the beginning and end of input.
        /// </summary>
        public static string TrimLeadingAndTrailingWhitespace(string input)
        {
            return input.Trim(SpaceCharacters);
        }

        /// <summary>
        /// Replaces sequences of 1+ space chars in input with a single space, 
        /// and then strips leading and trailing whitespace.
        /// </summary>
        public static string StripAndCollapseWhitespace(string input)
        {
            StringBuilder sbOut = new StringBuilder();
            if (!string.IsNullOrEmpty(input))
            {
                bool currentlyWhitespace = false;
                for (int i = 0; i < input.Length; i++)
                {
                    if (SpaceCharacters.Contains(input[i]))
                    {
                        if (!currentlyWhitespace)
                        {
                            // This is the beginning of a new whitespace 
                            // sequence, add a space.
                            sbOut.Append(' ');
                            currentlyWhitespace = true;
                        }
                        // Otherwise this is just a continuation of a 
                        // whitespace sequence, do nothing.
                    }
                    else
                    {
                        currentlyWhitespace = false;
                        sbOut.Append(input[i]);
                    }
                }
            }
            return TrimLeadingAndTrailingWhitespace(sbOut.ToString());
        }

        public static string[] StrictlySplitString(char delimiter, string input)
        {
            int position = 0;
            List<string> tokens = new List<string>();

            while (position < input.Length)
            {
                tokens.Add(
                    CollectSequenceOfCharacters(
                        input,
                        ref position,
                        (char ch) => ch != delimiter));

                position++;
            }

            return tokens.ToArray();
        }

        // 2.4.8 Comma-separated tokens
        public static string[] SplitStringOnCommas(string input)
        {
            // Not exactly following the spec, but the end result should be 
            // the same.
            return (from str
                    in input.Split(',')
                    select str.Trim(SpaceCharacters)
                   ).ToArray();
        }
    }
}
