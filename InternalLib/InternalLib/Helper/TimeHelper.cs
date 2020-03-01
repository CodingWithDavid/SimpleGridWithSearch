/*########################################################
 *#  InternalLib.dll                                     #
 *#  Copyright 2018 by WesTex Enterprises                #
 *########################################################*/

using System;

//3rd party
using NLog;

namespace InternalLib
{
    public static class TimeHelper
    {
        private readonly static Logger logger = LogManager.GetCurrentClassLogger();

        public static string FormattedDuration(int hours, int minutes, int seconds, int milliseconds)
        {
                string returnVal = string.Empty;
                try
                {
                    returnVal = string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D3}", hours, minutes, seconds, milliseconds);
                }
                catch (Exception exception)
                {
                    logger.Error(exception);
                }
                return returnVal;
        }

        public static string FormattedDuration(double totalSeconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(totalSeconds);
            string result = time.ToString(@"hh\:mm\:ss\:fff");
            return result;
        }
    }
}

