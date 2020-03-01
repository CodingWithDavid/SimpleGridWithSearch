/*########################################################
 *#  InternalLib.dll                                     #
 *#  Copyright 2018 by WesTex Enterprises                #
 *########################################################*/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

//locals
using InternalLib;

namespace InteranlLibTest
{
    [TestClass]
    public class URLHelperTest
    {
        [TestMethod]
        public void FormatUrlURLPositiveTest()
        {
            string testString = "\\test.com";
            testString = URLHelper.FormatUrl(testString);

            AreEqual("http://test.com/", testString);
        }

        [TestMethod]
        public void FormatUrlURLForwardSlashPositiveTest()
        {
            string testString = "//test.com";
            testString = URLHelper.FormatUrl(testString);

            AreEqual("http://test.com/", testString);
        }

        [TestMethod]
        public void FormatUrlURLMissingEndingSlashTest()
        {
            string testString = "http://test.com";
            testString = URLHelper.FormatUrl(testString);

            AreEqual("http://test.com/", testString);
        }

        [TestMethod]
        public void FormatUrlURLHTTPSSlashTest()
        {
            string testString = "https://test.com";
            testString = URLHelper.FormatUrl(testString);

            AreEqual("https://test.com/", testString);
        }

        [TestMethod]
        public void FormatUrlURLHTTPSBackwardSlashTest()
        {
            string testString = "https:\\test.com";
            testString = URLHelper.FormatUrl(testString);

            AreEqual("https://test.com/", testString);
        }
    }
}
