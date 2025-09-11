using AwardQuick.Services;
using AwardQuick.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Storage;
using System.IO;
using System.Reflection;
using System.Windows.Input;

namespace AwardQuick.ViewModels
{
    public partial class LicenseAgreementViewModel : ObservableObject
    {
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
            _ = LoadLicenseAsync();
            BackClickCommand = new AsyncRelayCommand(OnBackClicked);
        }
        private async Task OnBackClicked()
        {
            await Shell.Current.GoToAsync("..");
        }
        private async Task LoadLicenseAsync()
        {
            try
            {
                /*
                using var stream = await FileSystem.OpenAppPackageFileAsync("AppLicense/License.rar");
                using var reader = new StreamReader(stream);
                LicenseHtml = await reader.ReadToEndAsync();*/
                //When I compress the html files I need to use this code to read them back mdail 9-11-25
                string? html = await ReadDecompressedAsset.ReadDecompressedAssetAsync("AppLicense/License.html.gz");
                LicenseHtml = html ?? "License content could not be loaded.";
            }
            catch (Exception ex)
            {
                string title = "Error Loading License";
                string msg = "Could not load the license file." + ex.Message;
                string details = "Please contact support if this problem persists.";
                LicenseHtml = GeneratedHtml.GenerateLicenseErrorHtml(title, msg, details);
            }
        }
    }
}
