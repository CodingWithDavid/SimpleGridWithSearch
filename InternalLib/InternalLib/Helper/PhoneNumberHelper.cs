/*########################################################
 *#  InternalLib.dll                                     #
 *#  Copyright 2018 by WesTex Enterprises                #
 *########################################################*/

using System;
using System.Text.RegularExpressions;

//3rd party
using NLog;

namespace InternalLib
{
    public static class PhoneNumberHelper
    {
        private readonly static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Clean phone number
        /// </summary>
        /// <param name="phoneNumber">Number to clean</param>
        public static string CleanPhoneNumber(string phoneNumber)
        {
            string result = "";
            try
            {
                //Create string pattern
                string pattern = @"[\!\@\#\$\%\\^\&\*()\-\\_\+\=//\\\[\]\.\,\:\;//""\'\?\<\>\|\`\~]";

                //Create RegEx object
                Regex regex = new Regex(pattern);

                //Clean string
                result = regex.Replace(phoneNumber, "");

                result = result.Replace(" ", "");
            }
            catch (Exception ex)
            {
                logger.Error("Error processing cleaning a phone number : " + ex);
            }
            return result;
        }
    }
}