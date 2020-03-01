/*########################################################
 *#  InternalLib.dll                                     #
 *#  Copyright 2018 by WesTex Enterprises                #
 *########################################################*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

//3rd party
using NLog;

namespace InternalLib
{
    public static class XmlUtils
    {
        private const string strXmlUtils = "XmlUtils";
        private const string strRegExElementConstant = @"<([^>|^/]+)>";
        private const string strYyyyMMdd = "yyyy-MM-dd";
        private const string strConstant = "<.*?>(.*)</.*?>";

        private static readonly Logger logger;

        // this regex simply get the keys in the fragment
        private static readonly Regex xmlFragRegx;

        private static readonly Regex xmlRegex = new Regex(strConstant);
        private readonly static XmlReaderSettings fragmentSettings;

        static XmlUtils()
        {
            xmlFragRegx = new Regex(strRegExElementConstant, RegexOptions.Compiled);
            xmlRegex = new Regex(strConstant, RegexOptions.Compiled);
            logger = LogManager.GetLogger(strXmlUtils);
            fragmentSettings = new XmlReaderSettings
            {
                ConformanceLevel = ConformanceLevel.Fragment,
                IgnoreWhitespace = true,
                IgnoreComments = true
            };
        }

        /// <summary>
        ///     Returns a string containing the result of XML serialization
        ///     of the given object
        /// </summary>
        /// <param name="obj">The object to serialize</param>
        /// <returns>A string containing the XML</returns>
        public static string SerializeToXml(object obj)
        {
            string result = string.Empty;

            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            Stream output = new MemoryStream();
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                OmitXmlDeclaration = false
            };
            serializer.Serialize(XmlWriter.Create(output, settings), obj);
            result = StreamUtils.StreamToString(output);
            output.Close();

            return result;
        }

        public static string SerializeToXmlSaveSpaces(object obj)
        {
            string result = string.Empty;

            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            Stream output = new MemoryStream();
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                OmitXmlDeclaration = false,
                Indent = true
            };

            serializer.Serialize(XmlWriter.Create(output, settings), obj);
            result = StreamUtils.StreamToString(output);
            output.Close();

            return result;
        }

        /// <summary>
        /// Returns a string containing the result of XML serialization
        /// of the given object, with out the XML Heading
        /// </summary>
        /// <param name="obj">The object to serialize</param>
        /// <returns>A string containing the XML</returns>
        public static string SerializeToXmlClean(object obj)
        {
            string result = string.Empty;

            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            Stream output = new MemoryStream();
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                OmitXmlDeclaration = true
            };
            serializer.Serialize(XmlWriter.Create(output, settings), obj);
            result = StreamUtils.StreamToString(output);
            output.Close();

            return result;
        }

        /// <summary>
        /// Returns an object created from the result of XML deserialization
        /// </summary>
        /// <param name="xml">The XML to deserialize</param>
        /// <param name="type">
        ///     The data type of the object to create
        /// </param>
        /// <returns>The deserialized object</returns>
        public static object DeserializeFromXml(string xml, Type type)
        {
            XmlSerializer serializer = new XmlSerializer(type);
            object obj = serializer.Deserialize(new StringReader(xml));
            return obj;
        }

        /// <summary>
        /// De-serializes a serialized object from XML
        /// </summary>
        /// <typeparam name="T">The type of the object</typeparam>
        /// <param name="xml">The XML to deserialize</param>
        /// <returns>The deserialized object</returns>
        public static T DeserializeFromXml<T>(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(new StringReader(xml));
        }

        /// <summary>
        /// Substitute any special XML characters in the input string
        /// </summary>
        /// <param name="incoming">Input string to check</param>
        /// <returns>Reformatted string</returns>
        public static string ReplaceSpecialXmlCharacters(string incoming)
        {
            string a = incoming.Replace(@"&", "&amp;");
            string b = a.Replace(@"<", "&lt;");
            string c = b.Replace(@">", "&gt;");
            string d = c.Replace("\"", "&quot;");
            return d;
        }

        /// <summary>
        /// Validate the XML before deserializing the values for display name
        /// and tool tips
        /// </summary>
        public static string EscapeXMLfromString(string xml)
        {
            string result = "";
            //replace the &
            result = xml.Replace("&", "&amp;");
            //replace the '
            result = result.Replace("'", "&apos;");
            return result;
        }

        /// <summary>
        ///     Creates a string containing an XML-friendly date format
        /// </summary>
        /// <param name="dt">The Date/Time to convert</param>
        /// <returns>A string in the ISO date format, i.e. yyyy-mm-dd</returns>
        public static string CreateXmlDate(DateTime dt)
        {
            return dt.ToString(strYyyyMMdd);
        }

        ///  <summary>
        ///  Creates a typed dictionary from an XML fragment.  A fragment sample:
        ///  <key1>value</key1><key2>value</key2>
        ///  </summary>
        /// <returns>Dictionary string, string</returns>
        public static Dictionary<string, string> CreateDictionaryFromXmlFragment(string xmlFragment)
        {
            Dictionary<string, string> retval = new Dictionary<string, string>();
            MatchCollection matches = xmlFragRegx.Matches(xmlFragment);
            int count = matches.Count;
            for (int index = 0; index < count; index++)
            {
                string element = matches[index].Value.Replace("<", String.Empty);
                element = element.Replace(">", String.Empty);
                retval.Add(element, GetXmlValue(xmlFragment, element));
            }

            return retval;
        }

        /// <summary>
        /// this method will generate a dictionary from a XML fragment
        /// </summary>
        /// <param name="fragment">source XML</param>
        /// <returns>directory of the name values</returns>
        public static Dictionary<string, string> DictionaryFromXmlFragment(string fragment)
        {
            Dictionary<string, string> retval = new Dictionary<string, string>();
            try
            {
                using (TextReader tr = new StringReader(fragment))
                {
                    using (XmlReader reader = XmlReader.Create(tr, fragmentSettings))
                    {
                        while (!reader.EOF)
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                retval.Add(reader.Name, reader.ReadString());
                            }
                            else
                            {
                                reader.Read();
                            }
                        }
                    }
                    tr.Dispose();
                }
            }
            catch (XmlException)
            {
                // Some of the so-called XML is malformed.  Use a previous
                // implementation to parse the string
                retval = CreateDictionaryFromXml(fragment);
            }
            return retval;
        }

        /// <summary>
        /// this method will generate a dictionary from an XML string
        /// </summary>
        /// <param name="xml">source XML</param>
        /// <returns>dictionary of the results</returns>
        public static Dictionary<string, string> CreateDictionaryFromXml(string xml)
        {
            Match match = xmlRegex.Match(xml);
            Group group = match.Groups[1];
            return CreateDictionaryFromXmlFragment(group.Value);
        }

        /// <summary>
        /// Accepts a body consisting of XML tag and a target element and returns
        /// the value of the specified element
        /// </summary>
        /// <param name="messageBody">XML elements</param>
        /// <param name="element">a specific element</param>
        /// <returns>the value of the element</returns>
        public static string GetXmlValue(string messageBody, string element)
        {
            try
            {
                // "(?'Open'<watcherId>)(?'Value'.*)(?'Close'</watcherId>)"   - working sample
                Regex regx = new Regex("(?'Open'<" + element + @">)(?'Value'.*)(?'Close'</" + element + ">)");
                return regx.Match(messageBody).Groups["Value"].Value;
            }
            catch (Exception ex)
            {
                logger.Error(
                    "Could not find XML element " + element + " in body "
                    + messageBody + ": " + ex.Message + "\r\n" + ex.StackTrace);
                return String.Empty;
            }
        }

        /// <summary>
        /// This method will take a XML string and build a properly formatted phone number
        /// </summary>
        /// <param name="xml">XML String</param>
        /// <param name="node">XML Node</param>
        /// <param name="defaltValue">Set default value</param>
        /// <returns>phone found</returns>
        public static string ParseXmlNodeToPhoneString(string xml, string node, string defaltValue)
        {
            //Declare local variables
            string result = defaltValue;
            try
            {
                //Create and set XML document object
                XmlDocument document = new XmlDocument
                {
                    InnerXml = xml.Trim()
                };

                //Create XML node object
                XmlNode xmlNode = document.DocumentElement.SelectSingleNode(node);

                if (xmlNode != null)
                {
                    result = xmlNode.SelectSingleNode(node).InnerText;
                    //Replace unwanted chars
                    result = result.Replace("(", "");
                    result = result.Replace(")", "");
                    result = result.Replace(".", "");
                    result = result.Replace("-", "");
                    result = result.Replace("#", "");
                    result = result.Replace("+", "");
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error parsing XML for phone number: " + ex);
            }
            return result;
        }
    }
}