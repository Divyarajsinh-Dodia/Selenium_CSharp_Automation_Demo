using Common_PageObjects.ApplicationInfra.Menu.AppMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_PageObjects.ApplicationInfra.Menu
{
    public class Menu
    {
        private ApplicationMenu appMenu;

        public ApplicationMenu ApplicationMenu => appMenu = appMenu ?? new ApplicationMenu();
    }
}
