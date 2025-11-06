using AwardQuick.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwardQuick.ViewModels
{
    public partial class FormsViewModel : ObservableObject
    {
        private static FormsView FrView;

        public void SetView(FormsView frview) => FrView = frview;

        public IAsyncRelayCommand DAForm638FCommand { get; }
        public IAsyncRelayCommand DAForm4187Command { get; }
        public FormsViewModel()
        {
            //DAForm638FCommand = new AsyncRelayCommand(OpenInPdfViewAsync"DAForm638.pdf.gz"));
            //DAForm4187Command = new AsyncRelayCommand("DAForm4187.pdf.gz");
            DAForm638FCommand = new AsyncRelayCommand(DAForm638FCommandExecute);
            DAForm4187Command = new AsyncRelayCommand(DAForm4187CommandExecute);
        }

        private Task DAForm638FCommandExecute() => OpenInPdfViewAsync("DAForm638.pdf.gz");
        private Task DAForm4187CommandExecute() => OpenInPdfViewAsync("DAForm4187.pdf.gz");
        // Replace the helper to navigate using the parameter dictionary (no manual encoding)
        private async Task OpenInPdfViewAsync(string gzipFileName)
        {
            try
            {
                var packagedPath = gzipFileName.Contains('/') ? gzipFileName : $"Forms\\{gzipFileName}";

                // Materialize to a local .pdf and capture the path
                //var localPdfPath = await _pdfService.MaterializePdfFromAssetsAsync(packagedPath);
                if (string.IsNullOrWhiteSpace(packagedPath) || !File.Exists(packagedPath))
                {
                    if (FrView != null)
                        await FrView.DisplayAlert("Unable to resolve PDF path.", "Path is null or file missing.", "OK");
                    return;
                }

                Constants.PdfFileName = packagedPath;
                await MainThread.InvokeOnMainThreadAsync(async () => await Shell.Current.GoToAsync("PdfViewer"));
                //await Shell.Current.GoToAsync("PdfViewer");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Navigation failed: {ex}");
                if (FrView != null)
                    await FrView.DisplayAlert("Navigation failed", ex.Message, "OK");
            }
        }
    }
}
}
