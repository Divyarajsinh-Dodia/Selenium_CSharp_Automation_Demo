using Common_Framework;
using Common_PageObjects.Infrastructure;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_PageObjects.PageObjects
{
    public class ProductPage : CorePageBase
    {
        public ProductPage(IWebDriver driver)
          : base(driver)
        {
        }

        private const string ProductTabXpath = "//a[contains(text(),'Products')]";

        public IWebElement ProductTab => Driver.FindElementWithAutoWait(By.XPath(ProductTabXpath));

        public ProductPage NavigateProductPage()
        {
            var url = Driver.Url;
            ProductTab.Click();
            Driver.WaitForUrlChange(url);
            return this;
        }
    }
}
