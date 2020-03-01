/*########################################################
 *#  InternalLib.dll                                     #
 *#  Copyright 2018 by WesTex Enterprises                #
 *########################################################*/

using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

//3rd party
using NLog;

namespace InternalLib
{
    static public class EmailHelper
    {
        private readonly static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// this method is used to send an email
        /// </summary>
        /// <param name="message">the body of the email</param>
        /// <param name="toEmail">who is the email going to</param>
        /// <param name="subject">subject of the email</param>
        /// <returns>true on success or false on an error</returns>
        public static bool SendEmail(string message, string toEmail, string subject, string smtpName, string smtpUser, string smtpPW, string fromWho, int smtpPort = 587)
        {
            bool result = false;
            StringBuilder body = new StringBuilder();
            body.Append(message + Environment.NewLine + Environment.NewLine);

            string to = toEmail;
            MailMessage email = new MailMessage(new MailAddress(fromWho), new MailAddress(to))
            {
                Subject = subject,
                Body = body.ToString()
            };
            SmtpClient smtp = new SmtpClient(smtpName)
            {
                Port = smtpPort,
                Credentials = new NetworkCredential(smtpUser, smtpPW)
            };
            try
            {
                smtp.Send(email);
                result = true;
            }
            catch (Exception ex)
            {
                logger.Error("Error attempting to send Support/Help Email: " + ex.Message);
            }
            email.Dispose();
            return result;
        }

        /// <summary>
        /// this method is used to send an email
        /// </summary>
        /// <param name="subject">Subject of the email</param>
        /// <param name="body">body of the email</param>
        /// <param name="emails">comma delimited list of who to send the email to</param>
        /// <param name="isBodyHtml">true if the body of the email is HTML</param>
        /// <param name="from">who is the email from</param>
        /// <returns></returns>
        static public bool SendEmail(string subject, string body, string emails, bool isBodyHtml, string from, string smtpName, string smtpUser, string smtpPW, int smtpPort = 587)
        {
            bool result = false;
            result = SendEmail(subject, body, emails, "", "", isBodyHtml, from, smtpName, smtpUser, smtpPW, smtpPort );
            return result;
        }

        /// <summary>
        /// this method is used to send an email
        /// </summary>
        /// <param name="subject">subject of the email</param>
        /// <param name="body">body of the email</param>
        /// <param name="emails">comma delimited list of who to send the email to</param>
        /// <param name="ccemails">comma delimited list of who carbon copy the email to</param>
        /// <param name="bcemails">comma delimited list of who to blind copy the email to</param>
        /// <param name="isBodyHtml">true if the body of the email is HTML</param>
        /// <param name="from">who is the email from </param>
        /// <returns>true if the email was sent, else false</returns>
        static public bool SendEmail(string subject, string body, string emails, string ccemails, string bcemails, bool isBodyHtml, string from, string smtpName, string smtpUser, string smtpPW, int smtpPort = 587)
        {
            logger.Debug("Enter Send Email");

            bool result = false;
            try
            {
                MailMessage oMsg = new MailMessage
                {
                    From = new MailAddress(from)
                };

                if (!emails.IsEmpty())
                    oMsg.To.Add(emails);
                if (!ccemails.IsEmpty())
                    oMsg.CC.Add(ccemails);
                if (!bcemails.IsEmpty())
                    oMsg.Bcc.Add(bcemails);

                oMsg.Subject = subject;
                oMsg.Body = body;
                oMsg.IsBodyHtml = isBodyHtml;
                SmtpClient smtp = new SmtpClient(smtpName)
                {
                    Port = smtpPort,
                    Credentials = new NetworkCredential(smtpUser, smtpPW)
                };

                logger.Debug("Send Email - " + emails + " : " + subject + " : " + body);

                smtp.Send(oMsg);
                
                result = true;
                oMsg.Dispose();
            }
            catch (Exception e)
            {
                logger.Error("Exception sending email " + emails + " because: " + e);
            }

            logger.Debug("Exiting Send Email - " + result);
            return result;
        }

        /// <summary>
        /// this method is used to send an email
        /// </summary>
        /// <param name="subject">subject of the email</param>
        /// <param name="body">body of the email</param>
        /// <param name="emails">comma delimited list of who to send the email to</param>
        /// <param name="ccemails">comma delimited list of who carbon copy the email to</param>
        /// <param name="from">who is the email from </param>
        /// <returns>true if the email was sent, else false</returns>
        static public bool SendHTMLEmail(string subject, string body, string emails, string ccemails, string from, string smtpServer, int port)
        {
            logger.Debug("Enter Send Email");
            bool result = false;
            try
            {
                MailMessage oMsg = new MailMessage
                {
                    From = new MailAddress(from)
                };

                if (!emails.IsEmpty())
                    oMsg.To.Add(emails);
                if (!ccemails.IsEmpty())
                    oMsg.CC.Add(ccemails);

                oMsg.Subject = subject;
                oMsg.Body = body;
                oMsg.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient(smtpServer)
                {
                    Port = port
                };
                smtp.Send(oMsg);
                result = true;
                oMsg.Dispose();
            }
            catch (Exception e)
            {
                logger.Error("Exception sending email " + emails + " because: " + e);
            }
            return result;
        }

        /// <summary>
        /// this method is used to send an email
        /// </summary>
        /// <param name="subject">subject of the email</param>
        /// <param name="body">body of the email</param>
        /// <param name="emails">comma delimited list of who to send the email to</param>
        /// <param name="ccemails">comma delimited list of who carbon copy the email to</param>
        /// <param name="from">who is the email from </param>
        /// <returns>true if the email was sent, else false</returns>
        static public bool SendHTMLEmail(string subject, string body, string emails, string ccemails, string from, string smtpServer, int port, string userName, string password)
        {
            logger.Debug("Enter Send Email");
            bool result = false;
            try
            {
                MailMessage oMsg = new MailMessage
                {
                    From = new MailAddress(from)
                };

                if (!emails.IsEmpty())
                    oMsg.To.Add(emails);
                if (!ccemails.IsEmpty())
                    oMsg.CC.Add(ccemails);

                oMsg.Subject = subject;
                oMsg.Body = body;
                oMsg.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient(smtpServer)
                {
                    Port = port,
                    Credentials = new NetworkCredential(userName, password)
                };
                smtp.Send(oMsg);
                result = true;
                oMsg.Dispose();
            }
            catch (Exception e)
            {
                logger.Error("Exception sending email " + emails + " because: " + e);
            }
            return result;
        }

        public static bool PostEmail(string subSystem, string subject, string body, string SentFrom, string emails, string postAPIUrl, string ccemails = "", string bcemails = "", string status = "New")
        {
            bool result = false;
            if (postAPIUrl.IsNotEmpty())
            {
                try
                {
                    string url = URLHelper.EndingSlash(postAPIUrl);
                    string data = "subSystem=" + subSystem + "&SentTo=" + emails + "&SentToCC=" + ccemails + "&SentToBCC=" + bcemails +
                                    "&SentFrom=" + SentFrom + "&body=" + HttpUtility.UrlEncode(body) + "&status=" + status;
                    HTTPPostHelper hph = new HTTPPostHelper();
                    hph.ExecutePostCommand(url, data);
                    result = true;
                }
                catch (Exception ex)
                {
                    logger.Error("Error calling the internal API for sending an email : " + ex);
                }
            }
            return result;
        }

        /// <summary>
        /// this method will apply a RegEx to a string and determine if it is a valid email address
        /// </summary>
        /// <param name="email">email to validate</param>
        /// <returns>true if the email is valid else false</returns>
        public static bool ValidateEmail(string email)
        {
            bool result = Regex.Match(email, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*").Success;
            return result;
        }

        public static string EmailDomain(string email)
        {
            string result = "";
            MailAddress address = new MailAddress(email);
            string host = address.Host;
            int counter = 1;
            for (int i = host.Length-1; i != 0; i--)
            {
                if(host[i] == '.')
                {
                    result = host.Substring(0, host.Length - counter);
                    break;
                }
                counter++;
            }
            return result;
        }
    }
}