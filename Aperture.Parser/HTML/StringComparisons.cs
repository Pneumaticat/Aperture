using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aperture.Parser.HTML
{
    /// <summary>
    /// Case-sensitivity and string comparison.
    /// </summary>
    public static class StringComparisons // Must end with s to avoid ambiguity
    {                                     // with System.StringComparison
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
                if (ParserIdioms.LowercaseASCIILetters.Contains(inputArr[i]))
                {
                    int charIndex = Array.IndexOf(
                        ParserIdioms.LowercaseASCIILetters, inputArr[i]);
                    inputArr[i] = ParserIdioms.UppercaseASCIILetters[charIndex];
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
                if (ParserIdioms.UppercaseASCIILetters.Contains(inputArr[i]))
                {
                    int charIndex = Array.IndexOf(
                        ParserIdioms.UppercaseASCIILetters, inputArr[i]);
                    inputArr[i] = ParserIdioms.LowercaseASCIILetters[charIndex];
                }
            }

            return new string(inputArr);
        }
    }
}
