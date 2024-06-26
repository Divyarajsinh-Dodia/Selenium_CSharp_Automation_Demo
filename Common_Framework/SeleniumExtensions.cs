using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using NUnit.Framework;
using Common_Framework.Common;

namespace Common_Framework
{
    public static class SeleniumExtensions
    {
        /// <summary>
        /// Retries specific actions.
        /// </summary>
        /// <param name="actions">Need to pass actions to be preformed.</param>
        /// <param name="retryCount">Retry count for number of retries.</param>
        public static void Retry(Action actions, int retryCount = 3)
        {
            for (int counter = 1; counter <= retryCount; counter++)
            {
                try
                {
                    actions();
                    break;
                }
                catch (Exception ex)
                {
                    if (counter == retryCount)
                    {
                        throw;
                    }

                    var stackTrace = new StackTrace();
                    TestContext.Progress.WriteLine(
                        $"[Caller {stackTrace.GetFrame(1).GetMethod().Name}]: {ex.Message} (Going to retry the same action. Retry #{counter})");
                    TestContext.WriteLine(
                         $"[Caller {stackTrace.GetFrame(1).GetMethod().Name}]: {ex.Message} (Going to retry the same action. Retry #{counter})");
                }
            }
        }

        #region Alerts

        /// <summary>
        /// If an alert is present this will accept it.
        /// </summary>
        /// <param name="driver">Driver.</param>
        public static void AcceptPresentAlert(this IWebDriver driver)
        {
            bool alert;
            try
            {
                driver.SwitchTo().Alert();
                alert = true;
            }
            catch (NoAlertPresentException)
            {
                alert = false;
            }

            if (alert)
            {
                driver.SwitchTo().Alert().Accept();
            }
        }

        /// <summary>
        /// If an alert is present this will dismiss it.
        /// </summary>
        /// <param name="driver">Driver.</param>
        public static void DismissAlert(this IWebDriver driver)
        {
            IAlert alert = driver.SwitchTo().Alert();
            alert.Dismiss();
        }

        /// <summary>
        /// If an alert is present this will get Alert Text.
        /// </summary>
        /// <param name="driver">Driver.</param>
        /// <returns>Returns Alert text.</returns>
        public static string GetAlertText(this IWebDriver driver)
        {
            IAlert alert = driver.SwitchTo().Alert();
            return alert.Text;
        }

        #endregion Alerts

        #region Click

        /// <summary>
        /// Clicks Element with retries.
        /// </summary>
        /// <param name="driver">Need to pass Driver as parameter. Use this as "Driver.ClickWithRetry".</param>
        /// <param name="element">Pass element as Parameter for locators such as Id, Xpath, Name, Lisktext, etc.</param>
        public static void ClickWithRetry(this IWebDriver driver, IWebElement element, int retry = 10)
        {
            Retry(() =>
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(5000));
                ////var executor = (IJavaScriptExecutor)driver;
                ////executor.ExecuteScript("arguments[0].scrollIntoView(true);", element);

                ////wait.Until(d => element.IsVisible());
                ////new Actions(driver)
                ////    .MoveToElement(element)
                ////    .Build()
                ////    .Perform();
                wait.Until(d => element.IsVisible() && element.Enabled);
            });
            Retry(() => element.Click(), retry);
        }

        /// <summary>
        /// Clicks Element by Javascript.
        /// </summary>
        /// <param name="driver">Need to pass Driver as parameter. Use this as "Driver.JavascriptClick".</param>
        /// <param name="element">Pass element as Parameter for locators such as Id, Xpath, Name, Lisktext, etc.</param>
        public static void JavascriptClick(this IWebDriver driver, IWebElement element, int retry = 3)
        {
            Retry(
                () =>
                {
                    var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(5000));
                    wait.Until(d => element.IsVisible() && element.Enabled);
                    var executor = (IJavaScriptExecutor)driver;
                    executor.ExecuteScript("arguments[0].click();", element);
                }, retry);
        }

        /// <summary>
        /// Clicks Element with retries.
        /// </summary>
        /// <param name="element">Pass element as Parameter for locators such as Id, Xpath, Name, Lisktext, etc.</param>
        /// <param name="retries">Retries click for number of times.</param>
        public static void ClickWithRetry(IWebElement element, int retries = 3)
        {
            int attempts = 0;
            while (attempts < retries)
            {
                try
                {
                    element.Click();
                    break;
                }
                catch (Exception)
                {
                    attempts++;
                    Thread.Sleep(750);
                }
            }
        }

        /// <summary>
        /// Performs Double click Action.
        /// </summary>
        /// <param name="driver">IWebElement to be Clicked.</param>
        /// <param name="element">Amount of time to wait in between clicks.</param>
        public static void DoubleClickAction(this IWebDriver driver, IWebElement element)
        {
            Actions actions = new Actions(driver);
            actions.DoubleClick(element).Perform();
        }

        /// <summary>
        /// Waits for the Driver to find the element and attempts to click the object, if the element is not clickable then the exception is caught and the click is retried.
        /// </summary>
        /// <param name="element">IWebElement to be Clicked.</param>
        /// <param name="miliseconds">Amount of time to wait in between clicks.</param>
        private static void Click(this IWebElement element, int miliseconds)
        {
            var seconds = 0;
            while (seconds < 11)
            {
                try
                {
                    element.Click();
                    break;
                }
                catch (Exception ex)
                {
                    TestContext.Progress.WriteLine("{0}\t[Click] : {1}", TestContext.CurrentContext.Test.MethodName, ex.Message);
                }

                Thread.Sleep(miliseconds);
                seconds++;

                if (seconds == 10)
                {
                    throw new NoSuchElementException($"{element} is not clickable in {TestContext.CurrentContext.Test.MethodName}");
                }
            }
        }

        #endregion Click

        #region SendKeys

        /// <summary>
        /// Use when SendKeys() is not working on a field.
        /// </summary>
        /// <param name="driver">this.driver.</param>
        /// <param name="element">Iwebelement Field.</param>
        /// <param name="inputValiue">String to be sent to textbox.</param>
        /// <param name="retry">number of retries, default = 3.</param>
        public static void JavascriptSendKeys(this IWebDriver driver, IWebElement element, string inputValiue, int retry = 3)
        {
            Retry(
                () =>
                {
                    var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(5000));
                    wait.Until(d => element.IsVisible() && element.Enabled);
                    var executor = (IJavaScriptExecutor)driver;
                    executor.ExecuteScript($"arguments[0].value='{inputValiue}';", element, true);
                }, retry);
        }

        /// <summary>
        /// Sends the keys to a formatted field.
        /// </summary>
        /// <param name="element">IWebElement field.</param>
        /// <param name="inputValue">String to be sent.</param>
        /// <param name="milliseconds">Length of time to wait.</param>
        public static void SendKeys(this IWebElement element, string inputValue, int milliseconds)
        {
            element.Click(milliseconds);
            element.Clear();
            element.Click(milliseconds);
            element.SendKeys(inputValue);
        }

        /// <summary>
        /// Clears the field prior to entering the value.
        /// </summary>
        /// <param name="element">IWebElement.</param>
        /// <param name="inputValue">Value to be entered.</param>
        /// <param name="clearField">True clears the field.</param>
        public static void SendKeysWithClear(this IWebElement element, string inputValue, bool clearField)
        {
            if (clearField)
            {
                try
                {
                    element.Click();
                    element.SendKeys(Keys.Control + "a");
                    element.SendKeys(Keys.Delete);
                }
                catch
                {
                    element.Clear();
                }
            }

            element.SendKeys(inputValue);
        }

        /// <summary>
        /// Peforms SendKeys and Tab.
        /// </summary>
        /// <param name="element">Need to pass IWebelement.</param>
        /// <param name="text">Need to pass text.</param>
        public static void SendKeysToElementAndTab(IWebElement element, string text)
        {
            element.SendKeys(text);
            element.SendKeys(Keys.Tab);
        }

        /// <summary>
        /// Peforms SendKeys and Enter.
        /// </summary>
        /// <param name="element">Need to pass IWebelement.</param>
        /// <param name="text">Need to pass text.</param>
        public static void SendKeysToElementAndEnter(IWebElement element, string text)
        {
            element.SendKeys(text);
            element.SendKeys(Keys.Enter);
        }

        /// <summary>
        /// Peforms SendKeys but only of max length of textbox/element.
        /// </summary>
        /// <param name="element">Need to pass IWebelement.</param>
        /// <param name="text">Need to pass text.</param>
        public static void SendKeysWithMaxLength(IWebElement element, string text)
        {
            int maxLength = int.Parse(element.GetAttribute("maxlength"));
            if (text.Length > maxLength)
            {
                text = text.Substring(0, maxLength);
            }

            element.SendKeys(text);
        }
        #endregion SendKeys

        #region Checkbox

        /// <summary>
        /// Checks/Unchecks a check box based on the entered boolean.
        /// </summary>
        /// <param name="element">IWebElement checkbox.</param>
        /// <param name="check">bool, true = checked, false = unchecked.</param>
        public static void Check(this IWebElement element, bool check)
        {
            if (check && !element.Selected)
            {
                element.Click(500);
            }

            if (!check && element.Selected)
            {
                element.Click(500);
            }
        }

        /// <summary>
        /// Gets checkbox Value.
        /// </summary>
        /// <param name="checkbox">Need to pass IWebElement.</param>
        /// <returns>Returns string value of Checkbox.</returns>
        public static string GetCheckboxValue(IWebElement checkbox)
        {
            return checkbox.GetAttribute("value");
        }

        /// <summary>
        /// Gets checkbox Text.
        /// </summary>
        /// <param name="checkbox">Need to pass IWebElement.</param>
        /// <returns>Returns string value of Checkbox.</returns>
        public static string GetCheckboxText(IWebElement checkbox)
        {
            return checkbox.Text;
        }

        #endregion Checkbox

        #region Common_Exts

        /// <summary>
        /// Returns bool if element is visible or not.
        /// </summary>
        /// <returns>Returs true if element is visible on screen otherwise returns false.</returns>
        /// <param name="element">Element for which visibility needs to be checked.</param>
        public static bool IsVisible(this IWebElement element)
        {
            var isVisible = false;
            try
            {
                isVisible = element.Displayed;
            }
            catch (Exception ex)
            {
                TestContext.Progress.WriteLine("[Click] : {0}", ex.Message);
            }

            return isVisible;
        }

        /// <summary>
        /// Gets the Value of an element.
        /// </summary>
        /// <param name="element">Element.</param>
        /// <returns>Value.</returns>
        public static string GetValue(this IWebElement element)
        {
            string temp;
            try
            {
                temp = element.Text;
                if (string.IsNullOrEmpty(temp))
                {
                    temp = element.GetAttribute("value");
                }
            }
            catch (NoSuchElementException)
            {
                return null;
            }

            return string.IsNullOrEmpty(temp) ? temp : temp.RemoveSpecialCharacters();
        }

        /// <summary>
        /// Enters text into the input field.
        /// If value of the text is null then generates a random string.
        /// </summary>
        /// <param name="element">todo: describe element parameter on EnterText.</param>
        /// <param name="text">Refrence of Text.</param>
        /// <param name="prefix">Prefix for randomly generated text.</param>
        /// <param name="length">Length of generated text without prefix length.</param>
        /// <returns>String Value</returns>
        public static string EnterText(this IWebElement element, ref string text, string prefix = "Auto", int length = 5)
        {
            if (string.IsNullOrEmpty(text))
            {
                text = prefix.RandomString(length);
            }

            element.Clear();
            element.SendKeys(text);
            return text;
        }

        /// <summary>
        /// Enters number into the input field.
        /// If value of the text is null then generates a random string.
        /// </summary>
        /// <param name="element">todo: describe element parameter on EnterNumber.</param>
        /// <param name="text">Refrence of Text.</param>
        /// <param name="length">Length of randomly generated number.</param>
        /// <returns>String.</returns>
        public static string EnterNumber(this IWebElement element, ref string text, int length = 10)
        {
            if (string.IsNullOrEmpty(text))
            {
                text = new Random().Next(999999999).ToString().PadRight(length, '0').Substring(0, length);
            }

            element.Clear();
            element.SendKeys(text);
            return text;
        }

        /// <summary>
        /// Makes the Driver wait until the Url Changes
        /// </summary>
        /// <param name="driver">WebDriver</param>
        /// <param name="url">The url to compare.</param>
        public static void WaitForUrlChange(this IWebDriver driver, string url)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(60000));
            wait.Until(d => d.Url != url);
        }
        #endregion Common_Exts

        #region DropDown

        /// <summary>
        /// Selects a menu item from a drop down list of items.
        /// </summary>
        /// <param name="webElement">element of list to target.</param>
        /// <param name="itemToSelect">text match of item chosen from the selected list.</param>
        public static void Select(this IWebElement webElement, string itemToSelect)
        {
            new SelectElement(webElement).SelectByText(itemToSelect);
        }

        /// <summary>
        /// Select dropdown by value.
        /// </summary>
        /// <param name="webElement">Need IWebElement for finding element.</param>
        /// <param name="itemToSelect">Pass Item to be selected.</param>
        public static void SelectByValue(this IWebElement webElement, string itemToSelect)
        {
            new SelectElement(webElement).SelectByValue(itemToSelect);
        }

        /// <summary>
        /// Select dropdown by value.
        /// </summary>
        /// <param name="webElement">Need IWebElement for finding element.</param>
        /// <param name="itemIndex">Pass Item index to be selected.</param>
        public static void SelectByIndex(this IWebElement webElement, int itemIndex)
        {
            new SelectElement(webElement).SelectByIndex(itemIndex);
        }

        /// <summary>
        /// Selects a menu item from a drop down list of items.
        /// </summary>
        /// <param name="webElement">element of list to target</param>
        /// <param name="itemToSelect">enum value of item chosen from selected list of enums</param>
        public static void Select(this IWebElement webElement, Enum itemToSelect)
        {
            Retry(
                () =>
                {
                    var menuItem = itemToSelect.GetDescription();
                    new SelectElement(webElement).SelectByText(menuItem);
                },
                5);
        }

        /// <summary>
        /// Gets the string values of all the options in a drop down list
        /// </summary>
        /// <param name="element">Drop down field</param>
        /// <returns>List of string of the values</returns>
        public static IList<string> AllDropDownOptions(this IWebElement element)
        {
            IList<string> values = new List<string>();

            var selectList = new SelectElement(element);
            var webList = selectList.Options;
            foreach (var webElement in webList)
            {
                values.Add(webElement.GetValue());
            }

            return values;
        }

        /// <summary>
        /// Gets the string value of the first selected option of a drop down
        /// </summary>
        /// <param name="element">Drop down field</param>
        /// <returns>Value of first option</returns>
        public static string SelectedDropDownValue(this IWebElement element) => new SelectElement(element).SelectedOption.GetValue();

        /// <summary>
        /// Checks if option is present in dropdown list or not.
        /// </summary>
        /// <param name="dropdown">element of list to target.</param>
        /// <param name="text">text match of item chosen from the selected list.</param>
        /// <returns>Returns bool as true if passed string is present in dropdown option</returns>
        public static bool IsDropdownOptionPresent(IWebElement dropdown, string text)
        {
            SelectElement selectElement = new SelectElement(dropdown);
            foreach (IWebElement option in selectElement.Options)
            {
                if (option.Text == text)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Deselects all selected dropdown options.
        /// </summary>
        /// <param name="dropdown">element of list to target.</param>
        public static void ClearDropdownSelection(IWebElement dropdown)
        {
            SelectElement selectElement = new SelectElement(dropdown);
            selectElement.DeselectAll();
        }

        /// <summary>
        /// Returs Count of dropdown options.
        /// </summary>
        /// <param name="dropdown">element of list to target.</param>
        /// <returns>Returns Count of dropdown options.</returns>
        public static int GetDropdownSize(IWebElement dropdown)
        {
            SelectElement selectElement = new SelectElement(dropdown);
            return selectElement.Options.Count;
        }

        /// <summary>
        /// Checks if dropdown is Multiselect or not.
        /// </summary>
        /// <param name="dropdown">element of list to target.</param>
        /// <returns>Returns Bool as true if dropdown supports multiselect functionality or not.</returns>
        public static bool IsDropdownMultiple(IWebElement dropdown)
        {
            SelectElement selectElement = new SelectElement(dropdown);
            return selectElement.IsMultiple;
        }

        /// <summary>
        /// Selects Multiple options in MultiSelect Dropdown By values.
        /// </summary>
        /// <param name="dropdown">element of list to target.</param>
        public static void SelectMultipleDropdownByValues(IWebElement dropdown, string[] values)
        {
            SelectElement selectElement = new SelectElement(dropdown);
            foreach (string value in values)
            {
                selectElement.SelectByValue(value);
            }
        }

        /// <summary>
        /// Selects Multiple options in MultiSelect Dropdown By indexes.
        /// </summary>
        /// <param name="dropdown">element of list to target.</param>
        public static void SelectMultipleDropdownByIndexes(IWebElement dropdown, int[] indexes)
        {
            SelectElement selectElement = new SelectElement(dropdown);
            foreach (int index in indexes)
            {
                selectElement.SelectByIndex(index);
            }
        }

        /// <summary>
        /// Gets dropdown option values by using text.
        /// </summary>
        /// <param name="dropdown">element of list to target.</param>
        /// <param name="text">Need to pass text of option to get value.</param>
        /// <returns>Returns dropdown option values.</returns>
        public static string GetDropdownOptionValue(IWebElement dropdown, string text)
        {
            SelectElement selectElement = new SelectElement(dropdown);
            foreach (IWebElement option in selectElement.Options)
            {
                if (option.Text == text)
                {
                    return option.GetAttribute("value");
                }
            }

            return null;
        }

        /// <summary>
        /// Gets dropdown option index by using text.
        /// </summary>
        /// <param name="dropdown">element of list to target.</param>
        /// <param name="text">Need to pass text of option to get index.</param>
        /// <returns>Returns dropdown option index.</returns>
        public static int GetDropdownOptionIndex(IWebElement dropdown, string text)
        {
            SelectElement selectElement = new SelectElement(dropdown);
            int index = 0;
            foreach (IWebElement option in selectElement.Options)
            {
                if (option.Text == text)
                {
                    return index;
                }
                index++;
            }

            return -1;
        }

        /// <summary>
        /// Gets selected dropdown option index by using text.
        /// </summary>
        /// <param name="dropdown">element of list to target.</param>
        /// <returns>Returns dropdown option index.</returns>
        public static int GetSelectedDropdownOptionIndex(IWebElement dropdown)
        {
            SelectElement selectElement = new SelectElement(dropdown);
            return selectElement.Options.IndexOf(selectElement.SelectedOption);
        }

        /// <summary>
        /// Selects Random Dropdown option.
        /// </summary>
        /// <param name="dropdown">element of list to target.</param>
        public static void SelectRandomDropdownOption(IWebElement dropdown)
        {
            SelectElement selectElement = new SelectElement(dropdown);
            Random random = new Random();
            int index = random.Next(selectElement.Options.Count);
            selectElement.SelectByIndex(index);
        }

        /// <summary>
        /// Selects dropdown option using partial texts.
        /// </summary>
        /// <param name="dropdown">element of list to target.</param>
        /// <param name="partialText">Need to pass partial text of option.</param>
        public static void SelectDropdownOptionByPartialText(IWebElement dropdown, string partialText)
        {
            SelectElement selectElement = new SelectElement(dropdown);
            foreach (IWebElement option in selectElement.Options)
            {
                if (option.Text.Contains(partialText))
                {
                    selectElement.SelectByText(option.Text);
                    break;
                }
            }
        }

        #endregion DropDown

        #region Waits

        /// <summary>
        /// Sets the Implicit Wait time to off.
        /// </summary>
        /// <param name="driver">WebDriver.</param>
        public static void TurnOffImplicitWaits(this IWebDriver driver, int wait = 0)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(wait);
        }

        /// <summary>
        /// Sets the Implicit Wait time back to the constant time.
        /// </summary>
        /// <param name="driver">WebDriver</param>
        public static void TurnOnImplicitWaits(this IWebDriver driver)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(Constants.ImplicitWaitDuration);
        }

        #endregion Waits
    }
}
