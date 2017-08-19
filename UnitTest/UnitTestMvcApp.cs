using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcApp.Controllers;
using System.Web.Mvc;

namespace UnitTest
{
    [TestClass]
    public class UnitTestMvcApp
    {
        HomeController home = new HomeController();
        [TestMethod]
        public void TestMethod1()
        {
            var idx = home.Index() as ViewResult;
            string str = idx.ToString();
            Assert.AreEqual(str, "This is Home Page.");
        }
    }
}
