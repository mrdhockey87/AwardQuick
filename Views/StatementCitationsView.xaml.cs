using AwardQuick.Utilities;
using Syncfusion.Maui.Toolkit.TabView;

namespace AwardQuick.Views;

public partial class StatementCitationsView : ContentPage
{
    private readonly WebView _sharedWebView;
    private StatementCitationsViewModel ViewModel => (StatementCitationsViewModel)BindingContext;

    public StatementCitationsView()
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
        _ = ViewModel.LoadContentAsync();
        // Enable saving preferences after initialization
        ViewModel.IsInitializing = false;
    }

    private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ViewModel.HtmlContent) && !string.IsNullOrEmpty(ViewModel.HtmlContent))
        {
            _sharedWebView.Source = new HtmlWebViewSource { Html = ViewModel.HtmlContent };
        }
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

            // Set the nested tab selection if applicable
            if (ViewModel.CurrentMainTab != "Closing")
            {
                // Use Dispatcher to delay nested tab selection until after the main tab is loaded
                Dispatcher.DispatchDelayed(TimeSpan.FromMilliseconds(100), () => {
                    SetNestedTabSelection();
                });
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error setting initial tab selections: {ex.Message}");
            // Fall back to defaults
            MainTabView.SelectedIndex = 0; // Achievement
            ViewModel.CurrentMainTab = "Achievement";
            ViewModel.CurrentNestedTab = "Deployment";
        }
    }

    private void SetNestedTabSelection()
    {
        try
        {
            int nestedTabIndex = ViewModel.GetNestedTabIndex(ViewModel.CurrentNestedTab);
            if (nestedTabIndex >= 0)
            {
                // Set the nested tab based on current main tab
                switch (ViewModel.CurrentMainTab)
                {
                    case "Achievement":
                        if (AchievementNestedTabView != null)
                            AchievementNestedTabView.SelectedIndex = nestedTabIndex;
                        break;
                    case "Opening":
                        if (OpeningNestedTabView != null)
                            OpeningNestedTabView.SelectedIndex = nestedTabIndex;
                        break;
                    case "Helping":
                        if (HelpingNestedTabView != null)
                            HelpingNestedTabView.SelectedIndex = nestedTabIndex;
                        break;
                    case "Citations":
                        if (CitationsNestedTabView != null)
                            CitationsNestedTabView.SelectedIndex = nestedTabIndex;
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error setting nested tab selection: {ex.Message}");
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

            if (mainTabIndex == 4) // Closing Sentences
            {
                // Get the ContentView from the Closing Sentences tab
                if (MainTabView.Items.Count > 4 && MainTabView.Items[4].Content is ContentView closingContainer)
                {
                    return closingContainer;
                }
            }
            else
            {
                // Get the nested tab view
                SfTabView? nestedTabView = mainTabIndex switch
                {
                    0 => AchievementNestedTabView,
                    1 => OpeningNestedTabView,
                    2 => HelpingNestedTabView,
                    3 => CitationsNestedTabView,
                    _ => null
                };

                if (nestedTabView != null && nestedTabView.SelectedIndex >= 0 && nestedTabView.SelectedIndex < nestedTabView.Items.Count)
                {
                    var selectedNestedItem = nestedTabView.Items[nestedTabView.SelectedIndex];
                    if (selectedNestedItem.Content is ContentView container)
                    {
                        return container;
                    }
                }
            }
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

    private void OnMainTabSelectionChanged(object sender, TabSelectionChangedEventArgs e)
    {
        // Delegate business logic to ViewModel
        ViewModel.HandleMainTabSelectionChanged(e.NewIndex);

        // Handle UI-specific logic
        MoveWebViewToCurrentTab();
    }

    private void OnNestedTabSelectionChanged(object sender, TabSelectionChangedEventArgs e)
    {
        // Delegate business logic to ViewModel  
        ViewModel.HandleNestedTabSelectionChanged(e.NewIndex);

        // Handle UI-specific logic
        MoveWebViewToCurrentTab();
    }
    // Also attempt cleanup when the page disappears (covers other close scenarios)
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        // Fire-and-forget cleanup so UI navigation isn't blocked
        _ = Task.Run(async () => await DeleteAppDocumentFilesUtl.DeleteAllFilesInAppDocumentsFolderAsync());
    }
}