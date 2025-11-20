using AwardQuick.Overlays;
using AwardQuick.Utilities;

namespace AwardQuick.Pages
{
    public partial class LicenseAgreementView : ContentPage
    {
        public ProgressOverlay ProgressOverlay { get; private set; }
        private LicenseAgreementViewModel ViewModel => (LicenseAgreementViewModel)BindingContext;
        public LicenseAgreementView()
        {
            InitializeComponent();
            ProgressOverlay = progressOverlay;
            ViewModel.SetView(this);
        }
        // Also attempt cleanup when the page disappears (covers other close scenarios)
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            // Fire-and-forget cleanup so UI navigation isn't blocked
            _ = Task.Run(async () => await DeleteAppDocumentFilesUtl.DeleteAllFilesInAppDocumentsFolderAsync());
        }
    }
}