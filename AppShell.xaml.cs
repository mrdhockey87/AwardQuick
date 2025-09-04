using AwardQuick.Views;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Font = Microsoft.Maui.Font;

namespace AwardQuick
{
    public partial class AppShell : Shell
    {
        private bool isNavigating = false; // Flag to track navigation
        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();
            //var currentTheme = Application.Current!.RequestedTheme;
            //ThemeSegmentedControl.SelectedIndex = currentTheme == AppTheme.Light ? 0 : 1;
        }

        private void RegisterRoutes()
        {
            // Default views for other platforms
            Routing.RegisterRoute("Examples", typeof(ExamplesView));
            Routing.RegisterRoute("Forms", typeof(FormsView));
            Routing.RegisterRoute("LettersMemos", typeof(LettersMemosView));
            Routing.RegisterRoute("License", typeof(LicenseAgreementView));
            Routing.RegisterRoute("References", typeof(ReferencesView));
            Routing.RegisterRoute("StatmentCitations", typeof(StatmentCitationsView));
            Routing.RegisterRoute("WebResources", typeof(WebResourcesView));
            Routing.RegisterRoute("WrittingTools", typeof(WrittingToolsView));
        }
        public async Task OnMenuItemClicked(string route)
        {
            if (isNavigating) return; // Prevent multiple clicks
            isNavigating = true;
            Shell.Current.FlyoutIsPresented = false;
            await ProgressService.Instance.ShowProgressAsync();
            await Shell.Current.GoToAsync(route);
            ProgressService.Instance.HideProgress();
            isNavigating = false;
        }
        public async Task OnHomeClicked()
        {
            await OnMenuItemClicked("///MainPage");
        }
        private async void OnUpdateClicked(object sender, EventArgs e)
        {
            string msg = "While checking for updates, the program will close automatically.";
            string msg2 = "Click OK to close the program and check for updates.";
            bool answer = await DisplayAlert("Update Notice", msg + Environment.NewLine + msg2, "OK", "Cancel");
            if(answer)
            {
                // Close the app I still need to figure out how to run an external process to check for updates.
                System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
            }
        }

        private async void OnSupportClicked(object sender, EventArgs e)
        {
            if (sender is MenuItem menuItem)
            {
                //Figure out how to open a URL in default browser
            }
        }

        private async void OnAboutClicked(object sender, EventArgs e)
        {
            //Show DisplayAlert with about information or show InforamtionOverlay page and add to overlyay directory mdail 9-3-2025
            //await OnMenuItemClicked("About");
            await DisplayAlert("⚠️" + " Error", "Error opening default browser!", "OK");
        }

        private async void OnVisitMentorMilClicked(object sender, EventArgs e)
        {
            //Figure out how to open a URL in default browser on MentorMilitary.com web  site mdail 9-3-2025
            try
            {
                Uri uri = new("https://www.MentorMilitary.com"); // Replace with your desired URL
                await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
                // Handle exceptions, for example, if no browser is installed
                await DisplayAlert("⚠️" +  " Error", "Error opening default browser!", "OK");
                Console.WriteLine($"Error opening browser: {ex.Message}");
            }
        }

        public static async Task DisplaySnackbarAsync(string message)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Color.FromArgb("#FF3300"),
                TextColor = Colors.White,
                ActionButtonTextColor = Colors.Yellow,
                CornerRadius = new CornerRadius(0),
                Font = Font.SystemFontOfSize(18),
                ActionButtonFont = Font.SystemFontOfSize(14)
            };

            var snackbar = Snackbar.Make(message, visualOptions: snackbarOptions);

            await snackbar.Show(cancellationTokenSource.Token);
        }

        public static async Task DisplayToastAsync(string message)
        {
            // Toast is currently not working in MCT on Windows
            if (OperatingSystem.IsWindows())
                return;

            var toast = Toast.Make(message, textSize: 18);

            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            await toast.Show(cts.Token);
        }

        private void SfSegmentedControl_SelectionChanged(object sender, Syncfusion.Maui.Toolkit.SegmentedControl.SelectionChangedEventArgs e)
        {
            Application.Current!.UserAppTheme = e.NewIndex == 0 ? AppTheme.Light : AppTheme.Dark;
        }
    }
}
