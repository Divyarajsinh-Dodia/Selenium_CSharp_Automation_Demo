using Common_Framework;
using NUnit.Framework;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_PageObjects.Infrastructure
{
    public class CorePageBase : CoreBase
    {
        public CorePageBase(IWebDriver driver)
        {
            Driver = driver;
        }

        public CorePageBase()
        {
        }

        public T CloseAndSwitchWindow<T>(int window = 0)
    where T : CorePageBase
        {
            Driver.Close();
            SwitchToWindow(window);
            return GetPageObject<T>() as T;
        }

        public T SwitchToNewWindow<T>(int window)
    where T : CorePageBase
        {
            SwitchToWindow(window);
            return GetPageObject<T>() as T;
        }

        public string GetTitle() => Driver.Title;

        public string GetUrl() => Driver.Url;

        public int GetWindowHandlesCount() => Driver.WindowHandles.Count;

        public void WaitForElementToBeClickable(By by, int timeOut = 60000)
        {
            try
            {
                Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0.1);
                Wait(timeOut).Until(d => d.FindElement(by).IsVisible() && d.FindElement(by).Enabled);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Driver.Manage().Timeouts().ImplicitWait = DefaultTimeout;
            }
        }

        public void RefreshPage()
        {
            Driver.Navigate().Refresh();
        }

        public bool WaitForElToBeInvisible(By by, int timeOut = 60000)
        {
            try
            {
                Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0.1);
                var present = Wait(timeOut).Until(d => !d.FindElement(by).IsVisible());
                return present;
            }
            catch (Exception ex)
            {
                TestContext.Progress.WriteLine($"[WaitForElToBeInvisible]: {ex.Message}. Element: {by}");
                return false;
            }
            finally
            {
                Driver.Manage().Timeouts().ImplicitWait = DefaultTimeout;
            }
        }

        public bool WaitForElToBeInvisible(IWebElement element, int timeOut = 60000)
        {
            try
            {
                Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0.1);
                var present = Wait(timeOut).Until(d => !element.IsVisible());
                return present;
            }
            catch (Exception ex)
            {
                TestContext.Progress.WriteLine($"[WaitForElToBeInvisible]: {ex.Message}. Element: {element}");
                return false;
            }
            finally
            {
                Driver.Manage().Timeouts().ImplicitWait = DefaultTimeout;
            }
        }

        public bool WaitForElToExist(By by, int timeOut = 60000)
        {
            try
            {
                Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0.1);
                Wait(timeOut).Until(d => d.FindElement(by).IsVisible());
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                Driver.Manage().Timeouts().ImplicitWait = DefaultTimeout;
            }
        }

        public IWebElement WaitForElToBeVisible(By by, int timeOut = 60000)
        {
            try
            {
                Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0.1);
                Wait(timeOut).Until(d => d.FindElement(by).IsVisible());
                return Driver.FindElement(by);
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                Driver.Manage().Timeouts().ImplicitWait = DefaultTimeout;
            }
        }

        public IWebElement WaitForElToBeVisible(IWebElement element, int timeOut = 60000)
        {
            try
            {
                Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0.1);
                Wait(timeOut).Until(d => element.IsVisible());
                return element;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                Driver.Manage().Timeouts().ImplicitWait = DefaultTimeout;
            }
        }

        public void WaitForElementToBeClickable(IWebElement element, int timeOut = 60000)
        {
            try
            {
                Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0.1);
                Wait(timeOut).Until(d => element.IsVisible() && element.Enabled);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Driver.Manage().Timeouts().ImplicitWait = DefaultTimeout;
            }
        }

        /// <summary>
        /// Switch down through N number of subframes
        /// </summary>
        /// <param name="frames">N frames to transverse</param>
        public void SwitchToFrame(params int[] frames)
        {
            foreach (int f in frames)
            {
                Wait(10000).Until(d => d.FindElements(By.TagName("iframe")).Count >= f);
                Driver.SwitchTo().Frame(f);
            }
        }

        public virtual bool WaitForLoadingToComplete(int timeOut = 5000, int wait = 60000) => false;

        /// <summary>
        /// Performs a Drag and Drop Action
        /// </summary>
        /// <param name="path1">Starting Location of the element</param>
        /// <param name="path2">Ending Location of the element</param>
        protected void DragAndDrop(IWebElement path1, IWebElement path2)
        {
            var action = new Actions(Driver);
            action.DragAndDrop(path1, path2);
            action.Build().Perform();
        }

        /// <summary>
        /// Stitches multiple images together
        /// </summary>
        /// <param name="imagesSource">List of Image Bytes</param>
        /// <returns>Bitmap Image</returns>
        //private static Bitmap StitchImages(List<byte[]> imagesSource)
        //{
        //    var imageHeight = 0;
        //    var imageWidth = 0;
        //    var images = new List<Image>();

        //    foreach (var imageSource in imagesSource)
        //    {
        //        var imageConverter = new ImageConverter();
        //        var image = (Image)imageConverter.ConvertFrom(imageSource);
        //        images.Add(image);
        //        imageHeight += image.Height;
        //        imageWidth = image.Width > imageWidth ? image.Width : imageWidth;
        //    }

        //    var paddingBetweenImages = 4;
        //    var bitmap = new Bitmap(imageWidth, imageHeight + ((images.Count - 1) * paddingBetweenImages));
        //    using (var g = Graphics.FromImage(bitmap))
        //    {
        //        var localHeight = 0;
        //        foreach (var image in images)
        //        {
        //            g.DrawImage(image, 0, localHeight);
        //            localHeight += image.Height + 2;
        //        }
        //    }

        //    return bitmap;
        //}

        /// <summary>
        /// Takes multiple screenshot with scrolling and stitch them together in one file.
        /// </summary>
        /// <param name="completePathForFile">Image file name without extension</param>
        //private void FullPageScreenShot(string completePathForFile)
        //{
        //    var pageWidth = JsDriver.ExecuteScript("return Math.max(document.body.scrollWidth, document.body.offsetWidth, document.documentElement.clientWidth, document.documentElement.scrollWidth, document.documentElement.offsetWidth);").ToString();
        //    var pageHeight = JsDriver.ExecuteScript("return Math.max(document.body.scrollHeight, document.body.offsetHeight, document.documentElement.clientHeight, document.documentElement.scrollHeight, document.documentElement.offsetHeight);").ToString();
        //    int windowHeight = int.TryParse(JsDriver.ExecuteScript("return window.innerHeight;").ToString(), out windowHeight) ? windowHeight : 1080;
        //    int pwidth = int.TryParse(pageWidth, out pwidth) ? pwidth : 1920;
        //    int pheight = int.TryParse(pageHeight, out pheight) ? pheight : 1080;
        //    var scrollHeight = 0;
        //    JsDriver.ExecuteScript("window.scrollTo(0, 0);");
        //    var screenShots = new List<byte[]>();
        //    do
        //    {
        //        scrollHeight += windowHeight;
        //        var ss = ((ITakesScreenshot)Driver).GetScreenshot();
        //        screenShots.Add(ss.AsByteArray);
        //        JsDriver.ExecuteScript($"window.scrollTo(0, {scrollHeight});");
        //        //// There is no other way to make sure that scroll is completed as we would be dealing with
        //        //// different pages. Feel free to change this if you have any better way to make sure scrool is complete.
        //        Thread.Sleep(1000);
        //    }
        //    while (pheight > scrollHeight);

        //    //var finalImage = StitchImages(screenShots);
        //    //finalImage.Save(completePathForFile, ImageFormat.Png);
        //}

        public static string GetElementColor(IWebElement element, string cssValue)
        {
            string elementColor = element.GetCssValue($"{cssValue}");

            String[] hexValue = elementColor.Replace("rgba(", "").Replace(")", "").Split(',');

            hexValue[0] = hexValue[0].Trim();

            int hexValue1 = int.Parse(hexValue[0]);

            hexValue[1] = hexValue[1].Trim();

            int hexValue2 = int.Parse(hexValue[1]);

            hexValue[2] = hexValue[2].Trim();

            int hexValue3 = int.Parse(hexValue[2]);

            hexValue[3] = hexValue[3].Trim();

            int hexValue4 = int.Parse(hexValue[3]);

            string color = String.Format("#{0:X2}{1:X2}{2:X2}", hexValue1, hexValue2, hexValue3);

            return color;
        }
    }
}
