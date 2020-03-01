/*########################################################
 *#  InternalLib.dll                                     #
 *#  Copyright 2018 by WesTex Enterprises                #
 *########################################################*/

using System;

namespace InternalLib
{
    public class DateUtility
    {
        /// <summary>
        /// Is the birthDate older than the cut off time frame in years
        /// </summary>
        /// <param name="birthDate">Date of birth</param>
        ///<param name="cutOffAge">Number of years they need to be old</param>
        /// <returns></returns>
        public static bool OldEnough(DateTime birthDate, int cutOffAge)
        {
            bool result = false;

            //get the number of days since Cut off
            int years = DateTime.Now.Year - birthDate.Year;
            if (years > cutOffAge)
            {
                result = true;
            }
            else if (years == cutOffAge)
            {
                //is the birthrate before now
                if (DateTime.Now.Month > birthDate.Month)
                {
                    result = true;
                }
                else if (DateTime.Now.Month == birthDate.Month)
                {
                    if (DateTime.Now.Day > birthDate.Day)
                    {
                        result = true;
                    }
                    else if (DateTime.Now.Day == birthDate.Day)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// this method will checked if the string passed in is a valid date
        /// </summary>
        /// <param name="inDate">source date</param>
        /// <param name="future">if true, the date must be in the future</param>
        /// <param name="errorMessage">if there was an error, this is the error message</param>
        /// <returns>true if the date is a valid date and pass the future check</returns>
        public static bool IsValidDate(string inDate, bool future, out string errorMessage)
        {
            bool result = true;
            errorMessage = "";
            if (!inDate.IsEmpty())
            {
                DateTime tmp = new DateTime();
                if (DateTime.TryParse(inDate, out tmp))
                {
                    if (future)
                    {
                        if (tmp.Ticks < DateTime.Now.Ticks)
                        {
                            errorMessage = "Date can not be in the past";
                            result = false;
                        }
                    }
                }
                else
                {
                    errorMessage = "A valid date must be entered";
                    result = false;
                }
            }
            else
            {
                errorMessage = "A valid date must be entered";
                result = false;
            }
            return result;
        }

        /// <summary>
        /// this method will check the date to see if the string passed in is a valid US date
        /// </summary>
        /// <param name="datestring">source string</param>
        /// <returns>true if it is a US string</returns>
        public static bool IsUSDate(ref string datestring)
        {
            string[] fmts = { "MM/dd/yyyy", "M/dd/yyyy", "MM/dd/yy", "M/dd/yy", "MM/d/yyyy", "M/d/yyyy",
								"MM/dd/yyyy hh:mm:ss", "M/dd/yyyy  hh:mm:ss", "MM/dd/yy hh:mm:ss", "M/dd/yy hh:mm:ss", "MM/d/yyyy hh:mm:ss", "M/d/yyyy hh:mm:ss"
							};
            bool bReturn = false;

            try
            {
                DateTime dDate = new DateTime();
                if (DateTime.TryParse(datestring, out dDate))
                {
                    datestring = DateTime.ParseExact(datestring, fmts, null,
                        System.Globalization.DateTimeStyles.AllowWhiteSpaces).ToString("yyyy-MM-dd hh:mm:ss");
                    bReturn = true;
                }
            }
            catch
            {
                // Not a date with the specified format, ignore the exception
            }
            return bReturn;
        }
    }
}