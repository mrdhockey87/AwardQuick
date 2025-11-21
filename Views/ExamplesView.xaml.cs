using AwardQuick.Overlays;
using AwardQuick.Utilities;

namespace AwardQuick.Views;

public partial class ExamplesView : ContentPage
{
    private ExamplesViewModel ViewModel => (ExamplesViewModel)BindingContext;
    public ProgressOverlay? ProgressOverlay { get; private set; }
    public ExamplesView()
	{
		InitializeComponent();
        ProgressOverlay = ExamplesProgressOverlay;
        ViewModel.SetView(this);
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
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