using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwardQuick
{
    public static class AppVersion
    {
        public static string AppVersionNo
        {
            get
            {
                return AppInfo.Current.VersionString;
            }
        }
        public static string AppBuildNo
        {
            get
            {
                return AppInfo.Current.BuildString;
            }
        }
    }
}

/*
 * 
 *  v 2.0004.0012 Got the About page working in the dialog, however it is not looking the way I want it to. It has a thick border around it
 *                and I only want the title to have a background. THe About is using the dialog service which will work for many types
 *                of dialod and replace the dialog aleret the is built in the maui. mdail  9-9-25
 *  v 2.0004.0011 Added the sizes resource directory. Got the flyout menu to work with the custom control. Added images for the menu.
 *                got the line to work on the menu. Some of the methods to display things from the menu are not done yet. Fix some 
 *                missspelled views mdail  9-6-25
 *  v 2.0003.0010 Spent the day trying to get a divider line accross the menu between the 2,so many options but none worked.
 *                the only one that can close was using the _ text however that leave a problem that it will not size for 
 *                different monitor. I an going to have to convert to a custom menu as I started to do at the begining. 
 *                So now I will have to put the control back and redo the menu there. mdail  9-4-25
 *  v 2.0003.0009 Added all the view & view models, layed the Flyout menu out and register all routes in AppShell.xaml.cs
 *          Started AppStyles for using static resource styles, commited the delete of all the stuff that got made when the 
 *          app got made by vs, and all files I just made. renamed pages to views and pagemodels the viewmodels and updated code 
 *          to make the new directory namespaces work. fixed the files missing in the licenses html, probably did some other things too mdail  9-4-25
 *  v 2.2.8 Got app to run in windows, the Text color in the button for the License, Was missing Style, when I put Style in says can't find style
 *          asle says can't find path E:\\_MAUIProjects\\AwardQuick\\AwardQuick\\bin\\Debug\\net9.0-windows10.0.19041.0\\win10-x64\\AppLicenses\\License.html'." mdail  8-29-25
 *  v 2.2.7 finally got it to build without errors. Need to test on all platforms.
 *  v 2.2.6 Addjust spacing on the menu, added menu: to the enu of award quick, made the version label only
 *          AppVersionNo and not the build number. mdail  8-29-25
 *  v 2.2.5 Fixed grid issue on the MainPage. Layout almost right still needs minor tweeks, also need to 
 *          add the click event for the View License Agreement label & make sure versiontext is working mdail  8-28-25
 *  v 2.2.4 Start working on MainPage layout. mdail  8-28-25
 *  v 2.2.3 Added Fluyout menu that always staies open. Added new icon & Imagers. mdail  8-28-25
 *  v 2.0.2 Changed version number to only 3 digits. Fixed the Shell.Resources from strings 
 *          to ImageSource mdail  8-28-25
 *  v 2.0.1 added appshell and other files to start, change icon to aq icon etc.  mdail  8-27-25
 *  v 2.0.0 start project mdail  8-25-25
 *  
 */