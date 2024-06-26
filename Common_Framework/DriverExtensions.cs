using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Common_Framework
{
    public static class DriverExtensions
    {
        /// <summary>
        /// Gets Elements with Autowait functionality.
        /// </summary>
        /// <returns>Returs WebElement.</returns>
        /// <param name="driver">Need to pass Driver as parameter. Use this as "Driver.FindElementWithAutoWait".</param>
        /// <param name="by">Pass By as Parameter for locators such as Id, Xpath, Name, Lisktext, etc.</param>
        /// <param name="timeoutInSeconds"> Pass TimeOut in Seconds if want to customise for some element.Otherwise Default is 10 Seconds.</param>
        /// <param name="retryCount">Pass Retry count if want to customise.Otherwise Default is 3.</param>
        /// <param name="waitIntervalInMilliseconds">This will check for element in DOM every definite time.Default is 100 miliseconds.</param>
        /// <param name="ignoreExceptions">Pass Exceptions if you want to ignore any.</param>
        public static IWebElement FindElementWithAutoWait(this IWebDriver driver, By by, int timeoutInSeconds = 10, int retryCount = 3, int waitIntervalInMilliseconds = 100, List<Type> ignoreExceptions = null)
        {
            ignoreExceptions ??= new List<Type>();
            IWebElement element = null;
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds))
            {
                PollingInterval = TimeSpan.FromMilliseconds(waitIntervalInMilliseconds),
            };
            for (int i = 0; i < retryCount; i++)
            {
                try
                {
                    element = wait.Until(drv => drv.FindElement(by));
                    if (element != null)
                    {
                        break;
                    }
                }
                catch (Exception ex) when (ignoreExceptions.All(e => e != ex.GetType()))
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return element;
        }

        /// <summary>
        /// Gets Elements with custom wait.
        /// </summary>
        /// <returns>Returns Element.</returns>
        /// <param name="driver">Need to pass Driver as parameter. Use this as "Driver.FindElementWithCustomWait".</param>
        /// <param name="by">Pass By as Parameter for locators such as Id, Xpath, Name, Lisktext, etc.</param>
        public static IWebElement FindElementWithCustomWait(this IWebDriver driver, By by, int waitTime = 10)
        {
            try
            {
                return new WebDriverWait(driver, TimeSpan.FromSeconds(waitTime)).Until(d => d.FindElement(by));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return null;
        }

        /// <summary>
        /// Waits For Element to get removed from DOM.
        /// </summary>
        /// <param name="driver">Need to pass Driver as parameter. Use this as "Driver.WaitForElementToNotExist".</param>
        /// <param name="by">Pass By as Parameter for locators such as Id, Xpath, Name, Lisktext, etc.</param>
        /// <param name="timeOut">Timeout is Max time till which this method will wait for element to get removed from DOM. Time is in Miliseconds.</param>
        /// <returns>Retus bool value as per if element does not exist in DOM then returns true otherwise false.</returns>
        public static bool WaitForElementToNotExist(this IWebDriver driver, By by, int timeOut = 60000)
        {
            try
            {
                driver.TurnOffImplicitWaits();
                var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeOut));
                wait.Until(d => d.FindElements(by).Count < 1);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                driver.TurnOnImplicitWaits();
            }
        }

        /// <summary>
        /// Checks if element is enabled or not.
        /// </summary>
        /// <returns>Returs bool as per element is enabled or not.</returns>
        /// <param name="element">Need to pass IWebElement.</param>
        public static bool IsEnabled(this IWebElement element)
        {
            var temp = element.GetAttribute("class");
            return element.Enabled && !(temp.Contains("Disabled") || temp.Contains("disabled")) ? true : false;
        }

        /// <summary>
        /// Use to select value from a dropdown that select method won't work on
        /// Like Priority, ShipBy, Payment in PO page
        /// </summary>
        /// <param name="driver">Driver</param>
        /// <param name="id">id for dropdown</param>
        /// <param name="value">value to be selected from dropdown</param>
        public static void SetDropDown(this IWebDriver driver, string id, string value = null)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until(d => d.FindElement(By.Id(id)).IsVisible() && d.FindElement(By.Id(id)).Enabled);
            var testText = driver.FindElement(By.Id(id)).GetValue();
            driver.ClickWithRetry(driver.FindElement(By.Id(id)), 50);
            var dropDownId = id.Replace("Input", "DropDown");
            if (value == null)
            {
                wait.Until(d => d.FindElement(By.XPath($"//div[div/@id='{dropDownId}']")).IsVisible());
                wait.Until(d => d.FindElement(By.XPath($"//*[@id='{dropDownId}' and contains(@style, 'transition: none')]")).IsVisible());
                var dropDownElementsList = driver.FindElements(By.XPath($"//*[@id='{dropDownId}']//li"));

                var dropDownTextList = dropDownElementsList.Select(x => x.Text).ToList();
                foreach (var text in dropDownTextList)
                {
                    if (text != testText)
                    {
                        value = text;
                        break;
                    }
                }
            }

            var priorityEleLocator = $"//*[@id='{dropDownId}']//li[text()='{value}']";
            wait.Until(d => d.FindElement(By.XPath(priorityEleLocator)).IsVisible() && d.FindElement(By.XPath(priorityEleLocator)).Enabled);
            driver.ClickWithRetry(driver.FindElement(By.XPath(priorityEleLocator)), 50);
        }

        /// <summary>
        /// Method to select from a list where there is no data tags.
        /// </summary>
        /// <param name="driver">Driver</param>
        /// <param name="webElement">dropdown list element.</param>
        /// <param name="selection">string selection of dropdown choice.</param>
        public static void SelectFromList(this IWebDriver driver, IWebElement webElement, string selection)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            try
            {
                Retry(() =>
                {
                    wait.Until(d => webElement.IsVisible() && webElement.Enabled);
                    wait.Until(d => d.FindElement(By.XPath($"//*[contains(text(), '{selection}')]")).IsVisible());
                    webElement.FindElement(By.XPath($"//*[contains(text(), '{selection}')]")).Click();
                });
            }
            catch (Exception)
            {
                TestContext.Progress.WriteLine();
            }
        }

        /// <summary>
        /// Method to select a value from multi-select dropdown.
        /// </summary>
        /// <param name="driver">Driver.</param>
        /// <param name="webElement">Drop Down Input element.</param>
        /// <param name="list">values to be selected.</param>
        public static void SetMultiSelectDropDown(this IWebDriver driver, IWebElement webElement, params string[] list)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
            webElement.Click();
            var dropDownList = webElement.GetAttribute("Id").Replace("Input", "DropDown");
            var dropDownArrow = webElement.GetAttribute("Id").Replace("Input", "Arrow");
            wait.Until(d => d.FindElementWithAutoWait(By.Id(dropDownList)).IsVisible());
            if (list[0].Equals("[All]") || list[0].Equals("CheckAll"))
            {
                driver.FindElementWithAutoWait(By.XPath($"//*[@id='{dropDownList}']//Label[text()='Check All']/input")).Check(true);
            }
            else
            {
                for (int i = 0; i < list.Length; i++)
                {
                    driver.FindElementWithAutoWait(By.XPath($"//*[@id='{dropDownList}']//Label[text()='Check All']/input")).Check(false);
                    Thread.Sleep(500);
                    driver.FindElementWithAutoWait(By.XPath($"//*[@id='{dropDownList}']//Label[text()='{list[i]}']/input")).Check(true);
                }
            }

            driver.FindElementWithAutoWait(By.Id($"{dropDownArrow}")).Click();
            wait.Until(d => !d.FindElementWithAutoWait(By.Id(dropDownList)).IsVisible());
        }

        /// <summary>
        /// Waits For Element exist in DOM.
        /// </summary>
        /// <param name="driver">Need to pass Driver as parameter. Use this as "Driver.WaitForElementToNotExist".</param>
        /// <param name="by">Pass By as Parameter for locators such as Id, Xpath, Name, Lisktext, etc.</param>
        /// <param name="timeOut">Timeout is Max time till which this method will wait for element to exist DOM. Time is in miliseconds.</param>
        /// <returns>Returs boolean as per if elemenmt is present in DOM then returns true Otherwise will return false.</returns>
        public static bool WaitForElementToExist(this IWebDriver driver, By by, int timeOut = 5)
        {
            try
            {
                driver.TurnOffImplicitWaits();
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeOut));
                wait.Until(d => d.FindElement(by));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                driver.TurnOnImplicitWaits();
            }
        }

        #region Private Methods

        private static void Retry(Action actions, int retryCount = 3)
        {
            for (var counter = 1; counter <= retryCount; counter++)
            {
                try
                {
                    actions?.Invoke();
                    break;
                }
                catch (Exception)
                {
                    if (counter == retryCount)
                    {
                        throw;
                    }
                }
            }
        }

        #endregion Private Methods
    }
}
