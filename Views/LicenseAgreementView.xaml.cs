namespace AwardQuick.Pages
{
    public partial class LicenseAgreementView : ContentPage
    {
        public LicenseAgreementView()
        {
            InitializeComponent();
            // Subscribe to property changes
            /*
            if (BindingContext is LicenseAgreementViewModel viewModel)
            {
                viewModel.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(LicenseAgreementViewModel.LicenseHtml))
                    {
                        System.Diagnostics.Debug.WriteLine("LicenseHtml property changed in UI");
                    }
                };
           */
        }
    }
}