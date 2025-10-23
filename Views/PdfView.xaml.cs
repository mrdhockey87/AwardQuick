using PdfFormFramework.Services;
using System.Diagnostics;

namespace AwardQuick.Views;

public partial class PdfView : ContentPage
{
    private PdfViewModel ViewModel => (PdfViewModel)BindingContext;

    public PdfView()
    {
        InitializeComponent();
    }

    private async void Page_Loaded(object sender, EventArgs e)
    {
        Debug.WriteLine("Page loaded");
        // initialize/load content here if needed
        await LoadPdfFormAsync();
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

    private async Task LoadPdfFormAsync()
    {
        //Exception? error = null;
        //string? notFoundMsg = null;
        //await Task.CompletedTask;
    }
}