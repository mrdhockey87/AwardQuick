using AwardQuick.Services;
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
                using var stream = await FileSystem.OpenAppPackageFileAsync("AppLicense/License.html");
                using var reader = new StreamReader(stream);
                LicenseHtml = await reader.ReadToEndAsync();
            }
            catch (Exception ex)
            {
                LicenseHtml = GenerateErrorHtml(
                    "Error Loading License",
                    "Could not load the license file.",
                    ex.Message,
                    "Please contact support if this problem persists."
                );
            }
        }

        private static string GenerateErrorHtml(string title, string message, string details = "", string suggestion = "") => $@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>{title}</title>
    <style>
        body {{
            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
            line-height: 1.6;
            color: #333;
            max-width: 600px;
            margin: 20px auto;
            padding: 20px;
            background-color: #f8f9fa;
        }}
        
        .error-container {{
            background: #fff;
            border: 1px solid #dc3545;
            border-radius: 8px;
            padding: 20px;
            box-shadow: 0 2px 10px rgba(220, 53, 69, 0.1);
        }}
        
        .error-icon {{
            font-size: 48px;
            text-align: center;
            margin-bottom: 20px;
        }}
        
        .error-title {{
            color: #dc3545;
            font-size: 24px;
            font-weight: bold;
            margin-bottom: 15px;
            text-align: center;
        }}
        
        .error-message {{
            font-size: 16px;
            margin-bottom: 15px;
            color: #495057;
        }}
        
        .error-details {{
            background: #f8f9fa;
            border-left: 4px solid #dc3545;
            padding: 10px 15px;
            margin: 15px 0;
            font-family: 'Courier New', monospace;
            font-size: 14px;
            white-space: pre-line;
            color: #6c757d;
        }}
        
        .error-suggestion {{
            background: #d1ecf1;
            border: 1px solid #bee5eb;
            border-radius: 5px;
            padding: 10px 15px;
            margin-top: 15px;
            color: #0c5460;
        }}
        
        .timestamp {{
            text-align: center;
            font-size: 12px;
            color: #6c757d;
            margin-top: 20px;
            border-top: 1px solid #dee2e6;
            padding-top: 10px;
        }}
    </style>
</head>
<body>
    <div class='error-container'>
        <div class='error-title'>{title}</div>
        <div class='error-message'>{message}</div>
        {(string.IsNullOrEmpty(details) ? "" : $"<div class='error-details'>{details}</div>")}
        {(string.IsNullOrEmpty(suggestion) ? "" : $"<div class='error-suggestion'><strong> Suggestion:</strong> {suggestion}</div>")}
        <div class='timestamp'>Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}</div>
    </div>
</body>
</html>";
    }
}
