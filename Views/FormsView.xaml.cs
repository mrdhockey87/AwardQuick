using Syncfusion.Maui.Toolkit.Carousel;

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
        await Shell.Current.GoToAsync("///MainPage");
    }
}