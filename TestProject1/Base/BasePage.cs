using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace TestProject1.Base
{
    public class BasePage
    {
        protected IWebDriver driver;

        public BasePage(IWebDriver driver)
        {
            this.driver = driver;
        }
        public IWebElement FindElement(By by)
        {
            return driver.FindElement(by);
        }
        public IList<IWebElement> FindElements(By by)
        {
            return driver.FindElements(by);
        }
    }
}
