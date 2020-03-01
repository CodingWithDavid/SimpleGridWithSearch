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
    public class MethodExtenstionsTest
    {
        [TestMethod]
        public void ToBinaryStringZeroTest()
        {
            int number = 0;
            string result = number.ToBinaryString();
            AreEqual("0", result);
        }

        [TestMethod]
        public void ToBinaryStringPositiveTest()
        {
            int number = 3;
            string result = number.ToBinaryString(7);
            AreEqual("0000011", result);
        }

        [TestMethod]
        public void ToBinaryStringMaxTest()
        {
            int number = 127;
            string result = number.ToBinaryString(7);
            AreEqual("1111111", result);
        }

        [TestMethod]
        public void ToBinaryStringOverMaxTest()
        {
            int number = 127;
            string result = number.ToBinaryString(3);
            AreEqual("1111111", result);
        }


        [TestMethod]
        public void ToBinaryIntPositiveTest()
        {
            string str = "0000011";
            int result = str.ToBinaryInt();

            IsTrue(3 == result);            
        }

        [TestMethod]
        public void ToBinaryIntZeroTest()
        {
            string str = "00000";
            int result = str.ToBinaryInt();

            IsTrue(0 == result);
        }

        [TestMethod]
        public void ToBinaryIntMaxTest()
        {
            string str = "1111111";
            int result = str.ToBinaryInt();

            IsTrue(127 == result);
        }

        [TestMethod]
        public void IsADatePositiveTest()
        {
            string str = "01/01/2014";
            IsTrue(str.IsADate());
        }

        [TestMethod]
        public void IsADateNegitiveTest()
        {
            string str = "abcd";
            IsFalse(str.IsADate());
        }

        [TestMethod]
        public void IsADateBlankTest()
        {
            string str = "";
            IsFalse(str.IsADate());
        }

        [TestMethod]
        public void IsADateNullTest()
        {
            string str = null;
            IsFalse(str.IsADate());
        }

        [TestMethod]
        public void IsADatePositiveDifferneetFormatTest()
        {
            string str = "2014/01/01";
            IsTrue(str.IsADate());
        }

        [TestMethod]
        public void ENumfromAIntegerTest()
        {
            string str = EnumParse.ParseEnum<Test>(2);
            IsTrue(str.Match("two"));
        }

        [TestMethod]
        public void ENumfromAIntegerEmptyReturnTest()
        {
            string str = EnumParse.ParseEnum<Test>(20);
            IsTrue(str.IsEmpty());
        }

        [TestMethod]
        public void ENumfromAStringTest()
        {
            AreEqual(Test.one, EnumParse.ParseEnum<Test>("one"));
        }

        [TestMethod]
        public void ENumfromAStringEmptyReturnTest()
        {
            try
            {
                int result = (int)EnumParse.ParseEnum<Test>("");
                IsTrue(false);
            }
            catch
            {
                IsTrue(true);
            }
        }

        [TestMethod]
        public void ToDecimalPositiveTest()
        {
            string str = "1.11";
            decimal result = str.ToDecimal();

            IsTrue(result == 1.11m);
        }

        [TestMethod]
        public void ToDecimalEmptyTest()
        {
            string str = "";
            decimal result = str.ToDecimal();

            IsTrue(result == 0m);
        }

        [TestMethod]
        public void ToDecimalZeroTest()
        {
            string str = "0";
            decimal result = str.ToDecimal();

            IsTrue(result == 0m);
        }

        [TestMethod]
        public void ToDecimalNegitiveTest()
        {
            string str = "-10";
            decimal result = str.ToDecimal();

            IsTrue(result == -10m);
        }

    }

    public enum Test
    {
        one = 1,
        two = 2,
        three = 3
    }
}
