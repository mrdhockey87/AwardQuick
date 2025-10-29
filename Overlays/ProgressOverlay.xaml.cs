namespace AwardQuick.Overlays;

public partial class ProgressOverlay : ContentView
{
	public ProgressOverlay()
	{
		InitializeComponent();
    }
    public void Show()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            progressOverlay.IsVisible = true;
        });
    }
    public async Task ShowAsync()
    {
        await MainThread.InvokeOnMainThreadAsync(() =>
        {
            progressOverlay.IsVisible = true;
        });
    }

    public async Task Hide()
    {
        await MainThread.InvokeOnMainThreadAsync(() =>
        {
            progressOverlay.IsVisible = false;
            //progressBarOverlay.InvalidateMeasure();
        });
    }
}