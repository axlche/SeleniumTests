using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TestProject1.Base;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using log4net;

namespace TestProject1.PageObjects
{
    public class ListOfUsersPage : BasePage
    {
        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public IList<IWebElement> Rows => FindElements(By.CssSelector(".tabulator-row"));
        public IWebElement UsersTable => FindElement(By.Id("users-table"));
        public IWebElement PageHeader => FindElement(By.CssSelector("div[class='col-9'] h3]"));
        public IWebElement EmailHeader => FindElement(By.CssSelector(".tabulator-col[tabulator-field='email'"));
        public IWebElement ColumnCell(string column) => FindElement(By.CssSelector($".tabulator-cell[tabulator-field='{column}']"));

        
        public ListOfUsersPage(IWebDriver driver) : base(driver) { }

        public bool IsPageLoaded() => PageHeader.Displayed;

        public bool IsTableLoaded() => UsersTable.Displayed;

        public bool IsNewRowAdded(int rowsCount) => FindElement(By.CssSelector($".tabulator-row:nth-child({rowsCount + 1})")).Displayed;
        

        public void WaitForTableLoaded()
        {
            WebDriverWait wait = new(driver, TimeSpan.FromSeconds(25));
            wait.PollingInterval = TimeSpan.FromSeconds(5);
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("users-table")));
            _log.Info("Table loaded successfully.");
        }

        public void VerifyUser(int rowsCount, User user)
        {
            var role = "user";
            var state = " ";
            var demo = " ";
            var waitForSupervisor = " ";
            var managerType = " ";

            Assert.AreEqual(user.Email, FindElement(By.CssSelector($".tabulator-row:nth-child({rowsCount + 1}) .tabulator-cell:nth-child(1)")).Text, "Email are not equal.");
            Assert.AreEqual(role, FindElement(By.CssSelector($".tabulator-row:nth-child({rowsCount + 1}) .tabulator-cell:nth-child(2)")).Text, "Role are not equal.");
            Assert.AreEqual(user.Address1, FindElement(By.CssSelector($".tabulator-row:nth-child({rowsCount + 1}) .tabulator-cell:nth-child(3)")).Text, "Address1 are not equal.");
            Assert.AreEqual(user.Address2, FindElement(By.CssSelector($".tabulator-row:nth-child({rowsCount + 1}) .tabulator-cell:nth-child(4)")).Text, "Address2 are not equal.");
            Assert.AreEqual(user.City, FindElement(By.CssSelector($".tabulator-row:nth-child({rowsCount + 1}) .tabulator-cell:nth-child(5)")).Text, "City are not equal.");
            Assert.AreEqual(state, FindElement(By.CssSelector($".tabulator-row:nth-child({rowsCount + 1}) .tabulator-cell:nth-child(6)")).Text, "State are not equal.");
            Assert.AreEqual(user.Zip, FindElement(By.CssSelector($".tabulator-row:nth-child({rowsCount + 1}) .tabulator-cell:nth-child(7)")).Text, "Zip are not equal.");
            Assert.AreEqual(user.Description, FindElement(By.CssSelector($".tabulator-row:nth-child({rowsCount + 1}) .tabulator-cell:nth-child(8)")).Text, "Description are not equal.");
            Assert.AreEqual(demo, FindElement(By.CssSelector($".tabulator-row:nth-child({rowsCount + 1}) .tabulator-cell:nth-child(9)")).Text, "Demo are not equal.");
            Assert.AreEqual(user.AnnualPayment, FindElement(By.CssSelector($".tabulator-row:nth-child({rowsCount + 1}) .tabulator-cell:nth-child(10)")).Text, "Annual Payment are not equal.");
            Assert.AreEqual(waitForSupervisor, FindElement(By.CssSelector($".tabulator-row:nth-child({rowsCount + 1}) .tabulator-cell:nth-child(11)")).Text, "Wait for supervisor are not equal.");
            Assert.AreEqual(managerType, FindElement(By.CssSelector($".tabulator-row:nth-child({rowsCount + 1}) .tabulator-cell:nth-child(12)")).Text, "Manager type are not equal.");
            _log.Info("Created user displays in the table with correct data.");

        }
    }
}
