using AwardQuick.Utilities;

using Syncfusion.Maui.Toolkit.TabView;

namespace AwardQuick.Views;

public partial class WritingToolsView : ContentPage
{
    private readonly WebView _sharedWebView;
    private WritingToolsViewModel ViewModel => (WritingToolsViewModel)BindingContext;
    public WritingToolsView()
    {
        // Initialize WebView before InitializeComponent
        _sharedWebView = new WebView();
        InitializeComponent();
        // Set the WebView in the first container
        WebViewContainer.Content = _sharedWebView;
        // Subscribe to HtmlContent changes
        ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        // Set initial tab selections
        SetInitialTabSelections();
        // Load initial content
        //_ = ViewModel.LoadContentAsync();
        // Enable saving preferences after initialization
       // ViewModel.IsInitializing = false;
    }

    private void SetInitialTabSelections()
    {
        try
        {
            // Set the main tab selection
            int mainTabIndex = ViewModel.GetMainTabIndex(ViewModel.CurrentMainTab);
            if (mainTabIndex >= 0)
            {
                MainTabView.SelectedIndex = mainTabIndex;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error setting initial tab selections: {ex.Message}");
            // Fall back to defaults
            MainTabView.SelectedIndex = 0; // Achievement
        }
    }

    private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ViewModel.HtmlContent) && !string.IsNullOrEmpty(ViewModel.HtmlContent))
        {
            _sharedWebView.Source = new HtmlWebViewSource { Html = ViewModel.HtmlContent };
        }
    }
    private void MoveWebViewToCurrentTab()
    {
        // Move the WebView to the currently active tab's container
        var currentContainer = GetCurrentTabContainer();
        if (currentContainer != null && _sharedWebView.Parent != currentContainer)
        {
            // Remove from current parent
            if (_sharedWebView.Parent is ContentView oldParent)
            {
                oldParent.Content = null;
            }

            // Add to new parent
            currentContainer.Content = _sharedWebView;
        }
    }

    private ContentView? GetCurrentTabContainer()
    {
        try
        {
            // Find the currently selected tab container based on main and nested tab selection
            var mainTabIndex = MainTabView.SelectedIndex;

        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error getting current tab container: {ex.Message}");
        }

        return null;
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await DeleteAppDocumentFilesUtl.DeleteAllFilesInAppDocumentsFolderAsync();
        await Shell.Current.GoToAsync("///MainPage");
    }
}