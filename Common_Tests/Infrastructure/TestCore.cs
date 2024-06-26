using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Common_Framework.Common;

namespace Common_Tests.Infrastructure
{
    public class TestCore
    {
        public Random Rng => new Random();

        protected IWebDriver HelperDriver
        {
            get
            {
                return Driver;
            }
        }

        public static ThreadLocal<IWebDriver> driver = new ThreadLocal<IWebDriver>();

        private IWebDriver Driver
        {
            get => driver.Value;
            set => driver.Value = value;
        }


        protected void LogTestOutput(string logMessage)
        {
            TestContext.WriteLine(logMessage);
        }

        public void NavigateToLoginPage(string browserType, string url)
        {
            Navigate(url);
        }

        private void Navigate(string url)
        {
            Driver.Navigate().GoToUrl(url);

            TestContext.Progress.WriteLine($"============= Execution Started  For {TestContext.CurrentContext.Test.Name}=============");
            LogTestOutput($"============= Execution Started  For {TestContext.CurrentContext.Test.Name}=============");
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(Constants.ImplicitWaitDuration);
        }

        public IWebDriver Launch(string browserType)
        {
            TestContext.Progress.WriteLine($"============= Execution Started  For {TestContext.CurrentContext.Test.MethodName} =============");
            LogTestOutput($"============= Execution Started  For {TestContext.CurrentContext.Test.MethodName} =============");
            var pageTimeOut = "120";

            switch (browserType)
            {
                case "CHROME":
                    {
                        var options = new ChromeOptions();
                        options.AddUserProfilePreference("intl.accept_languages", "nl");
                        options.AddUserProfilePreference("disable-popup-blocking", "true");
                        options.AddUserProfilePreference("download.prompt_for_download", false);
                        options.AddUserProfilePreference("download.directory_upgrade", true);
                        options.AddUserProfilePreference("credentials_enable_service", false);
                        options.AddUserProfilePreference("profile.password_manager_enabled", false);
                        options.AddUserProfilePreference("plugins.always_open_pdf_externally", true);
                        options.AddUserProfilePreference("pdfjs.disabled", true);
                        options.AddArguments("--disable-backgrounding-occluded-windows");
                        options.AddArgument("--no-sandbox");
                        options.AddArgument("start-maximized");
                        options.AddArguments("--disable-dev-shm-usage");
                        Driver = new ChromeDriver(ChromeDriverService.CreateDefaultService(), options, TimeSpan.FromSeconds(int.Parse(pageTimeOut)));
                        break;
                    }

                case "CHROMEHEADLESS":
                    {
                        var options = new ChromeOptions();
                        options.AddUserProfilePreference("intl.accept_languages", "nl");
                        options.AddUserProfilePreference("disable-popup-blocking", "true");
                        options.AddUserProfilePreference("download.prompt_for_download", false);
                        options.AddUserProfilePreference("download.directory_upgrade", true);
                        options.AddUserProfilePreference("credentials_enable_service", false);
                        options.AddUserProfilePreference("profile.password_manager_enabled", false);
                        options.AddUserProfilePreference("plugins.always_open_pdf_externally", true);
                        options.AddUserProfilePreference("pdfjs.disabled", true);
                        options.AddArguments("--disable-backgrounding-occluded-windows");
                        options.AddArgument("--no-sandbox");
                        options.AddArguments("--disable-dev-shm-usage");
                        options.AddArgument("start-maximized");
                        options.AddArgument("--headless=new");
                        options.AddArgument("window-size=1920,1080");
                        Driver = new ChromeDriver(ChromeDriverService.CreateDefaultService(), options, TimeSpan.FromSeconds(int.Parse(pageTimeOut)));
                        TestContext.Progress.WriteLine("Setting Resolution to 1920 X 1080");
                        break;
                    }

                case "IE":
                    {
                        var internetExplorerOptions = new InternetExplorerOptions
                        {
                            IntroduceInstabilityByIgnoringProtectedModeSettings = true,
                            EnableNativeEvents = false,
                            RequireWindowFocus = true,
                            EnsureCleanSession = false,
                            IgnoreZoomLevel = true,
                        };

                        Driver = new InternetExplorerDriver(internetExplorerOptions);
                        break;
                    }

                case "EDGE":
                    {
                        var options = new EdgeOptions();

                        options.AddArgument("disable-gpu");
                        options.AddUserProfilePreference("intl.accept_languages", "en");
                        options.AddUserProfilePreference("disable-popup-blocking", "true");
                        options.AddUserProfilePreference("download.prompt_for_download", false);
                        options.AddUserProfilePreference("download.directory_upgrade", true);
                        options.AddUserProfilePreference("credentials_enable_service", false);
                        options.AddUserProfilePreference("plugins.always_open_pdf_externally", true);
                        options.AddUserProfilePreference("profile.password_manager_enabled", false);
                        options.AddUserProfilePreference("pdfjs.disabled", true);
                        options.AddArguments("--disable-backgrounding-occluded-windows");
                        options.AddArgument("--no-sandbox");
                        options.AddArgument("start-maximized");
                        Driver = new EdgeDriver(options);
                        break;
                    }

                case "FIREFOX":
                    {
                        var options = new FirefoxOptions();
                        var profile = new FirefoxProfile();

                        options.AddArgument("--window-size=1280,1024");
                        options.AddArgument("--width=1280");
                        options.AddArgument("--height=1024");
                        options.AddArgument("-start-maximized");
                        Driver = new FirefoxDriver(FirefoxDriverService.CreateDefaultService(), options, TimeSpan.FromSeconds(int.Parse(pageTimeOut)));
                        break;
                    }

                case "EDGEHEADLESS":
                    {
                        var options = new EdgeOptions();

                        options.AddArgument("disable-gpu");
                        options.AddUserProfilePreference("intl.accept_languages", "en");
                        options.AddUserProfilePreference("disable-popup-blocking", "true");
                        options.AddUserProfilePreference("download.prompt_for_download", false);
                        options.AddUserProfilePreference("download.directory_upgrade", true);
                        options.AddUserProfilePreference("credentials_enable_service", false);
                        options.AddUserProfilePreference("profile.password_manager_enabled", false);
                        options.AddUserProfilePreference("plugins.always_open_pdf_externally", true);
                        options.AddUserProfilePreference("pdfjs.disabled", true);
                        options.AddArguments("--disable-backgrounding-occluded-windows");
                        options.AddArgument("--no-sandbox");
                        options.AddArgument("start-maximized");
                        options.AddArgument("--headless=new");
                        options.AddArgument("window-size=3840,2160");
                        Driver = new EdgeDriver(options);
                        break;
                    }

                case "FIREFOXHEADLESS":
                    {
                        var options = new FirefoxOptions();
                        options.SetPreference(CapabilityType.PageLoadStrategy, "NORMAL");
                        options.SetPreference(CapabilityType.UnhandledPromptBehavior, "ACCEPT");
                        options.SetPreference(CapabilityType.UnexpectedAlertBehavior, "ACCEPT");
                        options.SetPreference("browser.download.folderList", 2);
                        options.SetPreference("browser.download.useDownloadDir", true);
                        options.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/pdf");
                        options.SetPreference("pdfjs.disabled", true);  // disable the built-in PDF viewer
                        options.AddArgument("disable-gpu");
                        options.AddArgument("-headless");
                        options.AddArgument("--width=2560");
                        options.AddArgument("--height=1440");
                        Driver = new FirefoxDriver(options);
                        break;
                    }
                case "SAFARI":
                    {
                        Driver = new SafariDriver();
                        break;
                    }
            }

            TestContext.Progress.WriteLine($"================== {browserType} ==================");
            LogTestOutput($"================== {browserType} ==================");
            return Driver;
        }

        protected void CloseBrowser()
        {
            try
            {
                TestContext.Progress.WriteLine("Teardown started, closing extra windows " + TestContext.CurrentContext.Test.Name);
                LogTestOutput("Teardown started, closing extra windows " + TestContext.CurrentContext.Test.Name);// for Allure Reports
                CloseExtraWindows();
                TestContext.Progress.WriteLine("=======================================");
                LogTestOutput("=======================================");// for Allure Reports
                TestContext.Progress.WriteLine("Teardown... Logout method execution started for " + TestContext.CurrentContext.Test.Name);
                LogTestOutput("Teardown... Logout method execution started for " + TestContext.CurrentContext.Test.Name);// for Allure Reports
                try
                {
                    driver.Value.Quit();
                    driver.Value.Dispose();
                }
                catch
                {
                    TestContext.Progress.WriteLine("=======================================");
                    TestContext.Progress.WriteLine("Teardown... Quiting Browser " + TestContext.CurrentContext.Test.Name);
                    LogTestOutput("=======================================");// for Allure Reports
                    LogTestOutput("Teardown... Quiting Browser " + TestContext.CurrentContext.Test.Name);// for Allure Reports
                    driver.Value.Quit();
                    driver.Value.Dispose();
                }

                TestContext.Progress.WriteLine("=======================================");
                TestContext.Progress.WriteLine("Teardown... Quiting Browser " + TestContext.CurrentContext.Test.Name);
                LogTestOutput("=======================================");// for Allure Reports
                LogTestOutput("Teardown... Quiting Browser " + TestContext.CurrentContext.Test.Name);// for Allure Reports
                Driver.Quit();
                Driver.Dispose();
            }
            catch (Exception)
            {
                TestContext.Progress.WriteLine("Exception Thrown on Teardown, Quiting Browser " + TestContext.CurrentContext.Test.Name);
                LogTestOutput("Exception Thrown on Teardown, Quiting Browser " + TestContext.CurrentContext.Test.Name);
                try
                {
                    Driver.Quit();
                    Driver.Dispose();
                }
                catch
                {
                    Driver.Quit();
                    Driver.Dispose();
                }

                Driver.Quit();
                Driver.Dispose();
            }

            TestContext.Progress.WriteLine(
                $"================= Closing browsers for '{TestContext.CurrentContext.Test.Name}' ===============");
            LogTestOutput(
                $"================= Closing browsers for '{TestContext.CurrentContext.Test.Name}' ===============");
        }

        private void CloseExtraWindows()
        {
            var windowCount = Driver.WindowHandles.Count;
            for (var i = 1; i < windowCount; i++)
            {
                SwitchToWindow(windowCount - i);
                Driver.Close();
            }

            SwitchToWindow(0);
        }

        private void SwitchToWindow(int num)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromMilliseconds(60000));
            wait.Until(d => d.WindowHandles.Count >= num + 1);
            Driver.SwitchTo().Window(Driver.WindowHandles[num]);
        }
    }
}
