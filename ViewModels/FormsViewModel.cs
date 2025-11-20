using AwardQuick.Utilities;
using AwardQuick.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel; // Launcher, Share, etc.

namespace AwardQuick.ViewModels
{
    public partial class FormsViewModel : ObservableObject
    {
        private static FormsView? FrView;
        private readonly AssetMaterializerService _materializer = new();

        public void SetView(FormsView frview) => FrView = frview;

        public IAsyncRelayCommand DAForm638Command { get; }
        public IAsyncRelayCommand DAForm4187Command { get; }
        public IAsyncRelayCommand CitationPageCommand { get; }
        public IAsyncRelayCommand DAForm7594Command { get; }
        public IAsyncRelayCommand NarrativePageCommand { get; }
        public FormsViewModel()
        {
            DAForm638Command = new AsyncRelayCommand(DAForm638CommandExecute);
            DAForm4187Command = new AsyncRelayCommand(DAForm4187CommandExecute);
            DAForm7594Command = new AsyncRelayCommand(DAForm7594CommandExecute);
            CitationPageCommand = new AsyncRelayCommand(CitationPageCommandExecute);
            NarrativePageCommand = new AsyncRelayCommand(NarrativePageCommandExecute);
        }

        private Task DAForm638CommandExecute() => OpenInPdfAsync("ARN32485_DA_FORM_638_003_EFILE_4.pdf.gz");
        private Task DAForm4187CommandExecute() => OpenInPdfAsync("ARN37028_DA_FORM_4187_100_EFILE_1.pdf.gz");
        private Task DAForm7594CommandExecute() => OpenInPdfAsync("DA_FORM_7594.pdf.gz");
        private Task CitationPageCommandExecute() => OpenInPdfAsync("CitationPage.pdf.gz");
        private Task NarrativePageCommandExecute() => OpenInPdfAsync("NarrativePage.pdf.gz");
        // Replace the helper to navigate using the parameter dictionary (no manual encoding) NarrativePageCommand
        private async Task OpenInPdfAsync(string gzipFileName)
        {
            try
            {
                if (FrView != null)
                {
                    await FrView.ProgressOverlay.ShowAsync();
                }
                var packagedPath = gzipFileName.Contains('/') ? gzipFileName : $"Forms\\{gzipFileName}";

                // Try to materialize and open
                string destPath;
                try
                {
                    destPath = await _materializer.MaterializeAssetAsync(packagedPath);
                }
                catch (Exception)
                {
                    if (FrView != null)
                    {
                        await FrView.ProgressOverlay.Hide();
                        await FrView.DisplayAlertAsync("Unable to resolve PDF path.", "Path is null or file missing.", "OK");
                    }
                    return;
                }

                // Set constants to the file name only (as requested)
                Constants.PdfFileName = Path.GetFileName(destPath);

                // Open with the system default viewer
                await _materializer.OpenWithDefaultAppAsync(destPath);
                if (FrView != null)
                {
                    await FrView.ProgressOverlay.Hide();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Navigation failed: {ex}");
                if (FrView != null)
                {
                    await FrView.ProgressOverlay.Hide();
                    await FrView.DisplayAlertAsync("Navigation failed", ex.Message, "OK");
                }
            }
        }
    }
}