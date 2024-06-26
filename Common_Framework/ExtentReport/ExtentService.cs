using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Framework.ExtentReport
{
    public class ExtentService
    {
        /// <summary>
        /// Current instance of the extent report object
        /// </summary>
        public static ExtentReports Instance { get; } = new();

        /// <summary>
        /// File path to the extent report
        /// </summary>
        public static string ReportPath { get; private set; }

        /// <summary>
        /// Initializes the ExtentReport with required system info
        /// </summary>
        /// <param name="currentDirectory">Directory where the the current application is running in</param>
        public static void InitializeExtentReport(string currentDirectory)
        {
            string reportsDirPath = Path.Combine(currentDirectory, "Reports", "Report");
            Directory.CreateDirectory(reportsDirPath);
            ExtentService.ReportPath = Path.Combine(reportsDirPath, $"AutomationDemo_Report.html");
            var htmlReporter = new ExtentSparkReporter(ExtentService.ReportPath);
            htmlReporter.Filter.StatusFilter.As(new Status[] { Status.Fail, Status.Pass, Status.Skip });
            htmlReporter.Config.DocumentTitle = "Automation Demo Report";
            htmlReporter.Config.ReportName = "Automation Report";
            htmlReporter.Config.Theme = AventStack.ExtentReports.Reporter.Config.Theme.Dark;
            htmlReporter.Config.Protocol = AventStack.ExtentReports.Reporter.Config.Protocol.HTTPS;
            Instance.AttachReporter(htmlReporter);
        }

        private static string GetTimestamp(DateTime dateTime)
        {
            return dateTime.ToString("ddMMyyyHHmm");
        }

        private static string GetPrettyTimestamp(DateTime dateTime)
        {
            return dateTime.ToString("dd_MM_yyyy");
        }

        private static string GetImagePath(string folderPath, string testName)
        {
            var imageName = $"{testName}.png";
            return Path.Combine(folderPath, imageName);
        }

        /// <summary>
        /// Takes a new screenshot of the given <c>IWebDriver</c> instance and returns a file path to the screenshot
        /// </summary>
        /// <param name="driver">Driver to take screenshot from</param>
        /// <param name="testName">Test name to create directory structures</param>
        /// <returns></returns>
        public static string GetScreenshotPath(IWebDriver driver, string testName)
        {
            var screenshot = ((ITakesScreenshot)driver).GetScreenshot();

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Screenshots");
            var imagePath = GetImagePath(folderPath, testName);

            Directory.CreateDirectory(folderPath);
            screenshot.SaveAsFile(imagePath);
            return imagePath;
        }
    }
}
