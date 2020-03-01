/*########################################################
 *#  InternalLib.dll                                     #
 *#  Copyright 2018 by WesTex Enterprises                #
 *########################################################*/

using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace InternalLib
{
    public static class StringHelper
    {
        /// <summary>
        /// this method will return a string embedded with in another string
        /// </summary>
        /// <param name="strBegin">string to start at</param>
        /// <param name="strEnd">string to end at</param>
        /// <param name="strSource">source string</param>
        /// <param name="includeBegin">if true, include the beginning string</param>
        /// <param name="includeEnd">if true, include the ending string</param>
        /// <returns></returns>
        public static string[] GetStringInBetween(string strBegin, string strEnd, string strSource, bool includeBegin, bool includeEnd)
        {
            string[] result = { "", "" };
            int iIndexOfBegin = strSource.IndexOf(strBegin);
            if (iIndexOfBegin != -1)
            {
                // include the Begin string if desired
                if (includeBegin)
                {
                    iIndexOfBegin -= strBegin.Length;
                }
                strSource = strSource.Substring(iIndexOfBegin + strBegin.Length);

                int iEnd = strSource.IndexOf(strEnd);
                if (iEnd != -1)
                {
                    // include the End string if desired
                    if (includeEnd)
                    {
                        iEnd += strEnd.Length;
                    }
                    result[0] = strSource.Substring(0, iEnd);
                    // advance beyond this segment
                    if (iEnd + strEnd.Length < strSource.Length)
                    {
                        result[1] = strSource.Substring(iEnd + strEnd.Length);
                    }
                }
            }
            else
            {
                // stay where we are
                result[1] = strSource;
            }
            return result;
        }

        /// <summary>
        /// This method is responsible for getting the ending part of a string based on a stop character
        /// </summary>
        /// <param name="inString">The string to get the sub string from</param>
        /// <param name="startChar">the char to start coping from</param>
        /// <returns>a substring if start char is find, else a blank string</returns>
        static public string GetSubstringToEndByChar(string inString, char startChar)
        {
            string result = "";
            int indexOfChar = inString.IndexOf(startChar);
            if(indexOfChar > 0)
            {
                result = inString.Substring(indexOfChar + 1);
            }
            return result;
        }

        /// <summary>
        /// Split the string based on ?
        /// </summary>
        /// <param name="instr">source string</param>
        /// <returns>string array of the string split out</returns>
        static public string[] SplitParamaters(string instr)
        {
            string[] result;
            if (instr != null)
            {
                result = instr.Split('?');
            }
            else
            {
                result = new string[1];
            }
            return result;
        }

        /// <summary>
        /// this method will take a list of strings and return a comma delimited string
        /// </summary>
        /// <param name="inItems">source list</param>
        /// <returns>comma delimited string</returns>
        static public string ListItemsToString(List<string> inItems)
        {
            StringBuilder result = new StringBuilder();
            foreach (string it in inItems)
            {
                result = result.Append(it + ",");
            }
            return result.ToString();
        }

        /// <summary>
        /// this method will return a substring from a string with checking if
        /// the substring asked for is smaller than the source string
        /// </summary>
        /// <param name="indata">source string</param>
        /// <param name="max">the length of the substring</param>
        /// <returns>the substring</returns>
        static public string GetMaxString(string indata, int max)
        {
            string result = indata;

            if (indata.Length > max)
            {
                result = indata.Substring(0, max - 1);
            }
            return result;
        }

        static public byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        static public string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        static public string ZipStr(string str)
        {
            string result = str;
            var bytes = Encoding.UTF8.GetBytes(str);

            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    //msi.CopyTo(gs);
                    CopyTo(msi, gs);
                }
                Encoding.Default.GetString(mso.ToArray());
                return result;
            }
        }

        public static void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }

        public static string Unzip(byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    CopyTo(gs, mso);
                }

                return Encoding.UTF8.GetString(mso.ToArray());
            }
        }
    }
}