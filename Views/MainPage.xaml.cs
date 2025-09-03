using AwardQuick.ViewModels;

namespace AwardQuick.Views

{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageViewModel model)
        {
            InitializeComponent();
            BindingContext = model;
            VersionLabel.Text = model.VersionNumber;    
        }

        private async void OnLicenseAgreementClicked(object sender, EventArgs e)
        {
            // Navigate to the License Agreement page
            await Shell.Current.GoToAsync("License");
        }

    }
}