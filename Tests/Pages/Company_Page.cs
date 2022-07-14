using MusalaSoftAutomationTask.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusalaSoftAutomationTask.Tests.Pages
{
    class Company_Page
    {

    CommonFunctions commonFunctions = new CommonFunctions();


        public bool NavigateToCompanyValidateURL(IWebDriver Driver, WebDriverWait wait)
        {
           return commonFunctions.NavigateToTab(Driver,wait, "company");

        }

        public bool ValidateLeadershipSection(IWebDriver Driver)
        {
            return commonFunctions.IsSectionVisible(Driver, "Leadership");

        }

        public bool OpenFaceBookLink(IWebDriver Driver, WebDriverWait wait)
        {
            commonFunctions.ScrollTo(Driver, By.ClassName("links-buttons"));

            IWebElement element = Driver.FindElement(By.ClassName("links-buttons")).FindElements(By.TagName("a"))[3];
            commonFunctions.ClickOn(Driver, wait, element, true);

            commonFunctions.SwitchToWindowWithUrlContaining(Driver, "www.facebook.com");
            bool res=commonFunctions.ValidatePageURL(Driver, "https://www.facebook.com/MusalaSoft?fref=ts");
            return res;

        }


    }
}
