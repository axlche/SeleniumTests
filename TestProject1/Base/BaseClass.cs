using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Configuration;
using System.Threading;
namespace TestProject1.Base
{
    [TestFixture]
    public abstract class BaseClass
    {

        public ThreadLocal<IWebDriver> driver = new();

        [SetUp]
        public void StartBrowser()
        {
            //string browserName = ConfigurationManager.AppSettings["chrome"];
            driver.Value = new ChromeDriver();
            driver.Value.Navigate().GoToUrl(ConfigurationManager.AppSettings["url"]);
            driver.Value.Manage().Window.Maximize();

        }

        [TearDown]
        public void CloseBrowser()
        {
            driver.Value.Quit();
            driver.Value.Dispose();
        }


    }
}

