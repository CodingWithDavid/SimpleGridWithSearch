/*########################################################
 *#  InternalLib.dll                                     #
 *#  Copyright 2018 by WesTex Enterprises                #
 *########################################################*/

using System;
using System.Linq;
using System.Net;

namespace InternalLib
{
    public static class URLHelper
    {
        /// <summary>
        /// this method will ensure the slashes on the in string are correct
        /// </summary>
        /// <param name="inUrl">source string</param>
        /// <returns>the cleaned string</returns>
        static public string CleanUrl(string inUrl)
        {
            string result = inUrl;
            //check for leading /
            if (inUrl[0] == '/')
            {
                result = inUrl.Substring(1);
            }
            return result;
        }

        /// <summary>
        /// this method will ensure there is an ending slash on the string
        /// </summary>
        /// <param name="inUrl">source string</param>
        /// <returns>modified string</returns>
        static public string EndingSlash(string inUrl)
        {
            string result = inUrl;
            if (inUrl.IsNotEmpty())
            {
                //check for leading /
                if (inUrl[inUrl.Length - 1] != '/')
                {
                    result = inUrl + "/";
                }
            }
            return result;
        }

        /// <summary>
        /// this method will ping the URL provided and return if the URL is live
        /// </summary>
        /// <param name="url">URL to ping</param>
        /// <returns>true if there is a response, else false</returns>
        static public bool PingUrl(string url)
        {
            bool result = false;

            try
            {
                //Creating the HttpWebRequest
                if (WebRequest.Create(Uri.EscapeUriString(url)) is HttpWebRequest request)
                {
                    //Setting the Request method HEAD, you can also use GET too.
                    request.Method = "HEAD";
                    //Getting the Web Response.
                    if (request.GetResponse() is HttpWebResponse response)
                    {
                        //Returns TURE if the Status code == 200
                        result = (response.StatusCode == HttpStatusCode.OK);
                        if (!result)
                        {
                            Console.WriteLine($"Error response: {response.StatusCode} - {response.StatusDescription}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Response was null");
                    }
                }
            }
            catch(Exception ex)
            {
                //just return false
                Console.WriteLine($"Error: {ex.Message}");
            }

            return result;
        }

        /// <summary>
        /// this method will ensure the URL contains HTTP and has proper slashes
        /// </summary>
        /// <param name="url">source string to validate</param>
        /// <returns>formatted string</returns>
        static public string FormatUrl(string url)
        {
            if (!url.ToLower().Contains("https:"))
            {
                if (!url.ToLower().Contains("http:\\") && (!url.ToLower().Contains("http://")))
                {
                    if (url[0] == '\\' || url[0] == '/')
                    {
                        url = @"http:" + url; 
                    }
                    else
                    {
                       url = @"http://" + url; 
                    }
                }
            }

            if (url.ToLower().Contains('\\'))
            {
                url = url.Replace("\\", "//");
            }
            if (url.Contains("///"))
            {
                url = url.Replace("///", "//");
            }
            url = EndingSlash(CleanUrl(url));
            return url;
        }
    }
}