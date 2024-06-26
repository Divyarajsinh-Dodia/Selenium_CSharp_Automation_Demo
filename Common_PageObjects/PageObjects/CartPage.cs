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
    public class CartPage : CorePageBase
    {
        private HomePage homePage;
        private LoginSignUpPage loginSignUpPage;
        private ProductPage productPage;
      public CartPage(IWebDriver driver)
        : base(driver)
        {
        }

        public HomePage HomePage => homePage = homePage ?? new HomePage(Driver);
        public LoginSignUpPage LoginSignUpPage => loginSignUpPage = loginSignUpPage ?? new LoginSignUpPage(Driver);
        public ProductPage ProductPage => productPage = productPage ?? new ProductPage(Driver);

        private const string CartTabXpath = "//a[contains(text(),'Cart')]";

        public IWebElement CartTab => Driver.FindElementWithAutoWait(By.XPath(CartTabXpath));

        public CartPage NavigateCartPage()
        {
            var url = Driver.Url;
            CartTab.Click();
            Driver.WaitForUrlChange(url);
            return this;
        }
    }
}
