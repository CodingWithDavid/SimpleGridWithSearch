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
    public class PasswordHelperTest
    {
        [TestMethod]
        public void ComplexPasswordPositiveTest()
        {
            string password = "abAB!@12";
            IsTrue(PasswordHelper.IsPasswordComplex(password, 8));
        }

        [TestMethod]
        public void ComplexPasswordToShortTest()
        {
            string password = "aA!@12";
            IsFalse(PasswordHelper.IsPasswordComplex(password, 8));
        }

        [TestMethod]
        public void ComplexPasswordNoNumbersTest()
        {
            string password = "abAB!@aa";
            IsFalse(PasswordHelper.IsPasswordComplex(password, 8));
        }

        [TestMethod]
        public void ComplexPasswordNoSpecialTest()
        {
            string password = "abABaa12";
            IsFalse(PasswordHelper.IsPasswordComplex(password, 8));
        }

        [TestMethod]
        public void ComplexPasswordNoUpperCaseTest()
        {
            string password = "abab!@12";
            IsFalse(PasswordHelper.IsPasswordComplex(password, 8));
        }

        [TestMethod]
        public void ComplexPasswordNoLowerTest()
        {
            string password = "ABAB!@12";
            IsFalse(PasswordHelper.IsPasswordComplex(password, 8));
        }
    }
}
