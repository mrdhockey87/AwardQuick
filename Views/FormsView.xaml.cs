using Syncfusion.Maui.Toolkit.Carousel;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui; // for FileSystem

namespace AwardQuick.Views;

public partial class FormsView : ContentPage
{
    private FormsViewModel ViewModel => (FormsViewModel)BindingContext;
    public FormsView()
    {
        InitializeComponent();
        ViewModel.SetView(this);
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        // Ensure app documents are cleaned before navigating away
        await DeleteAppDocumentFilesAsync();
        await Shell.Current.GoToAsync("///MainPage");
    }

    // Also attempt cleanup when the page disappears (covers other close scenarios)
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        // Fire-and-forget cleanup so UI navigation isn't blocked
        _ = DeleteAppDocumentFilesAsync();
    }

    private static async Task DeleteAppDocumentFilesAsync()
    {
        try
        {
            var dir = FileSystem.AppDataDirectory;
            if (string.IsNullOrWhiteSpace(dir) || !Directory.Exists(dir))
                return;

            // Enumerate files in the app documents/app data directory and delete them.
            // Only delete files (not directories) to keep this focused and safe.
            var files = Directory.GetFiles(dir);
            foreach (var file in files)
            {
                try
                {
                    File.Delete(file);
                    Debug.WriteLine($"Deleted app document file: {file}");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Failed to delete file '{file}': {ex.Message}");
                }
            }

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"DeleteAppDocumentFilesAsync error: {ex.Message}");
        }
    }
}