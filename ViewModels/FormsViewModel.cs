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

        public void SetView(FormsView frview) => FrView = frview;

        public IAsyncRelayCommand DAForm638Command { get; }
        public IAsyncRelayCommand DAForm4187Command { get; }
        public IAsyncRelayCommand CitationPageCommand { get; }
        public IAsyncRelayCommand DAForm7594Command { get; }
        public IAsyncRelayCommand NarrativePageCommand { get; }
        public FormsViewModel()
        {
            //DAForm638FCommand = new AsyncRelayCommand(OpenInPdfViewAsync"DAForm638.pdf.gz"));
            //DAForm4187Command = new AsyncRelayCommand("DAForm4187.pdf.gz");
            DAForm638Command = new AsyncRelayCommand(DAForm638CommandExecute);
            DAForm4187Command = new AsyncRelayCommand(DAForm4187CommandExecute);
            DAForm7594Command = new AsyncRelayCommand(DAForm7594CommandExecute);
            CitationPageCommand = new AsyncRelayCommand(CitationPageCommandExecute);
            NarrativePageCommand = new AsyncRelayCommand(NarrativePageCommandExecute);
        }

        private Task DAForm638CommandExecute() => OpenInPdfViewAsync("ARN32485_DA_FORM_638_003_EFILE_4.pdf.gz");
        private Task DAForm4187CommandExecute() => OpenInPdfViewAsync("ARN37028_DA_FORM_4187_100_EFILE_1.pdf.gz");
        private Task DAForm7594CommandExecute() => OpenInPdfViewAsync("DA_FORM_7594.pdf.gz");
        private Task CitationPageCommandExecute() => OpenInPdfViewAsync("CitationPage.pdf.gz");
        private Task NarrativePageCommandExecute() => OpenInPdfViewAsync("NarrativePage.pdf.gz");
        // Replace the helper to navigate using the parameter dictionary (no manual encoding) NarrativePageCommand
        private async Task OpenInPdfViewAsync(string gzipFileName)
        {
            try
            {
                var packagedPath = gzipFileName.Contains('/') ? gzipFileName : $"Forms\\{gzipFileName}";

                // Ensure packaged asset exists in the app package
                try
                {
                    // Attempt to open package file to ensure it exists
                    using var test = await FileSystem.OpenAppPackageFileAsync(packagedPath);
                }
                catch
                {
                    if (FrView != null)
                        await FrView.DisplayAlertAsync("Unable to resolve PDF path.", "Path is null or file missing.", "OK");
                    return;
                }

                // Build destination file name and path (remove only a single trailing .gz if present)
                var targetFileName = gzipFileName.EndsWith(".gz", StringComparison.OrdinalIgnoreCase)
                    ? gzipFileName[..^3] // remove ".gz"
                    : gzipFileName;
                // Ensure it has .pdf extension
                if (!targetFileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                    targetFileName = Path.ChangeExtension(targetFileName, ".pdf");

                var destPath = Path.Combine(FileSystem.AppDataDirectory, targetFileName);

                // Materialize the PDF bytes to the destination path
                if (packagedPath.EndsWith(".gz", StringComparison.OrdinalIgnoreCase))
                {
                    // Use the utility that returns a MemoryStream of the decompressed PDF
                    var ms = await ReadDecompressedAsset.ReadDecompressedPDFAssetAsync(packagedPath);
                    ms.Position = 0;
                    await using (var outFs = File.Create(destPath))
                    {
                        ms.Position = 0;
                        await ms.CopyToAsync(outFs);
                    }
                    try { ms.Dispose(); } catch { }
                }
                else
                {
                    // Copy the raw PDF file from the app package to dest
                    using var src = await FileSystem.OpenAppPackageFileAsync(packagedPath);
                    await using var outFs = File.Create(destPath);
                    src.Position = 0;
                    await src.CopyToAsync(outFs);
                }

                // Set Constants.PdfFileName to the file name (without path) as requested
                Constants.PdfFileName = Path.GetFileName(destPath);

                // Open with the system default PDF viewer
                var result = new ReadOnlyFile(destPath);
                await Launcher.OpenAsync(new OpenFileRequest { File = result, Title = Constants.PdfFileName });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Navigation failed: {ex}");
                if (FrView != null)
                    await FrView.DisplayAlertAsync("Navigation failed", ex.Message, "OK");
            }
        }
    }
}