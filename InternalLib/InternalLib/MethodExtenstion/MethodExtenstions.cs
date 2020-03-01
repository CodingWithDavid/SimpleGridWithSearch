/*########################################################
 *#  InternalLib.dll                                     #
 *#  Copyright 2018 by WesTex Enterprises                #
 *########################################################*/

using System;
using System.Text.RegularExpressions;
using System.Linq;
using System.Reflection;
using System.Text;

namespace InternalLib
{
    public static class MethodExtenstions
    {
        #region Float Extensions

        /// <summary>
        /// check to see if the float is zero
        /// </summary>
        /// <param name="num">source</param>
        /// <returns>true if zero</returns>
        public static bool IsZero(this float num)
        {
            bool result = false;
            if (num == 0)
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// check to see if the float is non zero
        /// </summary>
        /// <param name="num">source</param>
        /// <returns>true if not zero</returns>
        public static bool NotZero(this float num)
        {
            bool result = false;
            if (num != 0)
            {
                result = true;
            }

            return result;
        }

        #endregion Float Extensions

        #region Int Extensions

        /// <summary>
        /// check to see if the int is zero
        /// </summary>
        /// <param name="num">source</param>
        /// <returns>true if zero</returns>
        public static bool IsZero(this int num)
        {
            bool result = false;
            if (num == 0)
            { result = true; }

            return result;
        }

        /// <summary>
        /// check to see if the int is non zero
        /// </summary>
        /// <param name="num">source</param>
        /// <returns>true if not zero</returns>
        public static int NonZero(this int num)
        {
            int result = num;
            if (num < 0)
            {
                result = 0;
            }
            return result;
        }

        /// <summary>
        /// checks to see if the number is a 1
        /// </summary>
        /// <param name="num">source number</param>
        /// <returns>true if the number is a 1</returns>
        public static bool ToBool(this int num)
        {
            bool result = false;
            if (num == 1)
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// returns the smaller of 2 numbers
        /// </summary>
        /// <param name="numa">first number</param>
        /// <param name="numb">second number</param>
        /// <returns>the smaller number</returns>
        public static int UseMin(this int numa, int numb)
        {
            int result = numb;
            if (numa < numb)
            {
                result = numa;
            }
            return result;
        }

        /// <summary>
        /// Converts an integer in to a string that represents a binary value
        /// </summary>
        /// <param name="num">the number to convert</param>
        /// <param name="format">format the result with leading zeros</param>
        /// <returns>the string with the converted binary value</returns>
        public static string ToBinaryString(this int num, int format = 0)
        {
            string result = Convert.ToString(num, 2);
            if (format > 0)
            {
                result = result.PadLeft(format, '0');
            }
            return result;
        }

        #endregion Int Extenstions

        #region String Extensions

        /// <summary>
        /// checks to see if the string is a number
        /// </summary>
        /// <param name="num">source string</param>
        /// <returns>true if the string is a number</returns>
        public static bool IsNum(this string num)
        {
            bool result = !double.TryParse(num, out double tmp);
            return result;
        }

        /// <summary>
        /// checks to see if the string is null or empty
        /// </summary>
        /// <param name="str">source string</param>
        /// <returns>true if empty</returns>
        public static bool IsEmpty(this string str)
        {
            bool result = false;
            result = string.IsNullOrEmpty(str);
            return result;
        }

        /// <summary>
        /// Encodes a string in to base 64
        /// </summary>
        /// <param name="str">normal string to convert</param>
        /// <returns>converted string</returns>
        public static string ToBase64(this string str)
        {
            var result = System.Text.Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FromBase64(this string str)
        {
            var base64EncodedBytes = Convert.FromBase64String(str);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        /// <summary>
        /// checks to see if the string is not null or empty
        /// </summary>
        /// <param name="str">source string</param>
        /// <returns>true if the string is not empty</returns>
        public static bool IsNotEmpty(this string str)
        {
            bool result = false;
            result = !string.IsNullOrEmpty(str);
            return result;
        }

        /// <summary>
        /// this method will replace a null string with an empty string
        /// </summary>
        /// <param name="str">source string</param>
        /// <returns>cleaned string</returns>
        public static string NoNull(this string str)
        {
            string result = str;
            if (string.IsNullOrEmpty(str))
            {
                result = "";
            }
            return result;
        }

        /// <summary>
        /// this method will do a case insensitive string compare
        /// </summary>
        /// <param name="str">source string</param>
        /// <param name="match">string to compare to</param>
        /// <returns>true if the string match</returns>
        public static bool Match(this string str, string match)
        {
            bool result = false;
            if (str.IsEmpty())
            {
                if (match.IsEmpty())
                    result = true;
            }
            else
            {
                if (str.Equals(match, StringComparison.CurrentCultureIgnoreCase))
                {
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// this method will reformat the source string to be camel cased
        /// </summary>
        /// <param name="str">source string</param>
        /// <returns>mixed cased reformatted string</returns>
        public static string MixCase(this string str)
        {
            System.Globalization.CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Globalization.TextInfo textInfo = cultureInfo.TextInfo;
            return textInfo.ToTitleCase(str.ToLower());
        }

        /// <summary>
        /// this method will be used to format notes
        /// </summary>
        /// <param name="note">source note</param>
        /// <param name="userName">user name</param>
        /// <param name="newNote">new note</param>
        /// <returns>returns string with entries separated by carriage returns.</returns>
        public static string AppendNote(this string note, string userName, string newNote)
        {
            StringBuilder sb = new StringBuilder(note ?? string.Empty);
            if (sb.Length != 0)
            {
                sb.Append(Environment.NewLine);
            }
            sb.Append(userName);
            sb.Append(": ");
            sb.Append(newNote);
            sb.Append(" - ");
            sb.Append(DateTime.Now.ToString());
            return sb.ToString();
        }

        /// <summary>
        /// this method will clean a string to ensure only ASCII characters are in the string
        /// </summary>
        /// <param name="str">source string</param>
        /// <returns>reformatted string</returns>
        public static string MakeAsciiOnly(this string str)
        {
            if (!str.IsEmpty())
            {
                string fields = str.Replace('”', '"');
                fields = fields.Replace('’', '\'');
                fields = fields.Replace('“', '"');
                return Regex.Replace(fields, @"[^\x20-\x7F]", "");
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// this method will check to see if the string is a GUID
        /// </summary>
        /// <param name="str">source string</param>
        /// <returns>true if the string is a valid GUID</returns>
        public static bool IsAGuid(this string str)
        {
            bool result = false;
            if (Guid.TryParse(str, out Guid tmp))
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// fixes slashes in the string
        /// </summary>
        /// <param name="str">source string</param>
        /// <returns>reformatted string</returns>
        public static string FixUrlSlash(this string str)
        {
            string result = str;
            if (!str.IsEmpty())
                result = str.Replace('\\', '/');
            return result;
        }

        /// <summary>
        /// converts a string to a float
        /// </summary>
        /// <param name="str">source string</param>
        /// <returns>new float</returns>
        public static float ToFloat(this string str)
        {
            float.TryParse(str, out float result);
            return result;
        }

        /// <summary>
        /// converts a string to a decimal
        /// </summary>
        /// <param name="str">source string</param>
        /// <returns>new decimal</returns>
        public static decimal ToDecimal(this string str)
        {
            decimal.TryParse(str, out decimal result);
            return result;
        }

        /// <summary>
        /// converts a string to a double
        /// </summary>
        /// <param name="str">source string</param>
        /// <returns>new double</returns>
        public static double ToDouble(this string str)
        {
            double.TryParse(str, out double result);
            return result;
        }

        /// <summary>
        /// converts a string to an integer
        /// </summary>
        /// <param name="str">source string</param>
        /// <returns>integer</returns>
        public static int ToInt(this string str)
        {
            int.TryParse(str, out int result);
            return result;
        }

        /// <summary>
        /// converts a string to a long
        /// </summary>
        /// <param name="str">source string</param>
        /// <returns>long</returns>
        public static long ToLong(this string str)
        {
            long.TryParse(str, out long result);
            return result;
        }

        /// <summary>
        /// this method will check to see if the string is an integer
        /// </summary>
        /// <param name="str">source string</param>
        /// <returns>true if the string is a valid integer</returns>
        public static bool IsAnInt(this string str)
        {
            bool result = false;
            if (int.TryParse(str, out int tmp))
            {
                result = true;
            }
            return result;
        }


        /// <summary>
        /// converts a string to a date time
        /// </summary>
        /// <param name="str">source string</param>
        /// <returns>date time</returns>
        public static DateTime ToDateTime(this string str)
        {
            DateTime.TryParse(str, out DateTime result);
            return result;
        }

        /// <summary>
        /// converts a string to an Boolean
        /// </summary>
        /// <param name="str">source string</param>
        /// <returns>bool value</returns>
        static public bool ToBool(this string str)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(str))
            {
                if (str.ToLower().Equals("true"))
                {
                    result = true;
                }
                else if (str.ToLower().Equals("yes"))
                {
                    result = true;
                }
                else if (str.Equals("1"))
                {
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// this method will insert single quotes around a string
        /// </summary>
        /// <param name="target">source string</param>
        /// <returns>the quoted string</returns>
        static public string SingleQuote(this string target)
        {
            string result = target;
            if (result.IsNotEmpty())
            {
                result = "'" + target + "'";
            }
            return result;
        }

        /// <summary>
        /// this method will insert double quotes around a string
        /// </summary>
        /// <param name="target">source string</param>
        /// <returns>the quoted string</returns>
        static public string DoubleQuote(this string target)
        {
            string result = target;
            if (result.IsNotEmpty())
            {
                result = "\"" + target + "\"";
            }
            return result;
        }

        /// <summary>
        /// Converts to an integer a string representing the binary value
        /// </summary>
        /// <param name="target">the string to convert</param>
        /// <returns>the integer that was converted</returns>
        public static int ToBinaryInt(this string target)
        {
            return Convert.ToInt32(target, 2);
        }

        /// <summary>
        /// Checks to see if the string is a valid date
        /// </summary>
        /// <param name="target">the string to test</param>
        /// <returns>true if the string is a valid date</returns>
        public static bool IsADate(this string target)
        {
            bool result = false;
            DateTime tmpDate = new DateTime();
            result = DateTime.TryParse(target, out tmpDate);
            return result;
        }

        /// <summary>Gets date time newValue if possible or returns default val</summary>
        /// <param name = "valueToConvert">The newValue to convert.</param>
        /// <param name = "defaultVal">The default val.</param>
        /// <returns>date time newValue (default val if not convertible)</returns>
        public static DateTime GetSafeDate(this string valueToConvert, DateTime defaultVal)
        {
            bool isDt = DateTime.TryParse(valueToConvert, out DateTime returnVal);
            return isDt ? returnVal : defaultVal;
        }

        /// <summary>Gets decimal newValue if possible or returns -1</summary>
        /// <param name = "valueToConvert">The newValue to convert.</param>
        /// <returns>decimal newValue (-1 if not convertible)</returns>
        public static decimal GetSafeDecimal(this string valueToConvert)
        {
            bool isDecimal = decimal.TryParse(valueToConvert, out decimal returnVal);
            return isDecimal ? returnVal : -1;
        }
        #endregion String Extenstions

        #region TypeExtensions
        /// <summary>
        /// Returns a concatenated string with each readable property name and value
        /// </summary>
        /// <typeparam name="T">object type so that property info can be inspected</typeparam>
        /// <param name="objectInstance">the object from which to read property names/values</param>
        /// <returns>string containing property info for passed in object</returns>
        public static string GetPropertiesInfo<T>(this object objectInstance)
        {
            Type parentObjType = typeof(T);
            var sb = new StringBuilder();
            foreach (PropertyInfo prop in parentObjType.GetProperties().Where(prop => prop != null))
            {
                if (sb.Length > 0)
                    sb.Append("|" + prop.Name + ": " + prop.GetValue(objectInstance, null));
                else
                    sb.Append(prop.Name + ": " + prop.GetValue(objectInstance, null));
            }
            return sb.ToString();
        }
        #endregion
    }
}