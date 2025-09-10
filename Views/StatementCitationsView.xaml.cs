using Syncfusion.Maui.Toolkit.TabView;

namespace AwardQuick.Views;

public partial class StatementCitationsView : ContentPage
{
    private readonly WebView _sharedWebView;
    private string _currentMainTab = "Achievement";
    private string _currentNestedTab = "Deployment";
    private bool _isInitializing = true;

    // Preference keys for storing last selected tabs
    private const string MainTabPreferenceKey = "StatementCitations_LastMainTab";
    private const string NestedTabPreferenceKey = "StatementCitations_LastNestedTab";

    public StatementCitationsView()
    {
        // Initialize WebView before InitializeComponent
        _sharedWebView = new WebView();

        InitializeComponent();

        // Set the WebView in the first container
        WebViewContainer.Content = _sharedWebView;

        // Load saved tab preferences or use defaults
        LoadSavedTabPreferences();

        // Set initial tab selections
        SetInitialTabSelections();

        // Load initial content
        _ = LoadContentAsync();

        // Enable saving preferences after initialization
        _isInitializing = false;
    }

    private void LoadSavedTabPreferences()
    {
        // Load last selected tabs from preferences, default to Achievement/Deployment
        _currentMainTab = Preferences.Get(MainTabPreferenceKey, "Achievement");
        _currentNestedTab = Preferences.Get(NestedTabPreferenceKey, "Deployment");

        System.Diagnostics.Debug.WriteLine($"Loaded preferences - Main: {_currentMainTab}, Nested: {_currentNestedTab}");
    }

    private void SetInitialTabSelections()
    {
        try
        {
            // Set the main tab selection
            int mainTabIndex = GetMainTabIndex(_currentMainTab);
            if (mainTabIndex >= 0)
            {
                MainTabView.SelectedIndex = mainTabIndex;
            }

            // Set the nested tab selection if applicable
            if (_currentMainTab != "Closing")
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
            _currentMainTab = "Achievement";
            _currentNestedTab = "Deployment";
        }
    }

    private void SetNestedTabSelection()
    {
        try
        {
            int nestedTabIndex = GetNestedTabIndex(_currentNestedTab);
            if (nestedTabIndex >= 0)
            {
                // Set the nested tab based on current main tab
                switch (_currentMainTab)
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

    private int GetMainTabIndex(string mainTab)
    {
        return mainTab switch
        {
            "Achievement" => 0,
            "Opening" => 1,
            "Helping" => 2,
            "Citations" => 3,
            "Closing" => 4,
            _ => 0 // Default to Achievement
        };
    }

    private int GetNestedTabIndex(string nestedTab)
    {
        var tabNames = new[] { "Deployment", "Inspection", "Leadership", "Recognition", "Special", "Staff", "Volunteer", "Miscellaneous" };
        return Array.IndexOf(tabNames, nestedTab);
    }

    private void SaveTabPreferences()
    {
        // Only save if not initializing to avoid overwriting during startup
        if (!_isInitializing)
        {
            Preferences.Set(MainTabPreferenceKey, _currentMainTab);
            Preferences.Set(NestedTabPreferenceKey, _currentNestedTab);
            System.Diagnostics.Debug.WriteLine($"Saved preferences - Main: {_currentMainTab}, Nested: {_currentNestedTab}");
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
        await Shell.Current.GoToAsync("///MainPage");
    }

    private void OnMainTabSelectionChanged(object sender, TabSelectionChangedEventArgs e)
    {
        var selectedTabIndex = e.NewIndex;

        _currentMainTab = selectedTabIndex switch
        {
            0 => "Achievement",
            1 => "Opening",
            2 => "Helping",
            3 => "Citations",
            4 => "Closing",
            _ => "Achievement"
        };

        System.Diagnostics.Debug.WriteLine($"Main tab selected: {_currentMainTab}");

        // Reset to first nested tab when switching main tabs (except for Closing)
        if (_currentMainTab != "Closing")
        {
            _currentNestedTab = "Deployment";
        }

        // Move WebView to the new tab
        MoveWebViewToCurrentTab();

        // Save preferences
        SaveTabPreferences();

        _ = LoadContentAsync();
    }

    private void OnNestedTabSelectionChanged(object sender, TabSelectionChangedEventArgs e)
    {
        var selectedNestedTabIndex = e.NewIndex;
        var tabNames = new[] { "Deployment", "Inspection", "Leadership", "Recognition", "Special", "Staff", "Volunteer", "Miscellaneous" };

        if (selectedNestedTabIndex >= 0 && selectedNestedTabIndex < tabNames.Length)
        {
            _currentNestedTab = tabNames[selectedNestedTabIndex];
            System.Diagnostics.Debug.WriteLine($"Nested tab selected: {_currentNestedTab}");

            // Move WebView to the new tab
            MoveWebViewToCurrentTab();

            // Save preferences
            SaveTabPreferences();

            _ = LoadContentAsync();
        }
    }

    private async Task LoadContentAsync()
    {
        try
        {
            if (_sharedWebView == null) return;

            string contentKey = _currentMainTab == "Closing"
                ? "ClosingSentences"
                : $"{_currentMainTab}{_currentNestedTab}";

            System.Diagnostics.Debug.WriteLine($"Loading content for: {contentKey}");

            // Get the file path for the content
            var filePath = GetHtmlFilePath(contentKey);

            if (!string.IsNullOrEmpty(filePath))
            {
                var htmlContent = await LoadHtmlFromFileAsync(filePath);

                if (!string.IsNullOrEmpty(htmlContent))
                {
                    _sharedWebView.Source = new HtmlWebViewSource { Html = htmlContent };
                }
                else
                {
                    // Load default content if file not found
                    _sharedWebView.Source = new HtmlWebViewSource
                    {
                        Html = GenerateDefaultHtml(contentKey)
                    };
                }
            }
            else
            {
                // Load default content if no file path mapped
                _sharedWebView.Source = new HtmlWebViewSource
                {
                    Html = GenerateDefaultHtml(contentKey)
                };
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading content: {ex.Message}");

            // Load error content
            if (_sharedWebView != null)
            {
                _sharedWebView.Source = new HtmlWebViewSource
                {
                    Html = GenerateErrorHtml(_currentMainTab, _currentNestedTab, ex.Message)
                };
            }
        }
    }

    // Method to clear saved preferences (useful for testing or reset functionality)
    public static void ClearSavedTabPreferences()
    {
        Preferences.Remove(MainTabPreferenceKey);
        Preferences.Remove(NestedTabPreferenceKey);
        System.Diagnostics.Debug.WriteLine("Cleared saved tab preferences");
    }

    private string GetHtmlFilePath(string contentKey)
    {
        // Map content keys to actual file paths in Resources/Raw
        return contentKey switch
        {
            // Achievement Statements
            "AchievementDeployment" => "Statements/Achievement/Achievement-Deployment.html",
            "AchievementInspection" => "Statements/Achievement/Achievement-Inspection.html",
            "AchievementLeadership" => "Statements/Achievement/Achievement-Leadership.html",
            "AchievementRecognition" => "Statements/Achievement/Achievement-Recognition.html",
            "AchievementSpecial" => "Statements/Achievement/Achievement-Special.html",
            "AchievementStaff" => "Statements/Achievement/Achievement-Staff.html",
            "AchievementVolunteer" => "Statements/Achievement/Achievement-Volunteer.html",
            "AchievementMiscellaneous" => "Statements/Achievement/Achievement-Misc.html",

            // Opening Statements
            "OpeningDeployment" => "Statements/Opening/Opening-Deployment.html",
            "OpeningInspection" => "Statements/Opening/Opening-Inspection.html",
            "OpeningLeadership" => "Statements/Opening/Opening-Leadership.html",
            "OpeningRecognition" => "Statements/Opening/Opening-Recognition.html",
            "OpeningSpecial" => "Statements/Opening/Opening-Special.html",
            "OpeningStaff" => "Statements/Opening/Opening-Staff.html",
            "OpeningVolunteer" => "Statements/Opening/Opening-Volunteer.html",
            "OpeningMiscellaneous" => "Statements/Opening/Opening-Misc.html",

            // Helping Statements
            "HelpingDeployment" => "Statements/Helping/Helping-Deployment.html",
            "HelpingInspection" => "Statements/Helping/Helping-Inspection.html",
            "HelpingLeadership" => "Statements/Helping/Helping-Leadership.html",
            "HelpingRecognition" => "Statements/Helping/Helping-Recognition.html",
            "HelpingSpecial" => "Statements/Helping/Helping-Special.html",
            "HelpingStaff" => "Statements/Helping/Helping-Staff.html",
            "HelpingVolunteer" => "Statements/Helping/Helping-Volunteer.html",
            "HelpingMiscellaneous" => "Statements/Helping/Helping-Misc.html",

            // Citations
            "CitationsDeployment" => "Statements/Citations/Citations-Deployment.html",
            "CitationsInspection" => "Statements/Citations/Citations-Inspection.html",
            "CitationsLeadership" => "Statements/Citations/Citations-Leadership.html",
            "CitationsRecognition" => "Statements/Citations/Citations-Recognition.html",
            "CitationsSpecial" => "Statements/Citations/Citations-Special.html",
            "CitationsStaff" => "Statements/Citations/Citations-Staff.html",
            "CitationsVolunteer" => "Statements/Citations/Citations-Volunteer.html",
            "CitationsMiscellaneous" => "Statements/Citations/Citations-Misc.html",

            // Closing Sentences
            "ClosingSentences" => "Statements/Closing/Closing.html",

            _ => null
        };
    }

    private async Task<string?> LoadHtmlFromFileAsync(string filePath)
    {
        try
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync(filePath);
            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading HTML file {filePath}: {ex.Message}");
            return null;
        }
    }

    private string GenerateDefaultHtml(string contentKey)
    {
        return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>{contentKey}</title>
    <style>
        body {{
            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
            line-height: 1.6;
            color: #333;
            padding: 20px;
            background-color: #f9f9f9;
        }}
        .container {{
            background: white;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
        }}
        h1 {{ color: #2c3e50; }}
        .tab-info {{
            background: #e8f4fd;
            border-left: 4px solid #2196F3;
            padding: 10px 15px;
            margin: 15px 0;
        }}
        .preference-info {{
            background: #f0f9ff;
            border-left: 4px solid #0ea5e9;
            padding: 10px 15px;
            margin: 15px 0;
            font-size: 14px;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <h1>{contentKey}</h1>
        <div class='tab-info'>
            <strong>Current Selection:</strong><br>
            Main Tab: {_currentMainTab}<br>
            Sub Tab: {_currentNestedTab}
        </div>
        <div class='preference-info'>
            <strong>Note:</strong> Your tab selection will be remembered for next time.
        </div>
        <p>Content for <strong>{contentKey}</strong> will be loaded here.</p>
        <p>Please add the corresponding HTML file to <code>Resources/Raw/[appropriate subdirectory]</code>.</p>
    </div>
</body>
</html>";
    }

    private string GenerateErrorHtml(string mainTab, string nestedTab, string errorMessage)
    {
        return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Error Loading Content</title>
    <style>
        body {{
            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
            line-height: 1.6;
            color: #333;
            padding: 20px;
            background-color: #f9f9f9;
        }}
        .error-container {{
            background: #fff;
            border: 1px solid #dc3545;
            border-radius: 8px;
            padding: 20px;
            box-shadow: 0 2px 10px rgba(220, 53, 69, 0.1);
        }}
        .error-title {{ color: #dc3545; }}
        .tab-info {{
            background: #f8f9fa;
            border-left: 4px solid #6c757d;
            padding: 10px 15px;
            margin: 15px 0;
        }}
    </style>
</head>
<body>
    <div class='error-container'>
        <h1 class='error-title'>Error Loading Content</h1>
        <div class='tab-info'>
            <strong>Attempted to load:</strong><br>
            Main Tab: {mainTab}<br>
            Sub Tab: {nestedTab}
        </div>
        <p><strong>Error:</strong> {errorMessage}</p>
        <p>Please check that the HTML file exists in the correct Resources/Raw subdirectory.</p>
    </div>
</body>
</html>";
    }
}