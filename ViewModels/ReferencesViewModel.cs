using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AwardQuick.Utilities;
using AwardQuick.Views;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AwardQuick.ViewModels
{
    public partial class ReferencesViewModel : ObservableObject
    {
        private readonly IPdfService _pdfService;
        private static ReferencesView? RefView;
        public void SetView(ReferencesView refview) => RefView = refview;
        public ReferencesViewModel() : this(ServiceHelper.GetService<IPdfService>()) { }
        public IAsyncRelayCommand OpenAR600_8_22Command { get; }
        public IAsyncRelayCommand OpenM600_8_22Command { get; }
        public IAsyncRelayCommand OpenAR_670_1Command { get; }
        public IAsyncRelayCommand OpenAR_600_8_2Command { get; }
        public ReferencesViewModel(IPdfService pdfService)
        {
            _pdfService = pdfService;

            OpenAR600_8_22Command = new AsyncRelayCommand(OpenAR600_8_22Execute);
            OpenM600_8_22Command = new AsyncRelayCommand(OpenM600_8_22Execute);
            OpenAR_670_1Command = new AsyncRelayCommand(OpenAR_670_1Execute);
            OpenAR_600_8_2Command = new AsyncRelayCommand(OpenAR_600_8_2Execute);
        }
        private Task OpenAR600_8_22Execute() => OpenInPdfViewAsync("AR_600_8_22.pdf.gz");
        private Task OpenM600_8_22Execute() => OpenInPdfViewAsync("M600_8_22.pdf.gz");
        private Task OpenAR_670_1Execute() => OpenInPdfViewAsync("AR_670_1.pdf.gz");
        private Task OpenAR_600_8_2Execute() => OpenInPdfViewAsync("AR_600_8_2.pdf.gz");
        // Replace the helper to navigate using the parameter dictionary (no manual encoding)
        private async Task OpenInPdfViewAsync(string gzipFileName)
        {
            try
            {
                if (RefView?.ProgressOverlay != null)
                {
                    await RefView.ProgressOverlay.ShowAsync();
                }
                var packagedPath = gzipFileName.Contains('/') ? gzipFileName : $"References\\{gzipFileName}";

                // Materialize to a local .pdf and capture the path
                //var localPdfPath = await _pdfService.MaterializePdfFromAssetsAsync(packagedPath);
                if (string.IsNullOrWhiteSpace(packagedPath) || !File.Exists(packagedPath))
                {
                    if (RefView?.ProgressOverlay != null)
                    {
                        await RefView.ProgressOverlay.Hide();
                        await RefView.DisplayAlertAsync("Unable to resolve PDF path.", "Path is null or file missing.", "OK");
                    }
                    return;
                }

                Constants.PdfFileName = packagedPath;
                Constants.PdfFileViewTitle = "References";
                await MainThread.InvokeOnMainThreadAsync(async () => await Shell.Current.GoToAsync("PdfViewer"));
                if (RefView?.ProgressOverlay != null)
                {
                    await RefView.ProgressOverlay.Hide();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Navigation failed: {ex}");
                if (RefView?.ProgressOverlay != null)
                {
                    await RefView.ProgressOverlay.Hide();
                    await RefView.DisplayAlertAsync("Navigation failed", ex.Message, "OK");
                }
            }
        }
    }
}
