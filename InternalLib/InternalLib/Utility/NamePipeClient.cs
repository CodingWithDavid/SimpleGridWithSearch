/*########################################################
 *#  InternalLib.dll                                     #
 *#  Copyright 2018 by WesTex Enterprises                #
 *########################################################*/

using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;

//3rd party
using NLog;

namespace InternalLib.Utility
{
    public static class NamePipeClient
    {
        private readonly static Logger logger = LogManager.GetCurrentClassLogger();

        public static void PostMessage(string pipeName, string payload)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    using (var client = new NamedPipeClientStream(pipeName))
                    {
                        client.Connect();
                        var data = Encoding.ASCII.GetBytes(payload);
                        client.Write(data, 0, data.Length);
                    }
                }
                catch { };
            });
        }
    }
}
