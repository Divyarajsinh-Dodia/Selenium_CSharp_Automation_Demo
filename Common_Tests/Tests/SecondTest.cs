using Common_PageObjects.ApplicationInfra.Menu;
using Common_PageObjects.PageObjects;
using Common_Tests.Infrastructure;
using NUnit.Framework.Legacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Tests.Tests
{
    [Parallelizable(ParallelScope.All)]
    public class SecondTest : TestBase
    {
        [Test]
        [Parallelizable]
        public void CreateAccount()
        {
            Random Rng = new Random();

            string name = $"DD{Rng.Next(0000, 9999)}";
            string emailinput = $"dd_email{Rng.Next(0000, 9999)}@yopmail.com";
            var page = SelectNavItem<HomePage>(Menu.ApplicationMenu.HomePage);

            page
                .NavigateHomePage()
                .LoginSignUpPage
                .NavigateLoginSignUpPage()
                .EnterName(name)
                .EnterEmail(emailinput)
                .ClickOnSignUp()
                .GetEmail(out var email)
                .GetName(out var outname)
                .SetTitle()
                .EnterPassword("DD@123")
                .SelectBirthDay("16")
                .SelectBirthMonth("December")
                .SelectBithYear("1997")
                .CheckNewsletter(true)
                .CheckOptIn(true)
                .EnterFirstName("DD")
                .EnterLastName("Dodia")
                .EnterCompanyName("DT")
                .EnterAddress1("ABCDEFG")
                .EnterAddress2("EFGHIJK")
                .EnterState("Gujarat")
                .EnterCity("CITY")
                .EnterZipCode("123456789")
                .EnterMobileNumber("52626278929")
                .ClickCreateAccount()
                .IsAccountCreatedMsgVisible(out var isVisible);

            StringAssert.AreEqualIgnoringCase(name, outname, "Name should be same");
            StringAssert.AreEqualIgnoringCase(emailinput, email, "Email should be same");
            ClassicAssert.IsTrue(isVisible, "Account Created message should be visible");
        }

        [Test]
        [NonParallelizable]
        public void CreateAccount1()
        {
            Random Rng = new Random();

            string name = $"DD{Rng.Next(0000, 9999)}";
            string emailinput = $"dd_email{Rng.Next(0000, 9999)}@yopmail.com";
            var page = SelectNavItem<HomePage>(Menu.ApplicationMenu.HomePage);

            page
                .NavigateHomePage()
                .LoginSignUpPage
                .NavigateLoginSignUpPage()
                .EnterName(name)
                .EnterEmail(emailinput)
                .ClickOnSignUp()
                .GetEmail(out var email)
                .GetName(out var outname)
                .SetTitle()
                .EnterPassword("DD@123")
                .SelectBirthDay("16")
                .SelectBirthMonth("December")
                .SelectBithYear("1997")
                .CheckNewsletter(true)
                .CheckOptIn(true)
                .EnterFirstName("DD")
                .EnterLastName("Dodia")
                .EnterCompanyName("DT")
                .EnterAddress1("ABCDEFG")
                .EnterAddress2("EFGHIJK")
                .EnterState("Gujarat")
                .EnterCity("CITY")
                .EnterZipCode("123456789")
                .EnterMobileNumber("52626278929")
                .ClickCreateAccount()
                .IsAccountCreatedMsgVisible(out var isVisible);

            StringAssert.AreEqualIgnoringCase(name, outname, "Name should be same");
            StringAssert.AreEqualIgnoringCase(emailinput, email, "Email should be same");
            ClassicAssert.IsTrue(isVisible, "Account Created message should be visible");
        }

        [Test]
        [NonParallelizable]
        public void CreateAccount2()
        {
            Random Rng = new Random();

            string name = $"DD{Rng.Next(0000, 9999)}";
            string emailinput = $"dd_email{Rng.Next(0000, 9999)}@yopmail.com";
            var page = SelectNavItem<HomePage>(Menu.ApplicationMenu.HomePage);

            page
                .NavigateHomePage()
                .LoginSignUpPage
                .NavigateLoginSignUpPage()
                .EnterName(name)
                .EnterEmail(emailinput)
                .ClickOnSignUp()
                .GetEmail(out var email)
                .GetName(out var outname)
                .SetTitle()
                .EnterPassword("DD@123")
                .SelectBirthDay("16")
                .SelectBirthMonth("December")
                .SelectBithYear("1997")
                .CheckNewsletter(true)
                .CheckOptIn(true)
                .EnterFirstName("DD")
                .EnterLastName("Dodia")
                .EnterCompanyName("DT")
                .EnterAddress1("ABCDEFG")
                .EnterAddress2("EFGHIJK")
                .EnterState("Gujarat")
                .EnterCity("CITY")
                .EnterZipCode("123456789")
                .EnterMobileNumber("52626278929")
                .ClickCreateAccount()
                .IsAccountCreatedMsgVisible(out var isVisible);

            StringAssert.AreEqualIgnoringCase(name, outname, "Name should be same");
            StringAssert.AreEqualIgnoringCase(emailinput, email, "Email should be same");
            ClassicAssert.IsTrue(isVisible, "Account Created message should be visible");
        }

        [Test]
        [Parallelizable]
        public void CreateAccount3()
        {
            Random Rng = new Random();

            string name = $"DD{Rng.Next(0000, 9999)}";
            string emailinput = $"dd_email{Rng.Next(0000, 9999)}@yopmail.com";
            var page = SelectNavItem<HomePage>(Menu.ApplicationMenu.HomePage);

            page
                .NavigateHomePage()
                .LoginSignUpPage
                .NavigateLoginSignUpPage()
                .EnterName(name)
                .EnterEmail(emailinput)
                .ClickOnSignUp()
                .GetEmail(out var email)
                .GetName(out var outname)
                .SetTitle()
                .EnterPassword("DD@123")
                .SelectBirthDay("16")
                .SelectBirthMonth("December")
                .SelectBithYear("1997")
                .CheckNewsletter(true)
                .CheckOptIn(true)
                .EnterFirstName("DD")
                .EnterLastName("Dodia")
                .EnterCompanyName("DT")
                .EnterAddress1("ABCDEFG")
                .EnterAddress2("EFGHIJK")
                .EnterState("Gujarat")
                .EnterCity("CITY")
                .EnterZipCode("123456789")
                .EnterMobileNumber("52626278929")
                .ClickCreateAccount()
                .IsAccountCreatedMsgVisible(out var isVisible);

            StringAssert.AreEqualIgnoringCase(name, outname, "Name should be same");
            StringAssert.AreEqualIgnoringCase(emailinput, email, "Email should be same");
            ClassicAssert.IsTrue(isVisible, "Account Created message should be visible");
        }
    }
}
