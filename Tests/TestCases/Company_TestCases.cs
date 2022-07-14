using MusalaSoftAutomationTask.Common;
using MusalaSoftAutomationTask.Tests.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

namespace MusalaSoftAutomationTask.Tests.TestCases
{
    class Company_TestCases
    {
        IWebDriver driver;
        WebDriverWait wait;
        SharedSteps sharedSteps = new SharedSteps();
        Company_Page companyPage = new Company_Page();

        [SetUp]
        public void Setup()
        {
            driver = sharedSteps.InitiateBrowser(out wait);
        }


       

        [Test]
        public void ValidateCompanyTab()
        {
           
            sharedSteps.NavigateToIntialURL(driver);

            companyPage.NavigateToCompanyValidateURL(driver,wait);

            companyPage.ValidateLeadershipSection(driver);
            companyPage.OpenFaceBookLink(driver, wait);

        }


        [TearDown]
        public void TearDown()
        {
            sharedSteps.TearDown(driver);
        }
    }
}
