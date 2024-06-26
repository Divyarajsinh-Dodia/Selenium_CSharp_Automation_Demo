using Common_Framework;
using Common_PageObjects.Infrastructure;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_PageObjects.PageObjects
{
    public class LoginSignUpPage : CorePageBase
    {
        public LoginSignUpPage(IWebDriver driver)
          : base(driver)
        {
        }

        private const string LoginSignUpTabXpath = "//a[contains(text(),'Signup / Login')]";
        private const string NewUserSignUpHeadingClassName = "signup-form";
        private const string NameTBName = "name";
        private const string EmailTBXpath = "//input[contains(@data-qa,'signup-email')]";
        private const string SignupBtnXpath = "//button[contains(text(),'Signup')]";
        private const string GetNameTBId = "name";
        private const string GetEmailTBId = "email";
        private const string SetTitleId = "id_gender1";
        private const string PasswordTBId = "password";
        private const string BirthDayDDId = "days";
        private const string BirthMonthDDId = "months";
        private const string BirthYearDDId = "years";
        private const string NewsLetterCBId = "newsletter";
        private const string OptInCBId = "optin";
        private const string FirstNameTBId = "first_name";
        private const string LastNameTBId = "last_name";
        private const string CompanyTBId = "company";
        private const string Address1TBId = "address1";
        private const string Address2TBId = "address2";
        private const string CountryDDId = "country";
        private const string StateTBId = "state";
        private const string CityTBId = "city";
        private const string ZipCodeTBId = "zipcode";
        private const string MobileNumberId = "mobile_number";
        private const string CreateAccountBtnXpath = "//button[contains(text(),'Create Account')]";
        private const string AccountCreatedMsgXpath = "//*[contains(text(),'Account Created!')]";
        private const string AcContinueBtnXpath = "//*[contains(@data-qa,'continue-button')]";
        private const string UsernameXpath = "//*[@class='nav navbar-nav']/descendant::a[contains(text(),'Logged in as')]/b";
        private IWebElement LoginSignUpTab => Driver.FindElementWithAutoWait(By.XPath(LoginSignUpTabXpath));
        private IWebElement NewUserSignUpHeading => Driver.FindElementWithAutoWait(By.ClassName(NewUserSignUpHeadingClassName));
        private IWebElement NameTB => Driver.FindElementWithAutoWait(By.Name(NameTBName));
        private IWebElement EmailTB => Driver.FindElementWithAutoWait(By.XPath(EmailTBXpath));
        private IWebElement SignUpBtn => Driver.FindElementWithAutoWait(By.XPath(SignupBtnXpath));
        private IWebElement GetNameTB => Driver.FindElementWithAutoWait(By.Id(GetNameTBId));
        private IWebElement GetEmailTB => Driver.FindElementWithAutoWait(By.Id(GetEmailTBId));
        private IWebElement TitleRB => Driver.FindElementWithAutoWait(By.Id(SetTitleId));
        private IWebElement PasswordTB => Driver.FindElementWithAutoWait(By.Id(PasswordTBId));
        private IWebElement BirthDayDD => Driver.FindElementWithAutoWait(By.Id(BirthDayDDId));
        private IWebElement BirthMonthDD => Driver.FindElementWithAutoWait(By.Id(BirthMonthDDId));
        private IWebElement BirthYearDD => Driver.FindElementWithAutoWait(By.Id(BirthYearDDId));
        private IWebElement NewsletterCB => Driver.FindElementWithAutoWait(By.Id(NewsLetterCBId));
        private IWebElement OptinCB => Driver.FindElementWithAutoWait(By.Id(OptInCBId));
        private IWebElement FirstNameTB => Driver.FindElementWithAutoWait(By.Id(FirstNameTBId));
        private IWebElement LastNameTB => Driver.FindElementWithAutoWait(By.Id(LastNameTBId));
        private IWebElement ComapnyTB => Driver.FindElementWithAutoWait(By.Id(CompanyTBId));
        private IWebElement Address1TB => Driver.FindElementWithAutoWait(By.Id(Address1TBId));
        private IWebElement Address2TB => Driver.FindElementWithAutoWait(By.Id(Address2TBId));
        private IWebElement CountryDD => Driver.FindElementWithAutoWait(By.Id(CountryDDId));
        private IWebElement StateTB => Driver.FindElementWithAutoWait(By.Id(StateTBId));
        private IWebElement CityTB => Driver.FindElementWithAutoWait(By.Id(CityTBId));
        private IWebElement ZipCodeTB => Driver.FindElementWithAutoWait(By.Id(ZipCodeTBId));
        private IWebElement MobileNumberTB => Driver.FindElementWithAutoWait(By.Id(MobileNumberId));
        private IWebElement CreateAccountBtn => Driver.FindElementWithAutoWait(By.XPath(CreateAccountBtnXpath));
        private IWebElement AccountCreatedMsg => Driver.FindElementWithAutoWait(By.XPath(AccountCreatedMsgXpath));
        private IWebElement AcContinueBtn => Driver.FindElementWithAutoWait(By.XPath(AcContinueBtnXpath));
        private IWebElement Username => Driver.FindElementWithAutoWait(By.XPath(UsernameXpath));


        public LoginSignUpPage NavigateLoginSignUpPage()
        {
            var url = Driver.Url;
            LoginSignUpTab.Click();
            Driver.WaitForUrlChange(url);
            return this;
        }

        public LoginSignUpPage IsNewUserSignUpVisible(bool isVisible)
        {
            isVisible = NewUserSignUpHeading.IsVisible();
            return this;
        }

        public LoginSignUpPage EnterName(string name)
        {
            NameTB.SendKeys(name);
            return this;
        }

        public LoginSignUpPage EnterEmail(string email)
        {
            EmailTB.SendKeys(email);
            return this;
        }
        public LoginSignUpPage ClickOnSignUp()
        {
            SignUpBtn.Click();
            return this;
        }
        public LoginSignUpPage GetName(out string name)
        {
            name = GetNameTB.GetValue();
            return this;
        }
        public LoginSignUpPage GetEmail(out string email)
        {
            email = GetEmailTB.GetValue();
            return this;
        }
        public LoginSignUpPage SetTitle()
        {
            TitleRB.Click();
            return this;
        }

        public LoginSignUpPage EnterPassword(string password)
        {
            PasswordTB.SendKeys(password);
            return this;
        }
        public LoginSignUpPage SelectBirthDay(string day)
        {
            SelectElement BDay = new SelectElement(BirthDayDD);
            BDay.SelectByText(day);
            return this;
        }
        public LoginSignUpPage SelectBirthMonth(string month)
        {
            SelectElement BMonth = new SelectElement(BirthMonthDD);
            BMonth.SelectByText(month);
            return this;
        }
        public LoginSignUpPage SelectBithYear(string year)
        {
            SelectElement BYear = new SelectElement(BirthYearDD);
            BYear.SelectByText(year);
            return this;
        }

        public LoginSignUpPage CheckNewsletter(bool truefalse)
        {
            NewsletterCB.Check(truefalse);
            return this;
        }

        public LoginSignUpPage CheckOptIn(bool truefalse)
        {
            OptinCB.Check(truefalse);
            return this;
        }

        public LoginSignUpPage EnterFirstName(string firstname)
        {
            FirstNameTB.SendKeys(firstname);
            return this;
        }

        public LoginSignUpPage EnterLastName(string lastName)
        {
            LastNameTB.SendKeys(lastName);
            return this;
        }

        public LoginSignUpPage EnterCompanyName(string companyname)
        {
            ComapnyTB.SendKeys(companyname);
            return this;
        }

        public LoginSignUpPage EnterAddress1(string address)
        {
            Address1TB.SendKeys(address);
            return this;
        }

        public LoginSignUpPage EnterAddress2(string address)
        {
            Address2TB.SendKeys(address);
            return this;
        }

        public LoginSignUpPage SetCountry(string country)
        {
            SelectElement SelectCountry = new SelectElement(CountryDD);
            SelectCountry.SelectByText(country);
            return this;
        }

        public LoginSignUpPage EnterState(string state)
        {
            StateTB.SendKeys(state);
            return this;
        }

        public LoginSignUpPage EnterCity(string city)
        {
            CityTB.SendKeys(city);
            return this;
        }

        public LoginSignUpPage EnterZipCode(string zipcode)
        {
            ZipCodeTB.SendKeys(zipcode);
            return this;
        }

        public LoginSignUpPage EnterMobileNumber(string mobile)
        {
            MobileNumberTB.SendKeys(mobile);
            return this;
        }

        public LoginSignUpPage ClickCreateAccount()
        {
            CreateAccountBtn.Click();
            return this;
        }

        public LoginSignUpPage IsAccountCreatedMsgVisible(out bool isVisible)
        {
            isVisible = AccountCreatedMsg.IsVisible();
            return this;
        }

        public LoginSignUpPage ClickAcContinue()
        {
            AcContinueBtn.Click();
            return this;
        }

        public LoginSignUpPage GetUsername(out string name)
        {
            name = Username.Text;
            return this;
        }
    }
}
