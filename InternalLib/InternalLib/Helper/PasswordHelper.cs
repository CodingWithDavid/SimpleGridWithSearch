/*########################################################
 *#  InternalLib.dll                                     #
 *#  Copyright 2018 by WesTex Enterprises                #
 *########################################################*/

using System.Text.RegularExpressions;

//3rd party
using NLog;

namespace InternalLib
{
    public class PasswordHelper
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Checks the password passed in to make sure the length is at least as long as given, 
        /// there are upper case characters, lower case characters, special character and number
        /// </summary>
        /// <param name="password"></param>
        /// <param name="minuminLength"></param>
        /// <returns>true if it passes, else false</returns>
        public static bool IsPasswordComplex(string password, int minuminLength)
        {
            bool result = false;
            if (password.Length >= minuminLength)
            {
                //check lower
                Regex rex1 = new Regex("[a-z]+");
                result = rex1.IsMatch(password);

                if (result)
                {
                    //check upper
                    rex1 = new Regex("[A-Z]+");
                    result = rex1.IsMatch(password);

                    if (result)
                    {
                        //check for digit
                        rex1 = new Regex("\\d+");
                        result = rex1.IsMatch(password);

                        if (result)
                        {
                            rex1 = new Regex("[-+_!@#$%^&*.,?]");
                            result = rex1.IsMatch(password);
                        }
                    }
                }
            }
            return result;
        }
    }
}
