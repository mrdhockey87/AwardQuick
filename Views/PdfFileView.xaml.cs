using AwardQuick.Models;
using AwardQuick.Overlays;
using PdfFormFramework.Controls;
using PdfFormFramework.Printing;
using PdfFormFramework.Services;
using System.Diagnostics;

namespace AwardQuick.Views;

public partial class PdfFileView : ContentPage
{
    private bool _isLoading;
    private PdfFormData PdfFormData = null;
    private string PdfPageName = Constants.PdfFileName;
    public PdfFileView()
    {
        InitializeComponent(); 
       // _ = LoadPdfFormAsync();
    }

    private async void Page_Loaded(object sender, EventArgs e)
    {
        Debug.WriteLine("Page loaded");
        //await LoadPdfFormAsync();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_isLoading) return;        // prevent re-entrancy
        _isLoading = true;
        try
        {
            await LoadPdfFormAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"OnAppearing load failed: {ex}");
            await DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            _isLoading = false;
        }
    }

    // ADD: matches Clicked="OnPrintClicked" in XAML
    private async void OnPrintClicked(object sender, EventArgs e)
    {
        try
        {
            var pdfPath = FormView.CurrentPdfPath; // <- filled/view copy path
            if (string.IsNullOrEmpty(pdfPath) || !File.Exists(pdfPath))
            {
                await DisplayAlert("Print", "No filled PDF available yet.", "OK");
                return;
            }
            await PdfPrinterHelper.PrintOrEmailAsync(pdfPath);
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
            if (string.IsNullOrEmpty(FormView.CurrentPdfPath) || !File.Exists(FormView.CurrentPdfPath))
            {
                await DisplayAlert("Email", "No filled PDF available yet.", "OK");
                return;
            }
            await PdfPrinterHelper.PromptAndEmailAsync(FormView.CurrentPdfPath);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Email Error", ex.Message, "OK");
        }
    }
    //
    private async void OnPageBackClicked(object sender, EventArgs e)
    {

        try
        {
            // 1) Force the PdfInteractiveFormView out of the tree so Unloaded fires
            await EnsureUnloadAndCleanupAsync();
            // Navigate back safely (works with or without Shell)
            await NavigateBackSafeAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private Task NavigateBackSafeAsync()
    {
        return MainThread.InvokeOnMainThreadAsync(async () =>
        {
            try
            {
                if (Shell.Current is Shell shell)
                {
                    // Prefer popping if this page was pushed
                    if (shell.Navigation?.ModalStack?.Count > 0)
                        await shell.Navigation.PopModalAsync();
                    else if (shell.Navigation?.NavigationStack?.Count > 1)
                        await shell.Navigation.PopAsync();
                    else
                        await shell.GoToAsync("///Examples"); // absolute route as a fallback
                }
                else
                {
                    // Fallback for non-Shell apps
                    if (Navigation?.ModalStack?.Count > 0)
                    {
                        await Navigation.PopModalAsync();
                    }
                    else if (Navigation?.NavigationStack?.Count > 1)
                    {
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        if (Navigation == null)
                        {
                            await DisplayAlert("Error!", "No Navigation available to go back", "OK");
                        }
                        else
                        {
                            await Navigation.PopToRootAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"NavigateBackSafeAsync failed: {ex}");
                await DisplayAlert("Navigation Error", ex.Message, "OK");
            }
        });
    }

    private Task EnsureUnloadAndCleanupAsync()
    {
        try
        {
            // Disconnect the handler (forces native disposal)
            FormView?.Handler?.DisconnectHandler();
            // Remove from visual tree to trigger Unloaded in the control
            if (PdfContainer != null)
                PdfContainer.Content = null;
            // Optional: delete temp files the control created (defensive)
            _ = CleanupTempFilesAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"EnsureUnloadAndCleanup failed: {ex}");
        }
        return Task.CompletedTask;
    }

    private Task CleanupTempFilesAsync()
    {
        try
        {
            var source = (Constants.PdfFileName ?? string.Empty).Trim();
            if (Path.IsPathRooted(source) && source.EndsWith(".gz", StringComparison.OrdinalIgnoreCase))
            {
                var baseName = Path.GetFileNameWithoutExtension(source); // Example.pdf
                var tempName = baseName + "_temp.pdf";                   // Example.pdf_temp.pdf
                var candidate = Path.Combine(FileSystem.CacheDirectory, tempName);
                if (File.Exists(candidate))
                {
                    File.Delete(candidate);
                    Debug.WriteLine($"Deleted temp file: {candidate}");
                }
            }
            foreach (var file in Directory.EnumerateFiles(FileSystem.CacheDirectory, "*_temp.pdf"))
            {
                try { File.Delete(file); } catch { /* ignore */ }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"CleanupTempFiles failed: {ex}");
        }
        return Task.CompletedTask;
    }
    public async Task LoadPdfFormAsync()
    {
        Exception? error = null;
        progressOverlay.Show();

        try
        {
            // Read the current value at runtime (do not cache at construction)
            var source = (Constants.PdfFileName ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(source))
            {
                // packaged fallback (use forward slashes for non-Windows)
                var fallback = OperatingSystem.IsWindows()
                    ? "Examples\\PdfNotFound.pdf.gz"
                    : "Examples/PdfNotFound.pdf.gz";

                await FormView.LoadFromAppAsync(fallback, PdfFormData);
                Debug.WriteLine($"Loaded fallback PDF: {fallback}");
                return;
            }

            // Packaged relative path vs local file
            if (!Path.IsPathRooted(source))
            {
                // Keep the slashes that work for you on Windows; normalize for others
                var packagedPath = OperatingSystem.IsWindows()
                    ? source.Replace('/', '\\')
                    : source.Replace('\\', '/');

                await FormView.LoadFromAppAsync(packagedPath, PdfFormData);
                Debug.WriteLine($"PDF loaded from app: {packagedPath}");
                return;
            }

            // Local filesystem path -> only .gz supported by the control
            if (source.EndsWith(".gz", StringComparison.OrdinalIgnoreCase))
            {
                await FormView.LoadPdfGz(source, PdfFormData);
                Debug.WriteLine($"PDF loaded from local gz: {source}");
                return;
            }

            throw new NotSupportedException("Local .pdf isn’t supported. Use a packaged .gz (LoadFromAppAsync) or a local .gz (LoadPdfGz).");
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