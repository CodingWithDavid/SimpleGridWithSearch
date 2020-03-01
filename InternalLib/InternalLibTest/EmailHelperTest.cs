/*########################################################
 *#  InternalLib.dll                                     #
 *#  Copyright 2018 by WesTex Enterprises                #
 *########################################################*/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

//local
using InternalLib;

namespace InteranlLibTest
{
    [TestClass]
    public class EmailHelperTest
    {
        [TestMethod]
        public void TestGetDomainFromEmail()
        {
            string UUT = EmailHelper.EmailDomain("you@me.com");

            IsTrue(UUT.Match("me"));
        }

        [TestMethod]
        public void TestGetDomainFromEmailWithExtraDots()
        {
            string UUT = EmailHelper.EmailDomain("you.you2@Castle.me");

            IsTrue(UUT.Match("Castle"));
        }
    }
}
