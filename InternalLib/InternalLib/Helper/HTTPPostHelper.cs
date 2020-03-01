/*########################################################
 *#  InternalLib.dll                                     #
 *#  Copyright 2018 by WesTex Enterprises                #
 *########################################################*/

using System.IO;
using System.Net;
using System.Text;

namespace InternalLib
{
    public class HTTPPostHelper
    {
        public string ExecutePostCommon(string url, string data)
        {
            string result = string.Empty;
            result = ExecutePostCommand(url, "administrator", "12345", data);
            //result = "called";
            return result;
        }

        /// <summary>
        /// Executes an HTTP POST command and retrieves the information.
        /// This function will automatically include a "source" parameter if the "Source" property is set.
        /// </summary>
        /// <param name="url">The URL to perform the POST operation</param>
        /// <param name="userName">The user-name to use with the request</param>
        /// <param name="password">The password to use with the request</param>
        /// <param name="data">The data to post</param>
        /// <returns>The response of the request, or null if we got 404 or nothing.</returns>
        public string ExecutePostCommand(string url, string userName, string password, string data)
        {
            WebRequest request = WebRequest.Create(url);
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                request.Credentials = new NetworkCredential(userName, password);
                request.ContentType = "application/x-www-form-urlencoded";
                request.Method = "POST";

                System.Net.ServicePointManager.Expect100Continue = false;

                byte[] bytes = Encoding.UTF8.GetBytes(data);

                request.ContentLength = bytes.Length;
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(bytes, 0, bytes.Length);

                    using (WebResponse response = request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Executes a post command
        /// </summary>
        /// <param name="url">HTTP URL to post to</param>
        /// <param name="data">what data to post</param>
        /// <returns></returns>
        public string ExecutePostCommand(string url, string data)
        {
            return ExecutePostCommand(url, "user", "password", data);
        }

        /// <summary>
        /// executes a get HTTP method
        /// </summary>
        /// <param name="url">URL to get from</param>
        /// <returns>data returned from the get as a string</returns>
        public string ExecuteGetCommand(string url)
        {
            return ExecuteGetCommand(url, "user", "password");
        }

        /// <summary>
        /// Executes an HTTP GET command and retrieves the information.
        /// </summary>
        /// <param name="url">The URL to perform the GET operation</param>
        /// <param name="userName">The user-name to use with the request</param>
        /// <param name="password">The password to use with the request</param>
        /// <returns>The response of the request, or null if we got 404 or nothing.</returns>
        //Ignoring Code Analysis because we are helping the user with the call.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2200:RethrowToPreserveStackDetails")]
        public string ExecuteGetCommand(string url, string userName, string password)
        {
            using (WebClient client = new WebClient())
            {
                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
                {
                    client.Credentials = new NetworkCredential(userName, password);
                }

                try
                {
                    using (Stream stream = client.OpenRead(url))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
                catch (WebException ex)
                {
                    //
                    // Handle HTTP 404 errors gracefully and return a null string to indicate there is no content.
                    //
                    if (ex.Response is HttpWebResponse)
                    {
                        if ((ex.Response as HttpWebResponse).StatusCode == HttpStatusCode.NotFound)
                        {
                            return null;
                        }
                    }

                    throw ex;
                }
            }
        }

    }


}