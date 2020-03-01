/*########################################################
 *#  InternalLib.dll                                     #
 *#  Copyright 2018 by WesTex Enterprises                #
 *########################################################*/

using System;

//local

//3rd party
using Newtonsoft.Json;
using NLog;

namespace InternalLib.Helper
{
    public static class JsonHelper
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public static T Deserialize<T>(string input) where T : class
        {
            if (input.IsNotEmpty())
            {
                try
                {
                    return JsonConvert.DeserializeObject<T>(input);
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    return (T)Activator.CreateInstance(typeof(T));
                }
            }
            else
            {
                return (T)Activator.CreateInstance(typeof(T));
            }
        }

        public static string Serialize<T>(T input) where T : class
        {
            if (input != null)
            {
                try
                {
                    return JsonConvert.SerializeObject(input);
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    return "";
                }
            }
            else
            {
                return "";
            }
        }
    }
}
