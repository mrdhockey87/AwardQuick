using AwardQuick.CustomServices;
using AwardQuick.Views;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Font = Microsoft.Maui.Font;

namespace AwardQuick
{
    public partial class AppShell : Shell
    {
        private readonly IDialogService _dialogService;
        private bool isNavigating = false; // Flag to track navigation
        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();
            _dialogService = new ShellDialogService();
            BindingContext = this;
            this.Navigated += OnShellNavigated;
            this.Loaded += OnShellLoaded;
        }

        private void RegisterRoutes()
        {
            // Default views for other platforms
            Routing.RegisterRoute("PdfViewer", typeof(PdfView));
            Routing.RegisterRoute("Examples", typeof(ExamplesView));
            Routing.RegisterRoute("Forms", typeof(FormsView));
            Routing.RegisterRoute("LettersMemos", typeof(LettersMemosView));
            Routing.RegisterRoute("License", typeof(LicenseAgreementView));
            Routing.RegisterRoute("References", typeof(ReferencesView));
            Routing.RegisterRoute("StatementCitations", typeof(StatementCitationsView));
            Routing.RegisterRoute("WebResources", typeof(WebResourcesView));
            Routing.RegisterRoute("WritingTools", typeof(WritingToolsView));
            //Routing.RegisterRoute(nameof(Overlays.CustomDialogPage), typeof(Overlays.CustomDialogPage));
        }
        public async Task OnMenuItemClicked(string route)
        {
            if (isNavigating) return; // Prevent multiple clicks
            isNavigating = true;
            await ProgressService.Instance.ShowProgressAsync();
            await Shell.Current.GoToAsync(route);
            ProgressService.Instance.HideProgress();
            isNavigating = false;
        }
        public async Task OnHomeClicked()
        {
            await OnMenuItemClicked("///MainPage");
        }
        public async Task OnUpdateClicked(string route)
        {
            string msg = "While checking for updates, the program will close automatically.";
            string msg2 = "Click OK to close the program and check for updates.";
            bool answer = await DisplayAlert("Update Notice", msg + Environment.NewLine + msg2, "OK", "Cancel");
            if (answer)
            {
                // Close the app I still need to figure out how to run an external process to check for updates.
                System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
                try
                {
                    //Need to figure out how to run an external process to check for updates.
                    //and probably send the user to the proper place to download the updated app
                }
                catch (Exception ex)
                {
                    // Handle exceptions, for example, if no browser is installed
                    await DisplayAlert("⚠️" + " Error", "Error running update application!", "OK");
                    ProgressService.Instance.HideProgress();
                    Console.WriteLine($"Error opening browser: {ex.Message}");
                }
            }
        }
        public async Task OnSupportClicked(string route)
        {
            try
            {
                //Send to support page if there is a current one or make an overlay that displaies the
                ////phone number at the office for support mdail 9-5-2025
                await OnMenuItemClicked("///" + route);
            }
            catch (Exception ex)
            {
                // Handle exceptions, for example, if no browser is installed
                await DisplayAlert("⚠️" + " Error", "Error opening support page!", "OK");
                ProgressService.Instance.HideProgress();
                Console.WriteLine($"Error Support browser: {ex.Message}");
            }
        }

        public async Task OnAboutClicked(string route)
        {
            try
            {
                //TODO: make an about overlay view mdail 9-5-2025
                string title = "About Award Quick";
                string SerialNumber = "SN-000-000-000";
                string message = $"Award Quick" + Environment.NewLine + "Version: " + AppVersion.AppVersionNo + Environment.NewLine + "Build: " + AppVersion.AppBuildNo +
                     Environment.NewLine + Environment.NewLine + "Serial Number: " + SerialNumber + Environment.NewLine + Environment.NewLine +
                    "Award Quick is a tool designed to assist in the creation of military awards and decorations. " + Environment.NewLine +
                    "It provides templates, references, and writing tools to streamline the award writing process." + Environment.NewLine + Environment.NewLine +
                    "Copyright © 2025  All rights reserved. Mentor Enterprises, Inc." + Environment.NewLine + "(www.MentorEnterprisesInc.com)";
                await _dialogService.ShowAlertAsync(title, message,"OK","about.png");
            }
            catch (Exception ex)
            {
                //Show DisplayAlert with about information or show InforamtionOverlay page and add to overlyay directory mdail 9-3-2025
                //await OnMenuItemClicked("///About");
                await DisplayAlert("⚠️" + " Error", "Error opening about page! Error = " + ex.Message, "OK");
                ProgressService.Instance.HideProgress();
                Console.WriteLine($"Error opening About: {ex.Message}");
            }
        }
        public async Task OnLicensesClicked(string route)
        {
            try
            {
                //go to the 3rd  party licenses page mdail 9-5-2025
            }
            catch (Exception ex)
            {
                //Show DisplayAlert with about information or show InforamtionOverlay page and add to overlyay directory mdail 9-3-2025
                //await OnMenuItemClicked("///About");
                await DisplayAlert("⚠️" + " Error", "Error opening about page!", "OK");
                ProgressService.Instance.HideProgress();
                Console.WriteLine($"Error opening About: {ex.Message}");
            }
        }

        public async Task OnVisitMentorMilClicked(string route)
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
                await DisplayAlert("⚠️" + " Error", "Error opening default browser!", "OK");
                ProgressService.Instance.HideProgress();
                Console.WriteLine($"Error opening browser: {ex.Message}");
            }
        }

        public static async Task DisplaySnackbarAsync(string message)
        {
            CancellationTokenSource cancellationTokenSource = new();

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

        private void SfSegmentedControl_SelectionChanged(object? sender, Syncfusion.Maui.Toolkit.SegmentedControl.SelectionChangedEventArgs e)
        {
            Application.Current!.UserAppTheme = e.NewIndex == 0 ? AppTheme.Light : AppTheme.Dark;
        }

        private void OnShellNavigated(object? sender, ShellNavigatedEventArgs e)
        {
            if (e != null)
            {
            }
        }

        private async void OnShellLoaded(object? sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                if (DeviceInfo.Current.Platform == DevicePlatform.MacCatalyst)
                {
                    // Multiple refresh attempts with different delays
                    /*   await Task.Delay(50);
                       RefreshMenuBar();
                       await Task.Delay(150);
                       RefreshMenuBar();
                       await Task.Delay(300);
                       RefreshMenuBar();*/
                }
            });
        }

    }
}
