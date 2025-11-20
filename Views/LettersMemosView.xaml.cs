using AwardQuick.Overlays;
using AwardQuick.Utilities;

namespace AwardQuick.Views;

public partial class LettersMemosView : ContentPage
{
    private LettersMemosViewModel ViewModel => (LettersMemosViewModel)BindingContext;
    public ProgressOverlay ProgressOverlay { get; private set; }

    public LettersMemosView()
	{
		InitializeComponent();
        ProgressOverlay = progressOverlay;
        ViewModel.SetView(this);
    }
    private async void OnBackClicked(object sender, EventArgs e)
    {
        // Ensure app documents are cleaned before navigating away
        await DeleteAppDocumentFilesUtl.DeleteAllFilesInAppDocumentsFolderAsync();
        await Shell.Current.GoToAsync("///MainPage");
    }
    // Also attempt cleanup when the page disappears (covers other close scenarios)
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        // Fire-and-forget cleanup so UI navigation isn't blocked
        _ = Task.Run(async () => await DeleteAppDocumentFilesUtl.DeleteAllFilesInAppDocumentsFolderAsync());
    }
}