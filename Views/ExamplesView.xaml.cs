namespace AwardQuick.Views;

public partial class ExamplesView : ContentPage
{
    private ExamplesViewModel ViewModel => (ExamplesViewModel)BindingContext;
    public ExamplesView()
	{
		InitializeComponent();
        ViewModel.SetView(this);
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///MainPage");
    }
}