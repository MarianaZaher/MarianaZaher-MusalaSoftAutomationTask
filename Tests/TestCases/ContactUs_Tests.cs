
using MusalaSoftAutomationTask.Common;
using MusalaSoftAutomationTask.Tests.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

namespace MusalaSoftAutomationTask.Tests.TestCases
{
    public class ContactUs_Tests
    {
        IWebDriver driver;
        WebDriverWait wait;
        SharedSteps sharedSteps = new SharedSteps();
        ContactUs_Page contactUsPage;
       static ExcelHelper excelHelper = new ExcelHelper();

        [SetUp]
        public void Setup()
        {
            driver = sharedSteps.InitiateBrowser(out wait);
        }


        static List<string> rowData = excelHelper.GetExcelSheetData(2);

        [Test, TestCaseSource("rowData"), Parallelizable]
        public void InvalidEmailFormat(string email)
        { 
            contactUsPage = new ContactUs_Page()
            {name= "test",
              email= email,
              mobile=  "0123456789",
               subject= "test",
               message= "test"};
           
            sharedSteps.NavigateToIntialURL(driver);
           
                
            contactUsPage.ScrollToForm(driver, wait);

           
            contactUsPage.SendContactUsForm(driver, wait);

            Assert.IsTrue(sharedSteps.IsTextVisible(driver, "The e-mail address entered is invalid."));
            
        }


        [TearDown]
        public void TearDown()
        {
            sharedSteps.TearDown(driver);
        }
    }
}