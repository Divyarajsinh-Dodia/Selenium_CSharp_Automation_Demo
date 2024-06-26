using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Framework
{
    public static class SpecialExtenstions
    {
        public static string GetJsonData(string requiredData)
        {
            dynamic jsonFile = JsonConvert.DeserializeObject(File.ReadAllText("../../../../CommonSetting.json"));
            var neededData = jsonFile.SelectToken($"{requiredData}");
            return neededData;
        }

        public static string GetEnvVariable(string data)
        {
            var neededData = Environment.GetEnvironmentVariable(data);
            return neededData;
        }

        public static void GetURL(out string siteURL)
        {
            siteURL = string.Empty;
            var url = GetEnvVariable("SITE_URL");
            if (url == null || url == string.Empty || url == "site url")
            {
                siteURL = GetJsonData("URL");
            }
            else
            {
                siteURL = url;
            }
        }

        public static void GetBrowser(out string browser)
        {
            browser = string.Empty;
            var browserVariable = GetEnvVariable("BROWSER");
            if (browserVariable == null || browserVariable == string.Empty || browserVariable == "Select Browser")
            {
                var browserName = GetJsonData("BROWSER");
                switch (browserName.ToLower())
                {
                    case "chrome":
                        browser = "CHROME";
                        break;
                    case "chromeheadless":
                        browser = "CHROMEHEADLESS";
                        break;
                    case "firefox":
                        browser = "FIREFOX";
                        break;
                    case "internet explorer":
                        browser = "IE";
                        break;
                    case "edge":
                        browser = "EDGE";
                        break;
                    case "firefoxheadless":
                        browser = "FIREFOXHEADLESS";
                        break;
                    case "edgeheadless":
                        browser = "EDGEHEADLESS";
                        break;
                    case null:
                        browser = "CHROME";
                        break;
                }
            }
            else
            {
                var browserEnv = browserVariable;
                switch (browserEnv.ToLower())
                {
                    case "chrome":
                        browser = "CHROME";
                        break;
                    case "chromeheadless":
                        browser = "CHROMEHEADLESS";
                        break;
                    case "firefox":
                        browser = "FIREFOX";
                        break;
                    case "internet explorer":
                        browser = "IE";
                        break;
                    case "edge":
                        browser = "EDGE";
                        break;
                    case "firefoxheadless":
                        browser = "FIREFOXHEADLESS";
                        break;
                    case "edgeheadless":
                        browser = "EDGEHEADLESS";
                        break;
                    case null:
                        browser = "CHROME";
                        break;
                }
            }
        }

        public static void GetCredentials(out string userName, out string passWord)
        {
            var username = GetEnvVariable("USERNAME");
            var password = GetEnvVariable("PASSWORD");

            if (username == null || password == null || username == string.Empty || password == string.Empty || username == "username" || password == "password")
            {
                userName = GetJsonData("USERNAME");
                passWord = GetJsonData("PASSWORD");
            }
            else
            {
                userName = username;
                passWord = password;
            }
        }

        public static void GetScreenshotOnPass(out string yesOrNo)
        {
            var ssonPass = GetEnvVariable("SCREENSHOT_ON_PASS");
            if (ssonPass == null || ssonPass == string.Empty || ssonPass == "Select")
            {
                yesOrNo = GetJsonData("SCREENSHOT_ON_PASS");
            }
            else
            {
                yesOrNo = ssonPass;
            }
        }

        public static void GetScreenshotOnFail(out string yesOrNo)
        {
            var ssonFail = GetEnvVariable("SCREENSHOT_ON_FAIL");
            if (ssonFail == null || ssonFail == string.Empty || ssonFail == "Select")
            {
                yesOrNo = GetJsonData("SCREENSHOT_ON_FAIL");
            }
            else
            {
                yesOrNo = ssonFail;
            }
        }
    }
}
