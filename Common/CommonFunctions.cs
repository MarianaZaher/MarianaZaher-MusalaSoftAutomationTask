using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusalaSoftAutomationTask.Properties;
using System.Threading;
using System.IO;

namespace MusalaSoftAutomationTask.Common
{
    class CommonFunctions
    {
        public IWebDriver SetupDriver(out WebDriverWait wait, PageLoadStrategy pageLoadStrategy = PageLoadStrategy.Default, double timeToWaitInMinutes = 2)
        {
            IWebDriver driver;
            var driversDirectory = TestContext.CurrentContext.TestDirectory;

            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("ChromeWebDriver")))
                driversDirectory = Environment.GetEnvironmentVariable("ChromeWebDriver");
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("test-type");
            chromeOptions.AddArguments("start-maximized");
            chromeOptions.AddArguments("--js-flags=--expose-gc");
            chromeOptions.AddArguments("--enable-precise-memory-info");
            chromeOptions.AddArguments("--disable-popup-blocking");
            chromeOptions.AddArguments("--disable-default-apps");
            chromeOptions.AddArguments("test-type=browser");
            chromeOptions.AddArguments("disable-infobars");
            chromeOptions.AddArguments("--disable-notifications");
            chromeOptions.AddArguments("--disable-device-discovery-notifications");
            chromeOptions.AddExcludedArgument("enable-automation");
            //chromeOptions.AddAdditionalCapability("useAutomationExtension", false);
            chromeOptions.SetLoggingPreference(LogType.Browser, LogLevel.Severe);
            chromeOptions.PageLoadStrategy = pageLoadStrategy;
            driver = new ChromeDriver(driversDirectory, chromeOptions, TimeSpan.FromMinutes(timeToWaitInMinutes));




            wait = new WebDriverWait(driver, TimeSpan.FromMinutes(timeToWaitInMinutes));


            driver.Manage().Window.Maximize();

            return driver;
        }


        /// <summary>
        /// Navigate to given URL
        /// </summary>
        /// <param name="url">URL to go to</param>
        public virtual void GoToUrl(IWebDriver Driver )
        {
           
            try
            {
                Driver.Url =Resources.URL ;
            }
            catch (Exception)
            {
                // ignored
            }
        }


        public virtual bool NavigateToTab(IWebDriver Driver, WebDriverWait wait, string TabName)
        {
            string TabLink = Resources.URL + TabName;
            IWebElement element = Driver.FindElement(By.XPath($"//a[@href='{TabLink }/']"));
            ClickOn(Driver, wait, By.XPath($"//a[@href='{ TabLink }/']"), true);

              return  ValidatePageURL(Driver, TabLink);

            
        }

        /// <summary>
        /// Scripts: Returns IJavaScriptExecutor Object for the provided _driver
        /// Can be used for Clicking, Scrolling or Executing any Javascript Code in the Browser
        /// How to Use?
        ///     example:
        ///         ExecuteScript("arguments[0].click();", element);
        /// </summary>
        /// <returns>IJavaScriptExecutor Object</returns>
        public virtual IJavaScriptExecutor Scripts(IWebDriver Driver)
        {
            return (IJavaScriptExecutor)Driver;
        }

        /// <summary>
        /// Executes Javascript in the current page
        /// Usage Example: ExecuteScript("arguments[0].scrollIntoView();", FindElement(By.Id("elementToScrollToId")));
        /// </summary>
        /// <param name="script">Script to be executed in the page</param>
        /// <param name="args">Parameters used in the script</param>
        /// <returns></returns>
        public virtual object ExecuteScript(IWebDriver Driver, string script, params object[] args)
        {
            return Scripts(Driver).ExecuteScript(script, args);
        }


        /// <summary>
        /// Switch to window containing part of URL
        /// </summary>
        /// <param name="urlPart">Part of the URL that should be in the page</param>
        public virtual void SwitchToWindowWithUrlContaining(IWebDriver Driver, string urlPart)
        {
            try
            {
                foreach (var window in Driver.WindowHandles)
                {
                    Driver.SwitchTo().Window(window);
                    if (Driver.Url.Contains(urlPart))
                    {
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                // Methods to be handeled
                /* LogIssue("", "Exception occurred in function \"" + GetCurrentMethod() + "\". Please check attached screenshot: " + TakeScreenshot(), false, true);*/
                throw e;
            }
        }

        /// <summary>
        /// Type text to input
        /// </summary>
        /// <param name="Driver"></param>
        /// <param name="wait"></param>
        /// <param name="locator"></param>
        /// <param name="text"></param>
        public void SendKeys(IWebDriver Driver, WebDriverWait wait, By locator, string text)
        {
            try
            {
                IWebElement element = Driver.FindElement(locator);
                wait.Until(d => element.Displayed);
                wait.Until(d => element.Enabled);
                element.SendKeys(text);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /// <summary>
        /// Click element
        /// </summary>
        /// <param name="Driver"></param>
        /// <param name="wait"></param>
        /// <param name="element"></param>

        public void ClickElement(IWebDriver Driver, WebDriverWait wait, IWebElement element)
        {
            try
            {
                wait.Until(d => element.Displayed);
                wait.Until(d => element.Enabled);
                element.Click();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void ClickElement(IWebDriver Driver, WebDriverWait wait, By locator)
        {
            try
            {
                IWebElement element = Driver.FindElement(locator);
                wait.Until(d => element.Displayed);
                wait.Until(d => element.Enabled);
                element.Click();
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /// <summary>
        /// Click on Element: Tries to Click on an Element using its By Locator and waits if there is InvalidOperationException or StaleElementReferenceException
        /// Clicks on Element using javascript if usingJavascript is true and using .Click() Function if usingJavascript is false
        /// </summary>
        /// <param name="locator">By Locator of Element that needs to be Clicked</param>
        /// <param name="usingJavascript">Click on the button using Javascript or not</param>
        /// <param name="locatorDescription">Description of the Element for Logging</param>
        public virtual void ClickOn(IWebDriver Driver, WebDriverWait wait, By locator, bool usingJavascript, string locatorDescription = "")
        {
           

            try
            {
                var type = Driver.GetType();

                wait.Until(d =>
                {
                    try
                    {
                        var element = FindElement(Driver, locator);
                        if (element != null && element.Enabled)
                        {
                            if (usingJavascript || type.Name.Equals("InternetExplorerDriver"))
                            {
                                ExecuteScript(Driver, "arguments[0].click();", element);
                                return true;
                            }
                            if (element.Displayed)
                            {
                                element.Click();
                                return true;
                            }
                        }
                        return false;
                    }
                    catch (InvalidOperationException)
                    {
                        return false;
                    }
                    catch (StaleElementReferenceException)
                    {
                        return false;
                    }
                    catch (WebDriverException)
                    {
                        return true;
                    }
                });
            }
            catch (Exception e)
            {
                //LogIssue("", "Exception occurred in function \"" + GetCurrentMethod() + "\". Please check attached screenshot: " + TakeScreenshot(), false, true);
                throw e;
            }
        }

        /// <summary>
        /// Click on Element: Tries to click on an element using the browser's Javascript or not depending on the boolean value
        /// and waits if there is InvalidOperationException or StaleElementReferenceException
        /// </summary>
        /// <param name="element">Element that needs to be Clicked</param>
        /// <param name="usingJavascript">Click on the button using Javascript or not</param>
        /// <param name="elementDescription">Description of the Element for Logging</param>
        public virtual void ClickOn(IWebDriver Driver, WebDriverWait wait, IWebElement element, bool usingJavascript, string elementDescription = "")
        {
           
            try
            {
                var type = Driver.GetType();

                wait.Until(d =>
                {
                    try
                    {
                        if (element != null && element.Enabled)
                        {
                            if (usingJavascript || type.Name.Equals("InternetExplorerDriver"))
                            {
                                ExecuteScript(Driver,"arguments[0].click();", element);
                                return true;
                            }
                            if (element.Displayed)
                            {
                                element.Click();
                                return true;
                            }
                        }
                        return false;
                    }
                    catch (InvalidOperationException)
                    {
                        return false;
                    }
                    catch (StaleElementReferenceException)
                    {
                        return false;
                    }
                    catch (WebDriverException)
                    {
                        return true;
                    }
                });
            }
            catch (Exception e)
            {
                //LogIssue("", "Exception occurred in function \"" + GetCurrentMethod() + "\". Please check attached screenshot: " + TakeScreenshot(), false, true);
                throw e;
            }
        }


        /// <summary>
        /// Select from lookup by value
        /// </summary>
        /// <param name="Driver"></param>
        /// <param name="locator"></param>
        /// <param name="selectValue"></param>
        public void SelectByValue(IWebDriver Driver, By locator, string selectValue)
        {

            try
            {
                new SelectElement(Driver.FindElement(locator)).SelectByValue(selectValue);
            }
            catch (Exception e)
            {
                throw e;
            }
        }



        /// <summary>
        /// Get alert message
        /// </summary>
        /// <param name="Driver"></param>
        /// <returns></returns>
        public string GetAlertMessage(IWebDriver Driver)
        {

            string Message = Driver.SwitchTo().Alert().Text;
            Driver.SwitchTo().Alert().Accept();

            return Message;
        }


        public bool ValidatePageURL(IWebDriver Driver, string Expectedtitle)
        {

            String title = Driver.Title;
            string currentURL = Driver.Url;

            return currentURL.Contains(Expectedtitle, StringComparison.OrdinalIgnoreCase);
            
        }





        /// <summary>
        /// Checks if the driver is open or not
        /// </summary>
        /// <returns></returns>
        public virtual bool IsDriverOpen(IWebDriver Driver)
        {
            if (Driver == null) return false;

            try
            {
                // just to check driver is open
                var str = Driver.CurrentWindowHandle;


                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Close and Dispose the driver if opened
        /// </summary>
        public virtual void CloseDriver(IWebDriver Driver)
        {
            try
            {

                bool driverWasOpen;

                if (driverWasOpen = IsDriverOpen(Driver) && Driver.WindowHandles.Count == 1)
                    Driver.Close();
                else
                    Driver.Quit();

                Driver.Dispose();


            }
            catch (Exception e)
            {
                //LogIssue("", "Exception occurred in function \"" + GetCurrentMethod() + "\". Please check attached screenshot: " + TakeScreenshot(), false, true);
                throw e;
            }
        }

        /// <summary>
        /// Scroll to Element: scrolls to an element in the page (horizontal or vertical)
        /// <br>Automatically detects if there is a Custom Scroll Bar (mCustomScrollbar) in the page and uses the correct scroll function in this case</br>
        /// </summary>
        /// <param name="locator">Locator of Element that should be scrolled to</param>
        /// <param name="locatorDescription">Description of the Element for Logging</param>
        public virtual void ScrollTo(IWebDriver Driver,By locator, string locatorDescription = "")
        {
          

            try
            {
                var counter = 0;
                bool elementInView;

                do
                {
                    if (IsElementPresent(Driver,By.ClassName("mCustomScrollbar")))
                    {
                        ExecuteScript(Driver,"$('.mCustomScrollbar').mCustomScrollbar(\"scrollTo\",arguments[0]);", FindElement(Driver,locator));
                    }
                    else
                    {
                        ExecuteScript(Driver,"arguments[0].scrollIntoView();", FindElement(Driver,locator));
                    }

                    counter++;
                    elementInView = (bool)ExecuteScript(Driver,
                        "var elem = arguments[0],                 " +
                        "  box = elem.getBoundingClientRect(),    " +
                        "  cx = box.left + box.width / 2,         " +
                        "  cy = box.top + box.height / 2,         " +
                        "  e = document.elementFromPoint(cx, cy); " +
                        "for (; e; e = e.parentElement) {         " +
                        "  if (e === elem)                        " +
                        "    return true;                         " +
                        "}                                        " +
                        "return false;                            "
                        , FindElement(Driver,locator));
                } while (!elementInView && counter < 5);

                Thread.Sleep(1000);
            }
            catch (Exception e)
            {
               /* LogIssue("", "Exception occurred in function \"" + GetCurrentMethod() + "\". Please check attached screenshot: " + TakeScreenshot(), false, true);*/
                throw e;
            }
        }


        /// <summary>
        /// Is Element Present: Checks whether an Element is Present or Not
        /// </summary>
        /// <param name="locator">By Locator of Element that needs to be found</param>
        /// <param name="locatorDescription">Description of the Element for Logging</param>
        /// <returns>True in case element is present and False in case element is not present</returns>
        public virtual bool IsElementPresent(IWebDriver Driver, By locator, string locatorDescription = "")
        {
          
            try
            {
                FindElement(Driver,locator);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Finds the first element using the given locator
        /// Usage Example: FindElement(By.Id(""));
        /// </summary>
        /// <param name="locator">By locator of the element</param>
        /// <returns>Element found</returns>
        public virtual IWebElement FindElement(IWebDriver Driver, By locator)
        {
            return Driver.FindElement(locator);
        }


        /// <summary>
        /// Get Text of: gets the text of the element sent as parameter
        /// </summary>
        /// <param name="locator">Locator of the Element whose text will be returned</param>
        /// <param name="locatorDescription">Description of the Element for Logging</param>
        /// <returns>The Text of the Element</returns>
        public virtual string GetTextOf(IWebDriver Driver,By locator, string locatorDescription = "")
        {
          
            try
            {
                return FindElement(Driver,locator).Text;
            }
            catch (Exception e)
            {
                //LogIssue("", "Exception occurred in function \"" + GetCurrentMethod() + "\". Please check attached screenshot: " + TakeScreenshot(), false, true);
                throw e;
            }
        }


        /// <summary>
        /// Get Text of: gets the text of the element sent as parameter
        /// </summary>
        /// <param name="locator">Locator of the Element whose text will be returned</param>
        /// <param name="locatorDescription">Description of the Element for Logging</param>
        /// <returns>The Text of the Element</returns>
        public virtual string GetTextOf(IWebDriver Driver, IWebElement element, string locatorDescription = "")
        {

            try
            {
                return element.Text;
            }
            catch (Exception e)
            {
                //LogIssue("", "Exception occurred in function \"" + GetCurrentMethod() + "\". Please check attached screenshot: " + TakeScreenshot(), false, true);
                throw e;
            }
        }


        /// <summary>
        /// Get Parent Element: gets the parent of the element sent as parameter
        /// </summary>
        /// <param name="element">Child Element that the function should find its Parent</param>
        /// <param name="elementDescription">Description of the Element for Logging</param>
        /// <returns>The Parent Element</returns>
        public virtual IWebElement GetParentElement(IWebDriver Driver, By locator, string elementDescription = "")
        {
            
            try
            {
                IWebElement element = Driver.FindElement(locator);
                var parentElement = element.FindElement(By.XPath(".."));

                return parentElement;
            }
            catch (Exception e)
            {
                //LogIssue("", "Exception occurred in function \"" + GetCurrentMethod() + "\". Please check attached screenshot: " + TakeScreenshot(), false, true);
                throw e;
            }
        }


        /// <summary>
        /// Is Text Visible: Checks whether some Text is Visible in the page or Not
        /// </summary>
        /// <param name="text">text that needs to be found</param>
        /// <returns>True in case text is visible and False in case text is not visible</returns>
        public virtual bool IsTextVisible(IWebDriver Driver, string text)
        {
           
            try
            {
                var element = FindElement(Driver,By.XPath($"//span[contains(text(), '{text}')]"));
                return element.Displayed;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public virtual bool IsButtonVisible(IWebDriver Driver, string value)
        {

            try
            {
                var element = FindElement(Driver, By.XPath($"//input[contains(@value, '{value}')]"));
                return element.Displayed;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Driver"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public virtual bool IsSectionVisible(IWebDriver Driver, string text)
        {

            try
            {
                var element = FindElement(Driver, By.XPath($"//h2[contains(text(), '{text}')]"));
                return element.Displayed;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// set Directory: Get the full path of certain Folder inside the Project
        /// </summary>
        /// <param name="folderName">Container Folder inside the Project (e.g. Drivers or Attachments)</param>
        /// <returns></returns>
        public static string SetDir(string folderName)
        {

            return Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.FullName) + folderName;
            
        }
        public virtual void UploadFile(IWebDriver Driver, By locator)
        {
            var pdfAttachmentPath = SetDir("\\TestDataFiles\\pdf.pdf");
            IWebElement fileUpload = Driver.FindElement(locator);
            fileUpload.SendKeys(pdfAttachmentPath);

            WaitForPageReadyState(Driver);
        }


        /// <summary>
        /// Finds all elements using the given locator
        /// Usage Example: FindElements(By.Id(""));
        /// </summary>
        /// <param name="locator">By locator of the elements</param>
        /// <returns>List of elements found</returns>
        public virtual IList<IWebElement> FindElements(IWebDriver Driver,By locator)
        {
            return Driver.FindElements(locator);
        }


        /// <summary>
        /// Wait for Page Ready State: Waits till the page stops loading using javascript executor to get the value of "readyState" property in the page
        /// </summary>
        public virtual void WaitForPageReadyState(IWebDriver Driver)
        {
            string documentReadyState;
            do
            {
                try
                {
                    documentReadyState = (string)ExecuteScript(Driver,"return document.readyState");
                }
                catch (Exception)
                {
                    break;
                }
                Thread.Sleep(500);
            } while (!documentReadyState.Equals("complete") && !documentReadyState.Equals("interactive"));

            var iframes = FindElements(Driver,By.TagName("iframe"));

            for (var iframesCount = 0; iframesCount < iframes.Count; iframesCount++)
            {
                iframes = FindElements(Driver,By.TagName("iframe"));
                string frameId;

                try
                {
                    frameId = iframes[iframesCount].GetAttribute("id");
                }
                catch (ArgumentOutOfRangeException)
                {
                    break;
                }
                catch (StaleElementReferenceException)
                {
                    continue;
                }

                string frameReadyState;
                do
                {
                    try
                    {
                        frameReadyState =
                            (string)
                            ExecuteScript(Driver,"return document.getElementById('" + frameId + "').contentDocument.readyState");
                    }
                    catch (Exception)
                    {
                        break;
                    }
                    Thread.Sleep(500);
                } while (!frameReadyState.Equals("complete") && !frameReadyState.Equals("interactive"));
            }
        }

        /// <summary>
        /// Wait till Element is Displayed: Uses explicit wait till the element is displayed
        /// </summary>
        /// <param name="element">Element that should be displayed</param>
        /// <param name="locatorDescription">Description of the Element for Logging</param>
        public virtual void WaitTillElementIsDisplayed(IWebElement element, WebDriverWait wait, string locatorDescription = "")
        {
          
            try
            {
                wait.Until(d =>
                {
                    try
                    {
                        return element.Displayed;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                });
            }
            catch (Exception e)
            {
                //LogIssue("", "Exception occurred in function \"" + GetCurrentMethod() + "\". Please check attached screenshot: " + TakeScreenshot(), false, true);
                throw e;
            }
        }


    }
}

