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
    public partial class ReasonsLoLViewModel : ObservableObject
    {
        private static ReasonsLoLView? RView;
        public void SetView(ReasonsLoLView rview) => RView = rview;
        private string _ReasonsHtml = "";

        public string ReasonsHtml
        {
            get => _ReasonsHtml;
            set
            {
                _ReasonsHtml = value;
                OnPropertyChanged();
            }
        }
        public ICommand BackClickCommand { get; }
        public ReasonsLoLViewModel()
        {
            if (RView != null)
            {
                _ = RView.ProgressOverlay.ShowAsync();
            }
            _ = LoadReasonsAsync();
            BackClickCommand = new AsyncRelayCommand(OnBackClicked);
        }
        private async Task OnBackClicked()
        {
            await DeleteAppDocumentFilesUtl.DeleteAllFilesInAppDocumentsFolderAsync();
            await Shell.Current.GoToAsync("..");
        }

        private async Task LoadReasonsAsync()
        {
            try
            {
                if (RView != null)
                {
                    await RView.ProgressOverlay.Hide();
                }
                //When I compress the html files I need to use this code to read them back mdail 9-11-25
                string? html = await GeneratedHtml.LoadAndFormatStatementHtmlAsync("Letters/lateness.html.gz");
                ReasonsHtml = html ?? "Reasons for Letter of Lateness content could not be loaded.";
            }
            catch (Exception ex)
            {
                string title = "Error Loading Reasons for Letter of Lateness ";
                string msg = "Could not load the Reasons for Letter of Lateness file." + ex.Message;
                string details = "Please contact support if this problem persists.";
                if (RView != null)
                {
                    await RView.ProgressOverlay.Hide();
                }
                ReasonsHtml = GeneratedHtml.GenerateLicenseErrorHtml(title, msg, details);
            }
        }
    }
}