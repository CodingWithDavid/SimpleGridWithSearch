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
    public static class NameHelper
    {
        private readonly static Logger logger = LogManager.GetCurrentClassLogger();

        public static string GenerateName(string firstName, string lastName, DateTime date, int count, string mask, int pageNumber, int lineNumber)
        {
            string result = "";

            try
            {
                //Need to peel the mask apart into a collection of tokens
                Regex tokenRegEx = new Regex("<.*?>|[^<.*?>]|\\.");
                MatchCollection matches = tokenRegEx.Matches(mask);

                //Once get list of tokens, then need to determine what each token means and populate that section of the name.
                string workingToken = "";
                for (int i = 0; i < matches.Count; i++)
                {
                    string token = matches[i].ToString();

                    //Now we have the tokens, need to determine how to process them.
                    if (token.StartsWith("<") && token.EndsWith(">"))
                    {
                        //This is a special token and needs to be processed further, remove the braces from the string
                        workingToken = token.Replace("<", "");
                        workingToken = workingToken.Replace(">", "");

                        //next we need to determine if the token includes any integer data representing padding value.
                        Regex numberMatch = new Regex("[0-9*?]");
                        Match number = numberMatch.Match(workingToken);
                        int workingNumber = 0;

                        if (int.TryParse(number.Value, out int testValue))
                        {
                            workingNumber = testValue;
                        }

                        //remove the number from the token string.
                        workingToken = workingToken.Substring(0, workingToken.Length - number.Length).ToUpper();

                        switch (workingToken)
                        {
                            case "FI":
                                {
                                    if (firstName.IsNotEmpty())
                                    {
                                        result += firstName.Substring(0, 1);
                                    }
                                    break;
                                }
                            case "LI":
                                {
                                    if (lastName.IsNotEmpty())
                                    {
                                        result += lastName.Substring(0, 1);
                                    }
                                    break;
                                }
                            case "D":
                                {
                                    result += date.ToString();
                                    break;
                                }
                            case "DD":
                                {
                                    result += date.ToString("dd");
                                    break;
                                }
                            case "M":
                                {
                                    result += date.Month.ToString();
                                    break;
                                }
                            case "MM":
                                {
                                    result += date.ToString("MM");
                                    break;
                                }
                            case "YY":
                                {
                                    result += date.ToString("yy");
                                    break;
                                }
                            case "YYYY":
                                {
                                    result += date.ToString("yyyy");
                                    break;
                                }
                            case "SP":
                                {
                                    string format = "";
                                    for (int iCounter = 0; iCounter < workingNumber; iCounter++)
                                    {
                                        format += "0";
                                    }

                                    result += pageNumber.ToString(format);
                                    break;
                                }
                            case "SL":
                                {
                                    string szFormat = "";
                                    for (int iCounter = 0; iCounter < workingNumber; iCounter++)
                                    {
                                        szFormat += "0";
                                    }

                                    result += lineNumber.ToString(szFormat);
                                    break;
                                }
                            default:
                                {
                                    if (token.Contains("#"))
                                    {
                                        result += (count + 1).ToString().PadLeft(workingToken.Length, '0');
                                    }
                                    break;
                                }
                        }
                    }
                    else
                    {
                            result += token;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error generating the name: " + ex);
            }
            return result;
        }

    }
}
