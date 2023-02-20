using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TestProject1.Base;
using log4net;

namespace TestProject1.PageObjects
{
    public class CoursePage : BasePage
    {
        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region FirstNavMenu
        public IWebElement FirstNavMenu => FindElement(By.Id("first-nav-block"));
        public IWebElement DashboardMenuItem => FindElement(By.XPath("//a[normalize-space()='Dashboard']"));
        public IWebElement OrdersMenuItem => FindElement(By.XPath("//a[normalize-space()='Orders']"));
        public IWebElement ProductsMenuItem => FindElement(By.XPath("//a[normalize-space()='Products']"));
        public IWebElement CustomersMenuItem => FindElement(By.XPath("//a[normalize-space()='Customers']"));
        public IWebElement ReportsMenuItem => FindElement(By.XPath("//a[normalize-space()='Reports']"));
        public IWebElement IntegrationsMenuItem => FindElement(By.XPath("//a[normalize-space()='Integrations']"));
        public IWebElement CreateUsersMenuItem => FindElement(By.XPath("//a[normalize-space()='Create User']"));
        public IWebElement CreateManagerMenuItem => FindElement(By.XPath("//a[normalize-space()='Create Manager']"));
        public IWebElement CreateSubsMenuItem => FindElement(By.XPath("//a[normalize-space()='Create Subscription']"));
        public IWebElement ListOfUsersMenuItem => FindElement(By.XPath("//a[normalize-space()='List of users']"));
        public IWebElement ListOfSubsMenuItem => FindElement(By.XPath("//a[normalize-space()='List of Subscriptions']"));
        #endregion
        public IWebElement LblUser => FindElement(By.Id("user-label"));
        public IWebElement ButtonCheckStatus => FindElement(By.CssSelector("li.nav-item a.nav-link#status"));
        public IWebElement IdHeader => FindElement(By.CssSelector(".tabulator-col[tabulator-field='id']"));
        public IWebElement NameHeader => FindElement(By.CssSelector(".tabulator-col[tabulator-field='name']"));
        public IWebElement AgeHeader => FindElement(By.CssSelector(".tabulator-col[tabulator-field='age']"));
        public IList<IWebElement> Cells(string column)
        {
            return driver.FindElements(By.CssSelector($".tabulator-cell[tabulator-field='{column}']"));
        }

        public CoursePage(IWebDriver driver) : base(driver) { }

        public void VerifyLoggedIn(string userName)
        {
            WebDriverWait wait = new(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.TextToBePresentInElement(LblUser, userName));
            _log.Info("User logged in successfully!");
        }

        public void ScrollToBottom()
        {
            driver.FindElement(By.TagName("body")).SendKeys(Keys.End);
            Thread.Sleep(200);
            _log.Info("Page scrolled to the bottom");
        }

        public void WaitFirstNavMenuLoaded()
        {
            WebDriverWait wait = new(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementExists(By.Id("first-nav-block")));
            _log.Info("Left Navigation Menu Loaded");

        }

        public void ClickCheckStatusButton()
        {
            WebDriverWait wait = new(driver, TimeSpan.FromSeconds(25));
            wait.PollingInterval = TimeSpan.FromSeconds(5);
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("li.nav-item a.nav-link#status")));
            _log.Info("Button 'Check Status' displayed");
            ButtonCheckStatus.Click();
            _log.Info("Button 'Check Status' clicked successfully");
        }

        public void CheckStatusChanged()
        {
            WebDriverWait wait = new(driver, TimeSpan.FromSeconds(25));
            wait.PollingInterval = TimeSpan.FromSeconds(5);
            wait.Until(ExpectedConditions.TextToBePresentInElement(ButtonCheckStatus, "Loading.."));
            wait.Until(ExpectedConditions.TextToBePresentInElement(ButtonCheckStatus, "Active"));
            _log.Info("Status changed correct from Loading to Active");
        }

        public void VerifyColorNotRed(IWebElement element)
        {
            Actions action = new(driver);
            action.MoveToElement(element).Perform();
            Assert.IsTrue(element.GetCssValue("color") != "rgba(255,0,0,1)", "Color is red!");
            _log.Info($"{element} didn't change color to red on hovering");
        }

        public void WaitForSpinner()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id("spinner")));
            _log.Info("Spinner disappeared");
        }

        public void TableSorting(string column, string sortType)
        {
            IList<IWebElement> cells = driver.FindElements(By.CssSelector($".tabulator-cell[tabulator-field='{column}']"));
            IList<string> cellValues = new List<string>();
            IList<int> cellNums = new List<int>();
            foreach (IWebElement cell in cells)
            {
                if (cell.Text.Any(char.IsDigit) && int.TryParse(cell.Text, out int value))
                {
                    cellNums.Add(value);
                }
                else
                {
                    cellValues.Add(cell.Text);

                }
            }
            if (cellValues.Count > 0)
                VerifySorting(cellValues, sortType);
            else
                VerifyNumSorting(cellNums, sortType);
        }

        public void VerifySorting(IList<string> list, string sortType)
        {
            for (int i = 0; i < list.Count - 1; i++)
            {
                string currentValue = list[i];
                string nextValue = list[i + 1];
                CompareSortedValues(i, currentValue, nextValue, sortType);
            }
            _log.Info("Table sorted correctly");
        }

        public void VerifyNumSorting(IList<int> list, string sortType)
        {
            if (sortType == "asc")
                Assert.IsTrue(list.SequenceEqual(list.OrderBy(x => x)), "Column not sorted in ascending order");
            else
                Assert.IsTrue(list.SequenceEqual(list.OrderByDescending(x => x)), "Column not sorted in descending order");
            _log.Info("Table sorted correctly");
        }

        public void CompareSortedValues(int row, string currentValue, string nextValue, string sortType)
        {
            int comparison = String.Compare(currentValue, nextValue);
            StringBuilder assertMsg = new($"Sorting not correct on position {row}.");
            assertMsg
                .AppendLine($"Value on position {row}: {currentValue}.")
                .AppendLine($"Value on position {row + 1}: {nextValue}");
            if (sortType == "asc")
            {
                Assert.IsTrue(comparison <= 0, assertMsg.ToString());
            }
            else if (sortType == "desc")
            {
                Assert.IsTrue(comparison >= 0, assertMsg.ToString());
            }
        }

        
    }
}