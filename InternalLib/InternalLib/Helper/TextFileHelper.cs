/*########################################################
 *#  InternalLib.dll                                     #
 *#  Copyright 2018 by WesTex Enterprises                #
 *########################################################*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

//3rd party
using NLog;

namespace InternalLib
{
    public static class TextFileHelper
    {
        private readonly static Logger logger = LogManager.GetLogger("TextFileLoader");

        /// <summary>
        /// This is method, a file can be read in a text file
        /// </summary>
        /// <param name="file">Name of the file to load</param>
        /// <returns>The file as a string</returns>
        /// <exception>Will re-throw any exception it encounters</exception>
        static public string LoadTextFile(string file)
        {
            string result = string.Empty;
            //make sure the file exists
            if (File.Exists(file))
            {
                try
                {
                    result = File.ReadAllText(file);
                }
                catch (Exception e)
                {
                    logger.Error("Error loading text file: " + e);
                    throw;
                }
            }
            return result;
        }

        /// <summary>
        /// This is method a file can be read in to a list of strings
        /// </summary>
        /// <param name="file">Name of the file to load</param>
        /// <returns>The file as a string</returns>
        /// <exception>Will re-throw any exception it encounters</exception>
        static public List<string> TextFileToList(string file)
        {
            List<string> result = new List<string>();
            //make sure the file exists
            if (File.Exists(file))
            {
                try
                {
                    string data = File.ReadAllText(file);
                    string[] sdata = data.Replace('\n', ' ').Trim().Split('\r');
                    result.AddRange(sdata.Where(str => !string.IsNullOrEmpty(str)));
                }
                catch (Exception ex)
                {
                    logger.Error("Load text file error: " + ex);
                    throw new Exception(ex.Message);
                }
            }
            return result;
        }

        /// <summary>
        /// This is method a file can be read in to a list of strings
        /// </summary>
        /// <param name="file">Name of the file to load</param>
        /// <param name="encode">Type of encoding to use</param>
        /// <returns>The file as a string</returns>
        /// <exception>Will re-throw any exception it encounters</exception>
        static public List<string> TextFileToList(string file, Encoding encode)
        {
            List<string> result = new List<string>();
            //make sure the file exists
            if (File.Exists(file))
            {
                try
                {
                    string data = File.ReadAllText(file, encode);
                    string[] sdata = data.Replace('\n', ' ').Trim().Split('\r');
                    result.AddRange(sdata.Where(str => !string.IsNullOrEmpty(str)));
                }
                catch (Exception ex)
                {
                    logger.Error("Load text file error: " + ex);
                    throw new Exception(ex.Message);
                }
            }
            return result;
        }
    }
}