using MusalaSoftAutomationTask.Common;
using MusalaSoftAutomationTask.Tests.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusalaSoftAutomationTask.Tests.TestCases
{


    class Careers_TestCases
    {
        IWebDriver driver;
        WebDriverWait wait;
        SharedSteps sharedSteps = new SharedSteps();
        Careers_Page careerPage ;

        static ExcelHelper excelHelper = new ExcelHelper();


        [SetUp]
        public void Setup()
        {
            driver = sharedSteps.InitiateBrowser(out wait);
        }




        static List<string> rowData = excelHelper.GetExcelSheetData(2);

        [Test, TestCaseSource("rowData"), Parallelizable]
        public void ApplyForAutomationQAEngineerPosition(string email)
        {
            careerPage = new Careers_Page()
            {
                name= "test",
                mobile= "012345678",
                email= email,
                position= "Automation QA Engineer",
                location= "Sofia",
              sections= new string[] { "General Description", "Requirements", "Responsibilities", "What we offer" }
            };

            sharedSteps.NavigateToIntialURL(driver);

            careerPage.NavigateToCareers(driver, wait);

            careerPage.NavigateToOpenPositions(driver, wait);
            careerPage.SelectPositionDetails(driver, wait);
            careerPage.ClickToApplyForPosition(driver, wait);
            careerPage.ApplyForCareerForm(driver, wait);
           string response= careerPage.ValidateResponseMessage(driver, wait);

            Assert.IsTrue(response.Contains( "One or more fields have an error"));

        }


        [Test]
        [TestCase("Skopje")]
        [TestCase("Sofia")]
        public void FilterAvailablePositionsByCities(string city)
        {
            careerPage = new Careers_Page()
            {
                
                location = city
            };

            sharedSteps.NavigateToIntialURL(driver);

            careerPage.NavigateToCareers(driver, wait);

            careerPage.NavigateToOpenPositions(driver, wait);
             careerPage.GetPositionByCity(driver, wait);



        }

        [TearDown]
        public void TearDown()
        {
            sharedSteps.TearDown(driver);
        }
    }
}
