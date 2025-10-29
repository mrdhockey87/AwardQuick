using AwardQuick.Models;
using AwardQuick.Overlays;
using PdfFormFramework.Controls;
using PdfFormFramework.Services;
using System.Diagnostics;

namespace AwardQuick.Views;

public partial class PdfView : ContentPage
{
    private PdfFormData PdfFormData = null;
    private string PdfPageName = Constants.PdfFileName;
    public PdfView()
    {
        InitializeComponent(); 
        _ = LoadPdfFormAsync();
    }

    private async void Page_Loaded(object sender, EventArgs e)
    {
        Debug.WriteLine("Page loaded");
        //await LoadPdfFormAsync();
    }

    // ADD: matches Clicked="OnPrintClicked" in XAML
    private async void OnPrintClicked(object sender, EventArgs e)
    {
        try
        {
            // TODO: invoke your print workflow here (e.g., export PDF then open OS print UI)
            await DisplayAlert("Print", "Print action invoked.", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Print Error", ex.Message, "OK");
        }
    }

    // ADD: matches Clicked="OnEmailClicked" in XAML
    private async void OnEmailClicked(object sender, EventArgs e)
    {
        try
        {
            // TODO: invoke your email/share workflow here
            await DisplayAlert("Email", "Email action invoked.", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Email Error", ex.Message, "OK");
        }
    }
    public async Task LoadPdfFormAsync()
    {
        Exception? error = null;
        progressOverlay.Show();

        try
        {
            var source = PdfPageName?.Trim();

            if (string.IsNullOrWhiteSpace(source))
            {
                const string fallback = "Examples/PdfNotFound.pdf.gz";
                await FormView.LoadFromAppAsync(fallback, PdfFormData);
                Debug.WriteLine($"Loaded fallback PDF: {fallback}");
                return;
            }

            // If it's a packaged path (relative, not rooted), use LoadFromAppAsync
            if (!Path.IsPathRooted(source))
            {
                var packagedPath = source.Replace("\\", "/");
                if (!packagedPath.Contains("/"))
                    packagedPath = $"Examples/{packagedPath}";

                await FormView.LoadFromAppAsync(packagedPath, PdfFormData);
                Debug.WriteLine($"PDF loaded from app: {packagedPath}");
                return;
            }

            // If it's a local path: your control supports .gz via LoadPdfGz
            if (source.EndsWith(".gz", StringComparison.OrdinalIgnoreCase))
            {
                await FormView.LoadPdfGz(source, PdfFormData);
                Debug.WriteLine($"PDF loaded from local gz: {source}");
                return;
            }

            // No API in the control for a local .pdf file.
            // Either keep it packaged (.gz) or convert to a .gz local path and use LoadPdfGz.
            throw new NotSupportedException("This viewer does not load local .pdf files directly. Use a packaged .gz (LoadFromAppAsync) or a local .gz (LoadPdfGz).");
        }
        catch (Exception ex)
        {
            error = ex;
            Debug.WriteLine($"Error loading PDF: {ex.Message}");
            Debug.WriteLine(ex.StackTrace);
        }
        finally
        {
            await progressOverlay.Hide();
        }

        if (error is not null)
            await DisplayAlert("Error", $"Failed to load PDF: {error.Message}", "OK");
    }


    public async Task<string?> LocatePdfFileAsync(string fileName)
    {
        // Try multiple possible locations for the PDF file
        List<string> possibleLocations = new()
        {
            fileName,
            Path.Combine(FileSystem.AppDataDirectory, fileName),
            Path.Combine(FileSystem.CacheDirectory, fileName),
            Path.Combine("Resources", "Raw", fileName),
            Path.Combine(AppContext.BaseDirectory, fileName)
        };

        foreach (var path in possibleLocations)
        {
            Debug.WriteLine($"Checking for PDF at: {path}");
            if (File.Exists(path))
            {
                Debug.WriteLine($"Found PDF at: {path}");
                return path;
            }
        }
        // If not found in standard locations, try to copy from app resources
        try
        {
            string targetPath = Path.Combine(FileSystem.CacheDirectory, fileName);
            Debug.WriteLine($"Extracting PDF to: {targetPath}");

            using var stream = await FileSystem.OpenAppPackageFileAsync(fileName);
            using var fileStream = File.Create(targetPath);
            await stream.CopyToAsync(fileStream);

            Debug.WriteLine($"Extracted PDF to: {targetPath}");
            return targetPath;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error extracting PDF: {ex.Message}");
        }

        return null;
    }
}