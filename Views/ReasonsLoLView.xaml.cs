using AwardQuick.Overlays;
using AwardQuick.Utilities;

namespace AwardQuick.Views;

public partial class ReasonsLoLView : ContentPage
{
    public ProgressOverlay ProgressOverlay { get; private set; }
    public ReasonsLoLView()
	{
		InitializeComponent();
        ProgressOverlay = progressOverlay;
    }
    // Also attempt cleanup when the page disappears (covers other close scenarios)
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        // Fire-and-forget cleanup so UI navigation isn't blocked
        _ = Task.Run(async () => await DeleteAppDocumentFilesUtl.DeleteAllFilesInAppDocumentsFolderAsync());
    }
}