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
    public enum Statuses
    {
        Unknown = 0,
        New = 1,
        Processed = 2,
        Error = 3,
        Completed = 4,
        Queued = 5,
        Uploaded = 6,
        [System.ComponentModel.Description("Marked for Deletion")]
        MarkForDeleted = 7,
        [System.ComponentModel.Description("Ready for Download")]
        ReadyForDownload = 8,
    }

    [TestClass]
    public class EnumHelperTest
    {
        [TestMethod]
        public void GetDescriptionStringPositiveTest()
        {
            string result = EnumParse.GetDescription(Statuses.MarkForDeleted);
            IsTrue(result.Match("Marked for Deletion"));
        }

        [TestMethod]
        public void GetDefaultStringPositiveTest()
        {
            string result = EnumParse.GetDescription(Statuses.Queued);
            IsTrue(result.Match("Queued"));
        }
    }
}
