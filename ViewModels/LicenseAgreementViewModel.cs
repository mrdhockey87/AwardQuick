using System.IO;
using System.Reflection;
using System.Windows.Input;

using AwardQuick.Services;
using AwardQuick.Utilities;
using AwardQuick.Views;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Maui.Storage;

namespace AwardQuick.ViewModels
{
    public partial class LicenseAgreementViewModel : ObservableObject
    {
        private static LicenseAgreementView? LView;
        public void SetView(LicenseAgreementView lview) => LView = lview;
        private string _licenseHtml = "";

        public string LicenseHtml
        {
            get => _licenseHtml;
            set
            {
                _licenseHtml = value;
                OnPropertyChanged();
            }
        }
        public ICommand BackClickCommand { get; }
        public LicenseAgreementViewModel()
        {
            if (LView != null)
            {
                _ = LView.ProgressOverlay.ShowAsync();
            }
            _ = LoadLicenseAsync();
            BackClickCommand = new AsyncRelayCommand(OnBackClicked);
        }
        private async Task OnBackClicked()
        {
            await DeleteAppDocumentFilesUtl.DeleteAllFilesInAppDocumentsFolderAsync();
            await Shell.Current.GoToAsync("..");
        }
        private async Task LoadLicenseAsync()
        {
            try
            {

                if (LView != null)
                {
                    await LView.ProgressOverlay.Hide();
                }
                //When I compress the html files I need to use this code to read them back mdail 9-11-25
                string? html = await GeneratedHtml.LoadAndFormatStatementHtmlAsync("AppLicense/License.html.gz");
                LicenseHtml = html ?? "License content could not be loaded.";
            }
            catch (Exception ex)
            {
                string title = "Error Loading License";
                string msg = "Could not load the license file." + ex.Message;
                string details = "Please contact support if this problem persists.";
                if (LView != null)
                {
                    await LView.ProgressOverlay.Hide();
                }
                LicenseHtml = GeneratedHtml.GenerateLicenseErrorHtml(title, msg, details);
            }
        }
    }
}
