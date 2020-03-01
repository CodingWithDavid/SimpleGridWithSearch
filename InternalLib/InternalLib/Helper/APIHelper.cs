/*########################################################
 *#  InternalLib.dll                                     #
 *#  Copyright 2018 by WesTex Enterprises                #
 *########################################################*/

using System;
using System.Collections.Generic;
using System.Linq;

//locals
using InternalLib;

//3rd party
using NLog;

namespace InternalLib.Helper
{
    public static class APIHelper
    {
        private readonly static Logger logger = LogManager.GetCurrentClassLogger();

        public static string Get(string apiURL, string APICallingId, string apiCall, List<string> data)
        {
            string result = "";
            try
            {
                HTTPPostHelper hph = new HTTPPostHelper();
                string url = URLHelper.EndingSlash(apiURL) + URLHelper.EndingSlash(apiCall) + URLHelper.EndingSlash(APICallingId) + "?" + URLPassFormat(data);
                result = hph.ExecuteGetCommand(url);
            }
            catch (Exception ex)
            {
                logger.Error("Error calling the Software database API to " + apiCall + " with data : " + String.Join<string>(String.Empty, data) + " because: " + ex);
            }
            return result;
        }

        public static string Put(string apiURL, string APICallingId, string apiCall, List<string> data)
        {
            string result = "";
            try
            {
                HTTPPostHelper hph = new HTTPPostHelper();
                string url = URLHelper.EndingSlash(apiURL) + URLHelper.EndingSlash(apiCall) + APICallingId;
                result = hph.ExecutePostCommand(url, URLPassFormat(data));
            }
            catch (Exception ex)
            {
                logger.Error("Error calling the Software database API to " + apiCall + " with data : " + data + " because: " + ex);
            }
            return result;
        }

        private static string URLPassFormat(List<string> data)
        {
            string result = "";
            int i = 0;
            if (data.Any())
            {
                do
                {
                    result += data[i++] + "&";
                }
                while (i < data.Count);
            }
            return result;
        }
    }
}
