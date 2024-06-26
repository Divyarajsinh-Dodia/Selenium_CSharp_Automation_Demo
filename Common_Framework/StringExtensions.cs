using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common_Framework
{
    public static class StringExtensions
    {
        /// <summary>
        /// Formats the entered string to match a phone number, (123)456-7890
        /// </summary>
        /// <param name="inputValue">Phone Number String</param>
        /// <returns>Formatted Phone Number</returns>
        public static string FormatUsaPhoneNumber(this string inputValue)
        {
            var sb = new StringBuilder(inputValue);
            sb.Insert(6, "-");
            sb.Insert(3, ") ");
            sb.Insert(0, "(");
            return sb.ToString();
        }

        /// <summary>
        /// Removes all of the invisible characters that are in a string taken from the UI
        /// </summary>
        /// <param name="inputValue">String to be modified</param>
        /// <returns>String without invisible characters</returns>
        public static string RemoveSpecialCharacters(this string inputValue)
        {
            inputValue = Regex.Replace(inputValue, @"[\t|\r|\n]", string.Empty);
            return inputValue.Trim();
        }

        /// <summary>
        /// Removes the formatted parts of the Phone Number and returns 1234567890
        /// </summary>
        /// <param name="phoneNumber">String to have the formatting removed</param>
        /// <returns>Unformatted Phone Number String</returns>
        public static string UnFormatUsaPhoneNumber(this string phoneNumber)
        {
            phoneNumber = phoneNumber.Replace("(", string.Empty);
            phoneNumber = phoneNumber.Replace(")", string.Empty);
            phoneNumber = phoneNumber.Replace(" ", string.Empty);
            phoneNumber = phoneNumber.Replace("-", string.Empty);
            return phoneNumber;
        }

        /// <summary>
        /// Strip out markup and whitespace from a string
        /// </summary>
        /// <returns>Returns clean and tidy string.</returns>
        /// <param name="s">String that needs to be cleaned.</param>
        public static string Tidy(this string s)
        {
            //Replace any junk
            s = s.Replace("\r\n", string.Empty).Replace("&nbsp;", " ");

            //Get rid of any duplicated white space after above clean-up
            s = Regex.Replace(s, @"\s{2,}", " ");

            //Trim to remove any leading or trailing whitespace
            s = s.Trim();

            return s;
        }

        /// <summary>
        /// Function to generate random string of given length and use calling string as prefix to it.
        /// If you don't want prefix call this method with string.Empty
        /// </summary>
        /// <param name="s">Calling string. Use as prefix to generated string</param>
        /// <param name="length">String length</param>
        /// <returns>string</returns>
        public static string RandomString(this string s, int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var rand = new Random();
            var randomStr = new string(Enumerable.Repeat(chars, length)
              .Select(str => str[rand.Next(str.Length)]).ToArray());
            return $"{s}{randomStr}";
        }
    }
}
