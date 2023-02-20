using NUnit.Framework;
using TestProject1.Base;
using System;
using Newtonsoft.Json;
using System.IO;
using Faker;
using System.Configuration;
using OfficeOpenXml;
using TestProject1.PageObjects;
using NUnit.Allure.Attributes;
using Allure.Net.Commons;

namespace TestProject1
{
    [Parallelizable(ParallelScope.Children)]
    public class Tests : BaseClass
    {
        [Test]
        public void Homework1()
        {
            LoginPage loginPage = new(driver.Value);
            loginPage.Login(ConfigurationManager.AppSettings["login"], ConfigurationManager.AppSettings["password"]);

            CoursePage coursePage = new(driver.Value);
            coursePage.WaitForSpinner();

            coursePage.VerifyLoggedIn("John Walker");

            coursePage.WaitFirstNavMenuLoaded();
            coursePage.VerifyColorNotRed(coursePage.DashboardMenuItem);
            coursePage.VerifyColorNotRed(coursePage.OrdersMenuItem);
            coursePage.VerifyColorNotRed(coursePage.ProductsMenuItem);
            coursePage.VerifyColorNotRed(coursePage.CustomersMenuItem);
            coursePage.VerifyColorNotRed(coursePage.ReportsMenuItem);
            coursePage.VerifyColorNotRed(coursePage.IntegrationsMenuItem);
            coursePage.VerifyColorNotRed(coursePage.CreateUsersMenuItem);
            coursePage.VerifyColorNotRed(coursePage.CreateManagerMenuItem);
            coursePage.VerifyColorNotRed(coursePage.CreateSubsMenuItem);
            coursePage.VerifyColorNotRed(coursePage.ListOfUsersMenuItem);
            coursePage.VerifyColorNotRed(coursePage.ListOfSubsMenuItem);
        }

        [Test]
        public void Homework2()
        {
            LoginPage loginPage = new(driver.Value);
            loginPage.Login(ConfigurationManager.AppSettings["login"], ConfigurationManager.AppSettings["password"]);

            CoursePage coursePage = new(driver.Value);
            coursePage.WaitForSpinner();
            coursePage.ScrollToBottom();
            coursePage.ClickCheckStatusButton();
            coursePage.CheckStatusChanged();

            Assert.IsTrue(coursePage.ButtonCheckStatus.Text == "Active");

            coursePage.IdHeader.Click();
            coursePage.TableSorting("id", "asc");
            coursePage.IdHeader.Click();
            coursePage.TableSorting("id", "desc");
            coursePage.NameHeader.Click();
            coursePage.TableSorting("name", "asc");
            coursePage.NameHeader.Click();
            coursePage.TableSorting("name", "desc");
            coursePage.AgeHeader.Click();
            coursePage.TableSorting("age", "asc");
            coursePage.AgeHeader.Click();
            coursePage.TableSorting("age", "desc");

        }

        [Test]
        public void Homework3CreatingUserFromJson()
        {
            LoginPage loginPage = new(driver.Value);

            var currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = Path.Combine(currentDirectory, "json1.json");
            var jsonString = File.ReadAllText(filePath);
            var user = JsonConvert.DeserializeObject<User>(jsonString);

            loginPage.Login(ConfigurationManager.AppSettings["login"], ConfigurationManager.AppSettings["password"]);

            CoursePage coursePage = new(driver.Value);
            coursePage.WaitForSpinner();

            // Verifying number of rows existed in table before 1st user creation
            coursePage.ListOfUsersMenuItem.Click();
            ListOfUsersPage listOfUsersPage = new(driver.Value);
            var rowsNumber = listOfUsersPage.Rows.Count;

            // Creating from JSON file
            coursePage.CreateUsersMenuItem.Click();
            CreateUserPage createUserPage = new(driver.Value);
            createUserPage.CreateUser(user);

            listOfUsersPage.WaitForTableLoaded();
            listOfUsersPage.VerifyUser(rowsNumber, user);
        }

        [Test]
        public void Homework3CreatingUserFromUserObj()
        {
            var user = new User()
            {
                Email = "email@email.com",
                Password = "password1234",
                Address1 = "123 Street",
                Address2 = "45",
                City = "City",
                Zip = "0001",
                AnnualPayment = "200",
                Description = "description description new line"
            };
            
            LoginPage loginPage = new(driver.Value);
            loginPage.Login(ConfigurationManager.AppSettings["login"], ConfigurationManager.AppSettings["password"]);

            CoursePage coursePage = new(driver.Value);
            coursePage.WaitForSpinner();

            // Verifying number of rows existed in table before 1st user creation
            coursePage.ListOfUsersMenuItem.Click();
            ListOfUsersPage listOfUsersPage = new(driver.Value);
            var rowsNumber = listOfUsersPage.Rows.Count;

            // Creating user using dictionary
            coursePage.CreateUsersMenuItem.Click();
            CreateUserPage createUserPage = new(driver.Value);
            createUserPage.CreateUser(user);
            listOfUsersPage.WaitForTableLoaded();
            listOfUsersPage.VerifyUser(rowsNumber, user);

        }

        [Test]
        public void Homework3CreatingUserWithFaker()
        {
            var user = new User()
            {
                Email = Internet.Email(),
                Password = Lorem.GetFirstWord(),
                Address1 = Address.StreetAddress(),
                Address2 = Address.SecondaryAddress(),
                City = Address.City(),
                Zip = Address.ZipCode(),
                AnnualPayment = RandomNumber.Next(1, 100).ToString(),
                Description = Lorem.Sentence(),
            };

            LoginPage loginPage = new(driver.Value);
            loginPage.Login(ConfigurationManager.AppSettings["login"], ConfigurationManager.AppSettings["password"]);

            CoursePage coursePage = new(driver.Value);
            coursePage.WaitForSpinner();

            // Verifying number of rows existed in table before 1st user creation
            coursePage.ListOfUsersMenuItem.Click();
            ListOfUsersPage listOfUsersPage = new(driver.Value);
            var rowsNumber = listOfUsersPage.Rows.Count;

            // Creating using Faker
            coursePage.CreateUsersMenuItem.Click();
            CreateUserPage createUserPage = new(driver.Value);
            createUserPage.CreateUser(user);
            listOfUsersPage.WaitForTableLoaded();
            listOfUsersPage.VerifyUser(rowsNumber, user);
        }

        [Test]
        public void Homework3CreatingUserFromXlsxFile()
        {
            var currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = Path.Combine(currentDirectory, "userXlsx.xlsx");

            LoginPage loginPage = new(driver.Value);
            loginPage.Login(ConfigurationManager.AppSettings["login"], ConfigurationManager.AppSettings["password"]);

            CoursePage coursePage = new(driver.Value);
            coursePage.WaitForSpinner();

            coursePage.ListOfUsersMenuItem.Click();
            ListOfUsersPage listOfUsersPage = new(driver.Value);
            var rowsNumber = listOfUsersPage.Rows.Count;
            coursePage.CreateUsersMenuItem.Click();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excel = new ExcelPackage(new FileInfo("userXlsx.xlsx")))
            {
                ExcelWorksheet worksheet = excel.Workbook.Worksheets["Sheet1"];
                User user = new User
                {
                    Email = worksheet.Cells[2, 1].Value.ToString(),
                    Password = worksheet.Cells[2, 2].Value.ToString(),
                    Address1 = worksheet.Cells[2, 3].Value.ToString(),
                    Address2 = worksheet.Cells[2, 4].Value.ToString(),
                    City = worksheet.Cells[2, 5].Value.ToString(),
                    Zip = worksheet.Cells[2, 6].Value.ToString(),
                    AnnualPayment = worksheet.Cells[2, 7].Value.ToString(),
                    Description = worksheet.Cells[2, 8].Value.ToString()
                };

                CreateUserPage createUserPage = new(driver.Value);
                createUserPage.CreateUser(user);
                listOfUsersPage.WaitForTableLoaded();
                listOfUsersPage.VerifyUser(rowsNumber, user);
            }
        }

        [Test]
        public void Homework3CreatingFirstUser()
        {
            LoginPage loginPage = new(driver.Value);
            loginPage.Login(ConfigurationManager.AppSettings["login"], ConfigurationManager.AppSettings["password"]);

            CoursePage coursePage = new(driver.Value);
            coursePage.WaitForSpinner();

            // Verifying number of rows existed in table before 1st user creation
            coursePage.ListOfUsersMenuItem.Click();
            ListOfUsersPage listOfUsersPage = new(driver.Value);
            var rowsNumber = listOfUsersPage.Rows.Count;

            //Create first user
            coursePage.CreateUsersMenuItem.Click();
            CreateUserPage createUserPage = new(driver.Value);
            createUserPage.CreateRandomUser();
            listOfUsersPage.WaitForTableLoaded();
            Assert.IsTrue(listOfUsersPage.IsNewRowAdded(rowsNumber));
        }
    }
}