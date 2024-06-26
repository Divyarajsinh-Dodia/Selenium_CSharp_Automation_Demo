using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_PageObjects.ApplicationInfra.Menu.AppMenu
{
    public class ApplicationMenu : Menu
    {
        public By HomeElement => By.XPath("//a[contains(text(),'Home')]");

        //MenuOptions

        public MenuOptions HomePage => new MenuOptions
        {
            Element = HomeElement,
        };
    }
}
