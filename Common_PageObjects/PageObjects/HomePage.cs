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
    public class HomePage : CorePageBase
    {
        private CartPage cartPage;   
        private LoginSignUpPage loginSignUpPage;
        private ProductPage productPage;
        public HomePage(IWebDriver driver)
: base(driver)
        {
        }

        public CartPage CartPage => cartPage = cartPage ?? new CartPage(Driver);
        public LoginSignUpPage LoginSignUpPage => loginSignUpPage = loginSignUpPage ?? new LoginSignUpPage(Driver);
        public ProductPage ProductPage => productPage = productPage ?? new ProductPage(Driver);

        private const string HomeTabXpath = "//a[contains(text(),'Home')]";

        public IWebElement HomeTab => Driver.FindElementWithAutoWait(By.XPath(HomeTabXpath));

        public HomePage NavigateHomePage()
        {
            HomeTab.Click();
            return this;
        }
    }
}
