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
 *  v 3.008.0024 Changed the name of the progress overlay to avoid naming confilct with the one in the framework mdail 10-29-25
 *  v 3.008.0023 Tried several ways to get the PDF form to open in the PDF viewer, but none worked. mdail 10-29-25
 *  v 3.008.0022 Start adding the framework for the PDF form handling. Added the PdfView and PdfViewModel. mdail  10-23-25
 *  v 2.007.0022 Finished the Examples page, I need to test to make sure the PDFs opens and open the correct ones. mdail  10-6-25
 *  v 2.006.0021 Started the Examples page with the top 4 buttons, the line across and the label just above the line. Still need to add what the
 *                original had on the second page to the bottom of the page. The research into handling open the PDF for the Counseling I determined
 *                that because of the form, it can't be done as a pdf, It would need to be a html then printed as a pdf. mdail  10-3-25
 *  v 2.0006.0020 Fixed the css and html files for the Statement Citations page. The html files now look correct and like the License file. mdail  9-25-25
 *  v 2.0005.0019 Compressed all the html files for the statement citations page. mdail  9-12-25
 *  v 2.0005.0018 Fixed the home screen image text being not clear, added the full.css and the code to add it to the html pages. The pages now look correct
 *                and and have the right font size. mdail  9-12-25
 *  v 2.0005.0017 Changed the Statement Citations page to use it view model, also changed the background color of the selected tab to the same as the 
 *                images and bottom text on the main page, changed that color to be more red & fixed icon too. also added code to use gzip files 
 *                for the html pages, text files & PDF file which use a different method to read and then need to 
 *                use var ms = await ReadDecompressedAssetAsync("document.pdf.gz"); to read, then to display use var bytes = ms.ToArray();.
 *                then display the bytes in the PDF viewer. Set the License page to use the compressed version of the file, still need to compress 
 *                and edit the other html pages to look better. Also added a html helper class so the raw html text goes in there and return to the page
 *                so I do have to have the raw html inside the code pages. mdail  9-11-25
 *  v 2.0005.0016 Added the Statement Citations page and  need to move some of the code to the4 view model. It is using synfusion tab view control
 *                to display the different tabs and subtabs. I added the Html pages but the seem to now have any style to them. I need to add
 *                style to make them look batter. mdail  9-10-25
 *  v 2.0004.0015 Changed the magenta type color to a deeper red color. Changed the SVG files, the color the xaml, and the icon svg file. mdail  9-10-25
 *  v 2.0004.0014 Added font awesome fonts to the project and maui program. Added helpers classes for all 3 font files. mdail  9-10-25
 *  v 2.0004.0013 Fixed the About page to look the way I want it to. Added an optional icon to the title line of the dialog. mdail  9-10-25
 *  v 2.0004.0012 Got the About page working in the dialog, however it is not looking the way I want it to. It has a thick border around it
 *                and I only want the title to have a background. THe About is using the dialog service which will work for many types
 *                of dialog and replace the dialog alert the is built in the maui. mdail  9-9-25
 *  v 2.0004.0011 Added the sizes resource directory. Got the flyout menu to work with the custom control. Added images for the menu.
 *                got the line to work on the menu. Some of the methods to display things from the menu are not done yet. Fix some 
 *                misspelled views mdail  9-6-25
 *  v 2.0003.0010 Spent the day trying to get a divider line across the menu between the 2,so many options but none worked.
 *                the only one that can close was using the _ text however that leave a problem that it will not size for 
 *                different monitor. I an going to have to convert to a custom menu as I started to do at the beginning. 
 *                So now I will have to put the control back and redo the menu there. mdail  9-4-25
 *  v 2.0003.0009 Added all the view & view models, laid the Flyout menu out and register all routes in AppShell.xaml.cs
 *          Started AppStyles for using static resource styles, committed the delete of all the stuff that got made when the 
 *          app got made by vs, and all files I just made. renamed pages to views and page models the view models and updated code 
 *          to make the new directory namespaces work. fixed the files missing in the licenses html, probably did some other things too mdail  9-4-25
 *  v 2.2.8 Got app to run in windows, the Text color in the button for the License, Was missing Style, when I put Style in says can't find style
 *          also says can't find path E:\\_MAUIProjects\\AwardQuick\\AwardQuick\\bin\\Debug\\net9.0-windows10.0.19041.0\\win10-x64\\AppLicenses\\License.html'." mdail  8-29-25
 *  v 2.2.7 finally got it to build without errors. Need to test on all platforms.
 *  v 2.2.6 Adjust spacing on the menu, added menu: to the enu of award quick, made the version label only
 *          AppVersionNo and not the build number. mdail  8-29-25
 *  v 2.2.5 Fixed grid issue on the MainPage. Layout almost right still needs minor tweak, also need to 
 *          add the click event for the View License Agreement label & make sure version text is working mdail  8-28-25
 *  v 2.2.4 Start working on MainPage layout. mdail  8-28-25
 *  v 2.2.3 Added Flyout menu that always stays open. Added new icon & Imagers. mdail  8-28-25
 *  v 2.0.2 Changed version number to only 3 digits. Fixed the Shell.Resources from strings 
 *          to ImageSource mdail  8-28-25
 *  v 2.0.1 added app shell and other files to start, change icon to aq icon etc.  mdail  8-27-25
 *  v 2.0.0 start project mdail  8-25-25
 *  
 */