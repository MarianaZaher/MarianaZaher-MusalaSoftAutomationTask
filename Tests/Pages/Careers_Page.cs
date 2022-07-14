using MusalaSoftAutomationTask.Common;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusalaSoftAutomationTask.Tests.Pages
{
    class Careers_Page
    {

        CommonFunctions commonFunctions = new CommonFunctions();

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("email")]
        public string email { get; set; }

        [JsonProperty("mobile")]
        public string mobile { get; set; }
        [JsonProperty("position")]
        public string position { get; set; }
        [JsonProperty("location")]
        public string location { get; set; }
        [JsonProperty("sections")]
        public string[] sections { get; set; }


        public bool NavigateToCareers(IWebDriver Driver, WebDriverWait wait)
        {
            return commonFunctions.NavigateToTab(Driver, wait, "careers");

        }

        public bool NavigateToOpenPositions(IWebDriver Driver, WebDriverWait wait)
        {
            return commonFunctions.NavigateToTab(Driver, wait, "careers/join-us");

        }

        public bool SelectPositionDetails(IWebDriver Driver, WebDriverWait wait)
        {
            commonFunctions.SelectByValue(Driver, By.Id("get_location"), location);
            commonFunctions.ClickOn(Driver, wait, By.XPath($"//h2[contains(text(), '{position }')]"), true);
            bool sectionsShown = true;
            foreach (string section in sections)
            {
                if (!commonFunctions.IsSectionVisible(Driver, section))
                    sectionsShown = false;
            }
            return sectionsShown;
        }

        public void ClickToApplyForPosition(IWebDriver Driver, WebDriverWait wait)
        {

            if (commonFunctions.IsButtonVisible(Driver, "Apply"))
            {

                commonFunctions.ClickOn(Driver, wait, By.XPath($"//input[contains(@value, 'Apply')]"), true);

                commonFunctions.WaitTillElementIsDisplayed(Driver.FindElement(By.Id("fancybox-content")), wait);


            }
            // else
            // log exception


        }
        public string ValidateResponseMessage(IWebDriver Driver, WebDriverWait wait)
        {

            commonFunctions.WaitForPageReadyState(Driver);

            commonFunctions.WaitTillElementIsDisplayed(Driver.FindElement(By.ClassName("wpcf7-response-output")), wait);

            return commonFunctions.GetTextOf(Driver, By.ClassName("wpcf7-response-output"));


        }
        public void ApplyForCareerForm(IWebDriver Driver, WebDriverWait wait)
        {

            commonFunctions.SendKeys(Driver, wait, By.Id("cf-1"), name);
            commonFunctions.SendKeys(Driver, wait, By.Id("cf-2"), email);
            commonFunctions.SendKeys(Driver, wait, By.Id("cf-3"), mobile);

            commonFunctions.UploadFile(Driver, By.Id("cf-4"));

            commonFunctions.ClickElement(Driver, wait, By.Id("adConsentChx"));

            commonFunctions.ClickElement(Driver, wait, By.XPath("//input[@type='submit']"));

        }


        public void GetPositionByCity(IWebDriver Driver, WebDriverWait wait)
        {
           // List<PositionDetails> positionDetailsList = new List<PositionDetails>();
            commonFunctions.SelectByValue(Driver, By.Id("get_location"), location);
           var cards= commonFunctions.FindElements(Driver, By.ClassName("card-jobsHot"));
            foreach (var item in cards)
            {
                Console.WriteLine("Position =" + item.FindElement(By.TagName("h2")).Text);
                Console.WriteLine("More_info =" + item.FindElement(By.ClassName("card-jobsHot__link")).GetAttribute("href"));
                Console.WriteLine("city ="+ item.FindElement(By.ClassName("card-jobsHot__location")).Text);
                //positionDetailsList.Add(new PositionDetails()
                //{
                //    city = item.FindElement(By.ClassName("card-jobsHot__location")).Text,
                //    Position = item.FindElement(By.TagName("h2")).Text,
                //    More_info = item.FindElement(By.ClassName("card-jobsHot__link")).GetAttribute("href")
                //}) ;
            }

           // return positionDetailsList;
        }
    }
}
