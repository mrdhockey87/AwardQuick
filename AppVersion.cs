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
 *  v 2.2.5 Fixed grid issue on the MainPage. Layout almost right still needs minor tweeks, also need to 
 *          add the click event for the View License Agreement label & make sure versiontext is working mdail  10-1-25
 *  v 2.2.4 Start working on MainPage layout. mdail  9-26-25
 *  v 2.2.3 Added Fluyout menu that always staies open. Added new icon & Imagers. mdail  9-12-25
 *  v 2.0.2 Changed version number to only 3 digits. Fixed the Shell.Resources from strings 
 *          to ImageSource mdail  8-28-25
 *  v 2.0.1 added appshell and other files to start, change icon to aq icon etc.  mdail  8-27-25
 *  v 2.0.0 start project mdail  8-25-25
 *  
 */