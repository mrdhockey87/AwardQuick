namespace AwardQuick.Views;

public partial class ExamplesView : ContentPage
{
	public ExamplesView()
	{
		InitializeComponent();
    }
    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///MainPage");
    }
}