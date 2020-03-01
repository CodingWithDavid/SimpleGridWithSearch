/*########################################################
 *#  InternalLib.dll                                     #
 *#  Copyright 2018 by WesTex Enterprises                #
 *########################################################*/

using System;
using System.IO;
using System.Text;

//3rd party
using NLog;

namespace InternalLib
{
    public static class StreamUtils
    {
        private readonly static Logger logger = LogManager.GetLogger("StreamUtils");

        /// <summary>
        ///  Takes a Path, and option appendage text, and reads the target document
        ///  back as a single string
        /// </summary>
        /// <param name="text">Option text to append to each line read</param>
        /// <param name="path">Location of the document</param>
        /// <returns></returns>
        public static string GetTextFromPath(string text, string path)
        {
            StringBuilder sb = new StringBuilder();
            using (StreamReader sr = new StreamReader(path))
            {
                while (sr.Peek() >= 0)
                {
                    sb.Append(sr.ReadLine());
                    sb.Append(text);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Reads data from a stream until the end is reached. The
        /// data is returned as a byte array. An IOException is
        /// thrown if any of the underlying IO calls fail.
        /// </summary>
        /// <param name="stream">The stream to read data from</param>
        /// <param name="initialLength">The initial buffer length</param>
        public static byte[] ReadFully(Stream stream, long initialLength)
        {
            // reset pointer just in case
            stream.Seek(0, SeekOrigin.Begin);

            // If we've been passed an unhelpful initial length, just
            // use 32K.
            if (initialLength < 1)
            {
                initialLength = 32768;
            }

            byte[] buffer = new byte[initialLength];
            int read = 0;

            int chunk;
            while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
            {
                read += chunk;

                // If we've reached the end of our buffer, check to see if there's
                // any more information
                if (read == buffer.Length)
                {
                    int nextByte = stream.ReadByte();

                    // End of stream? If so, we're done
                    if (nextByte == -1)
                    {
                        return buffer;
                    }

                    // Nope. Resize the buffer, put in the byte we've just
                    // read, and continue
                    byte[] newBuffer = new byte[buffer.Length * 2];
                    Array.Copy(buffer, newBuffer, buffer.Length);
                    newBuffer[read] = (byte)nextByte;
                    buffer = newBuffer;
                    read++;
                }
            }
            // Buffer is now too big. Shrink it.
            byte[] ret = new byte[read];
            Array.Copy(buffer, ret, read);
            return ret;
        }

        /// <summary>
        /// Streams to string.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static string StreamToString(Stream stream)
        {
            if (stream == null)
            {
                return string.Empty;
            }

            stream.Seek(0, SeekOrigin.Begin);
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// this method will read in a file into a byte array
        /// </summary>
        /// <param name="filePath">source file</param>
        /// <returns>byte array</returns>
        public static byte[] FileToByteArray(string filePath)
        {
            byte[] result = null;

            try
            {
                result = File.ReadAllBytes(filePath);
            }
            catch (Exception ex)
            {
                logger.Debug("Error attempting to open file: " + filePath + "\r\n" + ex.Message);
            }

            return result;
        }
    }
}