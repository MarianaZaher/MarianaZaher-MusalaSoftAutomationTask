using MusalaSoftAutomationTask.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusalaSoftAutomationTask.Tests.TestCases
{
    
    class SharedSteps
    {
CommonFunctions commonFunctions = new CommonFunctions();
        public IWebDriver InitiateBrowser(out WebDriverWait wait)
        {
            // initiate browser 
            return commonFunctions.SetupDriver(out wait);
        }
        public void NavigateToIntialURL(IWebDriver Driver)
        {
            commonFunctions.GoToUrl(Driver);
        }


      
        public  bool IsTextVisible(IWebDriver Driver, string text)
        {

            try
            {
                return commonFunctions.IsTextVisible(Driver, text);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void TearDown(IWebDriver Driver)

        {
            commonFunctions.CloseDriver(Driver);
        }
    }
}
