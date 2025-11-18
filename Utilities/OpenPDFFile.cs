using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;

namespace AwardQuick.Utilities
{
    public class OpenPDFFile
    {
        public static async Task OpenPdfAsync(string fileName)
        {
            // NOTE: Provided fileName currently unused; keep placeholder for future mapping logic.
            var pdfPath = Path.Combine(FileSystem.AppDataDirectory, "my_document.pdf"); // Assuming the file is saved here
            if (File.Exists(pdfPath))
            {
                await Launcher.OpenAsync(new OpenFileRequest { File = new ReadOnlyFile(pdfPath) });
            }
            else
            {
                await ShowAlert("Error", "PDF document not found.", "OK");
            }
        }

        public static async Task ShowAlert(string title, string message, string cancel)
        {
            try
            {
                // Always marshal to UI thread
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                        await Shell.Current.CurrentPage.DisplayAlertAsync(title, message, cancel);
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ShowAlert failed: {ex.Message}");
            }
        }
    }
}
