using log4net;
using OpenQA.Selenium;
using TestProject1.Base;

namespace TestProject1.PageObjects
{
    public class LoginPage : BasePage
    {
        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public IWebElement LoginField => FindElement(By.Id("login"));
        public IWebElement PasswordField => FindElement(By.Id("password"));
        public IWebElement BtnLogin => FindElement(By.CssSelector(".btn-primary.rounded-2"));


        public LoginPage(IWebDriver driver) : base(driver) { }


        public void Login(string login, string password)
        {
            LoginField.SendKeys(login);
            PasswordField.SendKeys(password);
            BtnLogin.Click();
            _log.Info("Successful login");
        }
    }
}
