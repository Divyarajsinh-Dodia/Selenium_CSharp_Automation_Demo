using AventStack.ExtentReports;
using Common_Framework;
using Common_Framework.ExtentReport;
using Common_PageObjects.ApplicationInfra.Menu;
using Common_PageObjects.Infrastructure;
using Common_PageObjects.PageObjects;
using Newtonsoft.Json;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using System.Reflection;

namespace Common_Tests.Infrastructure
{
    public abstract class TestBase : TestCore
    {
        private Menu menu;
        private HomePage homePage;
        private static readonly string CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public Menu Menu
        {
            get
            {
                menu = menu ?? new Menu();
                return menu;
            }
        }

        public HomePage HomePage => homePage = homePage ?? new HomePage(Driver);

        private IWebDriver Driver
        {
            get => driver.Value;
            set => driver.Value = value;
        }

        protected T SelectNavItem<T>(MenuOptions options)
            where T : CorePageBase
        {
            var page = new CorePageBase(Driver);
            var title = page.GetTitle();

            SeleniumExtensions.Retry(() =>
            {
                try
                {
                    var x = options.Element;
                    var element = Driver.FindElementWithAutoWait(x);
                    Driver.JavascriptClick(element);
                }
                catch
                {
                    var x = options.Element;
                    var element = Driver.FindElementWithAutoWait(x);
                    Driver.JavascriptClick(element);
                }
            });

            if (page.GetWindowHandlesCount() > 1)
            {
                page.SwitchToWindow(1);
            }

            Thread.Sleep(2500);
            return page.GetPageObject<T>() as T;
        }

        //[SetUp]  // Commenting this for some checking
        //public void Setup()
        //{
        //    TestContext.Progress.WriteLine($"-----TEST: {TestContext.CurrentContext.Test.Name} STARTED at {DateTime.Now.ToLongTimeString()}, (Try # {TestContext.CurrentContext.CurrentRepeatCount + 1})------");
        //    LogTestOutput($"----+TEST: {TestContext.CurrentContext.Test.Name} Started at {DateTime.Now.ToLongTimeString()} (Try # {TestContext.CurrentContext.CurrentRepeatCount + 1}) ------");
        //}

        //NOTE : Updated Testbase so that there will be a new login in each tests --> If need to check all tests in one time login then change below Nunit attributes to OneTimeSetup and OneTimeTearDown

        [OneTimeSetUp]
        protected void OneTimeSetup()
        {
            ExtentService.InitializeExtentReport(CurrentDirectory);
        }

        [OneTimeTearDown]
        protected void OneTimeTearDown()
        {
            ExtentService.Instance.Flush();
            Console.WriteLine($"The report has been exported to: {ExtentService.ReportPath}");

            if (Driver != null)
            {
                Driver.Dispose();
                Driver.Close();
            }
        }

        [SetUp]
        public void InitializeOnce()
        {
            string name = TestContext.CurrentContext.Test.Name;
            string category = "";
            if (TestContext.CurrentContext.Test.Properties.ContainsKey("Name"))
            {
                name = TestContext.CurrentContext.Test.Properties.Get("Name") as string;
            }
            if (TestContext.CurrentContext.Test.Properties.ContainsKey("Category"))
            {
                category = TestContext.CurrentContext.Test.Properties.Get("Category") as string;
            }

            ExtentTestManager.CreateTest(name, category);

            SpecialExtenstions.GetURL(out var siteUrl);
            SpecialExtenstions.GetBrowser(out var browser);
            SpecialExtenstions.GetCredentials(out var nameofuser, out var passwordofuser);
            var browserType = browser;
            TestContext.Progress.WriteLine($"Testing URL: {siteUrl}");
            LogTestOutput($"Testing URL: {siteUrl}");
            Driver = Launch(browserType);
            homePage = new HomePage(Driver);
            var size = Driver.Manage().Window.Size.ToString();
            TestContext.Progress.WriteLine($"Resolution is: {size} ");
            LogTestOutput($"Resolution is: {size} ");
            NavigateToLoginPage(browserType, siteUrl);
            HomePage.NavigateHomePage();
        }

        [TearDown]
        public void Quit()
        {
            LogToReports();
            CloseBrowser();
            if (Driver != null)
            {
                Driver.Dispose();
            }
        }

        private void LogToReports()
        {
            string filename = $"{TestContext.CurrentContext.Test.Name}_{DateTime.Now:h_mm_ss}.png";
            SpecialExtenstions.GetScreenshotOnFail(out var screenshotOnFail);
            SpecialExtenstions.GetScreenshotOnPass(out var screenshotOnPass);
            //string name = TestContext.CurrentContext.Test.Name;
            if (TestContext.CurrentContext.Result.Outcome == ResultState.Success)
            {
                if (screenshotOnPass.ToLower() == "yes")
                {
                    LogTestOutput(TestContext.CurrentContext.Test.Name + " " + " has passed, Please find Screenshot in attachments.");
                    //extent reports

                    var screenshots = ((ITakesScreenshot)Driver).GetScreenshot().AsBase64EncodedString;
                    var trailMessage = "<b>" + "This test has passed." + "</br>" + "<b>" + "Screenshot :";
                    //var mediaEntity = ExtentService.CaptureScreenShot(Driver, filename);
                    ExtentTestManager.GetTest().Pass(trailMessage, MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshots).Build());
                }
                else if (screenshotOnPass.ToLower() == "no")
                {
                    LogTestOutput(TestContext.CurrentContext.Test.Name + " " + " has passed");

                    var trailMessage = "<b>" + "This test has passed." + "</br>";
                    ExtentTestManager.GetTest().Pass(trailMessage);
                }
            }
            else
            {
                if (screenshotOnFail.ToLower() == "yes")
                {
                    LogTestOutput(TestContext.CurrentContext.Test.Name + " " + " has failed, Please find Screenshot in attachments.");

                    // Extent Reports
                    var screenshots = ((ITakesScreenshot)Driver).GetScreenshot().AsBase64EncodedString;
                    var stackTrace = TestContext.CurrentContext.Result.StackTrace;
                    var stackMessage = TestContext.CurrentContext.Result.Message;
                    var trailMessage = "<b>" + "This test has failed please check the below log." + "</br>" +
                                        "<pre>" + stackTrace + "</pre>" + "</br>"
                                        + "<pre>" + stackMessage + "</pre>"
                                        + "</br>" + "<b>" + "Screenshot :";
                    //var mediaEntity = ExtentService.CaptureScreenShot(Driver, filename);
                    ExtentTestManager.GetTest().Fail(trailMessage, MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshots).Build());
                }
                else if (screenshotOnFail.ToLower() == "no")
                {
                    var stackTrace = TestContext.CurrentContext.Result.StackTrace;
                    LogTestOutput(TestContext.CurrentContext.Test.Name + " has failed");
                    LogTestOutput("This Test has failed " + stackTrace);

                    var screenshots = ((ITakesScreenshot)Driver).GetScreenshot().AsBase64EncodedString;
                    var stackMessage = TestContext.CurrentContext.Result.Message;
                    var trailMessage = "<b>" + "This test has failed please check the below log." + "</br>" +
                                        "<pre>" + stackTrace + "</pre>" + "</br>"
                                        + "<pre>" + stackMessage + "</pre>";
                    //var mediaEntity = ExtentService.CaptureScreenShot(Driver, filename);
                    ExtentTestManager.GetTest().Fail(trailMessage);
                }
            }
        }

        public static void GetJsonData(string requiredData, out string neededData)
        {
            dynamic jsonFile = JsonConvert.DeserializeObject(File.ReadAllText("../../../../CommonSetting.json"));
            neededData = jsonFile.SelectToken($"{requiredData}");
        }

        [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
        public sealed class TestCaseIDAttribute : Attribute
        {
            public TestCaseIDAttribute(string id)
            {
                Id = id;
            }

            public string Id { get; }
        }
    }
}
