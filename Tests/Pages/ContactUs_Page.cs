using MusalaSoftAutomationTask.Common;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusalaSoftAutomationTask.Tests.Pages
{
    class ContactUs_Page
    {
        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("email")]
        public string email { get; set; }

        [JsonProperty("mobile")]
        public string mobile { get; set; }

        [JsonProperty("subject")]
        public string subject { get; set; }
        [JsonProperty("message")]
        public string message { get; set; }

        CommonFunctions commonFunctions = new CommonFunctions();
        public void ScrollToForm(IWebDriver Driver, WebDriverWait wait)
        {
            commonFunctions.ScrollTo(Driver, By.ClassName("links-buttons"));

            IWebElement element = Driver.FindElement(By.ClassName("links-buttons")).FindElements(By.TagName("a"))[0];
            commonFunctions.ClickOn(Driver, wait, element, true);
            commonFunctions.IsElementPresent(Driver, By.Id("contact_form_pop"));

        }

        public void SendContactUsForm(IWebDriver Driver, WebDriverWait wait)
        {

            commonFunctions.SendKeys(Driver, wait, By.Id("cf-1"), name);
            commonFunctions.SendKeys(Driver, wait, By.Id("cf-2"), email);
            commonFunctions.SendKeys(Driver, wait, By.Id("cf-3"), mobile);
            commonFunctions.SendKeys(Driver, wait, By.Id("cf-4"), subject);
            commonFunctions.SendKeys(Driver, wait, By.Id("cf-5"), message);

            commonFunctions.ClickElement(Driver, wait, By.XPath("//input[@type='submit']"));

        }


        public string ReadEmailAlertMsg(IWebDriver Driver)
        {
          IWebElement AlertElement=  commonFunctions.GetParentElement(Driver, By.Id("cf-2")).FindElements(By.ClassName("wpcf7-not-valid-tip"))[0];
            return commonFunctions.GetTextOf(Driver, AlertElement);

        }


    }
}
