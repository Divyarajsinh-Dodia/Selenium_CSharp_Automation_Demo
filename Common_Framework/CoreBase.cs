using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Framework
{
    public abstract class CoreBase
    {
        public static ThreadLocal<IWebDriver> driver = new ThreadLocal<IWebDriver>();
        /// <summary>
        /// Gets or sets instance of selenium webdriver interface.
        /// </summary>
        public IWebDriver Driver
        {
            get => driver.Value;
            set => driver.Value = value;
        }

        /// <summary>
        /// Gets instance of selenium jd executor interface.
        /// </summary>
        public IJavaScriptExecutor JsDriver => Driver as IJavaScriptExecutor;

        /// <summary>
        /// Gets instance of selenium actions class.
        /// </summary>
        public Actions Action => new Actions(Driver);

        /// <summary>
        /// Gets instance of system random class.
        /// </summary>
        public Random Rng => new Random();

        /// <summary>
        /// Gets dEFAULT TIMEOUT.
        /// </summary>
        public TimeSpan DefaultTimeout => TimeSpan.FromSeconds(30);

        /// <summary>
        /// Gets page object for page naigation.
        /// </summary>
        /// <returns name="T">Returns Page Name as parameter.</returns>
        /// <typeparam name="T">Need to pass pageobject as parameter</typeparam>
        public CoreBase GetPageObject<T>()
    where T : CoreBase
        {
            return (T)Activator.CreateInstance(typeof(T), new object[] { Driver });
        }

        #region Public Methods

        /// <summary>
        /// Retries given function multiple times.
        /// </summary>
        /// <param name="actions">Actions to Retry.</param>
        /// <param name="retryCount">Retry Count. Default value is 3.</param>
        public static void Retry(Action actions, int retryCount = 3)
        {
            SeleniumExtensions.Retry(actions, retryCount);
        }

        /// <summary>
        /// Instance of WebDriverWait.
        /// </summary>
        /// <returns>Returs Driver Instance.</returns>
        /// <param name="timeOut">Defines Time for Timeout in miliseconds.</param>
        public WebDriverWait Wait(int timeOut = 60000) => new WebDriverWait(Driver, TimeSpan.FromMilliseconds(timeOut));

        /// <summary>
        /// Scrolls to given webelement.
        /// </summary>
        /// <param name="elem">Webelement.</param>
        public void ScrollToElement(IWebElement elem)
        {
            JsDriver.ExecuteScript("arguments[0].scrollIntoView(true);", elem);
        }

        /// <summary>
        ///  Executes JS to set value to Element       
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        public void SetColorById(string id, string value)
        {
            JsDriver.ExecuteScript($"document.getElementById('{id}').value='{value}'");
        }

        public string GetValueByID(string id)
        {
            var value = JsDriver.ExecuteScript($"return document.getElementById('{id}').value").ToString();
            return value;
        }

        public string GetValueByClassName(string className)
        {
            var value = JsDriver.ExecuteScript($"return document.getElementsByClassName('{className}').value").ToString();
            return value;
        }

        public string GetValueByName(string name)
        {
            var value = JsDriver.ExecuteScript($"return document.getElementsByName('{name}').value").ToString();
            return value;
        }

        /// <summary>
        /// Switches to default driver content which is the main body frame on the first available window.
        /// </summary>
        public void SwitchToDefaultContent()
        {
            Driver.SwitchTo().DefaultContent();
        }

        /// <summary>
        /// Switches to a specific window which is open, first available window starting at point 0.
        /// </summary>
        /// <param name="window">number of window to switch to</param>
        public void SwitchToWindow(int window)
        {
            Wait().Until(d => d.WindowHandles.Count >= window + 1);
            Driver.SwitchTo().Window(Driver.WindowHandles[window]);
        }

        /// <summary>
        /// Switches to the parent frame on the current window.
        /// </summary>
        public void SwitchToParentFrame()
        {
            Driver.SwitchTo().ParentFrame();
        }

        /// <summary>
        /// switches to a sub-frame within the current window.
        /// </summary>
        /// <param name="frame">todo: describe frame parameter on SwitchToFrame</param>
        /// <param name="subFrame">todo: describe subFrame parameter on SwitchToFrame.</param>
        public void SwitchToFrames(int frame, int subFrame)
        {
            Driver.SwitchTo().Frame(frame).SwitchTo().Frame(subFrame);
        }

        /// <summary>
        /// Changes url which the current driver instance is in control of.
        /// </summary>
        /// <param name="url">Site url.</param>
        public void NavigateTo(string url)
        {
            Driver.Navigate().GoToUrl(url);
        }

        #endregion Public Methods
    }
}
