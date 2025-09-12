using AwardQuick.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Net.Http;

namespace AwardQuick.ViewModels
{
    public partial class StatementCitationsViewModel : ObservableObject
    {
        // Preference keys for storing last selected tabs
        private const string MainTabPreferenceKey = "StatementCitations_LastMainTab";
        private const string NestedTabPreferenceKey = "StatementCitations_LastNestedTab";

        [ObservableProperty]
        private string _currentMainTab = "Achievement";

        [ObservableProperty]
        private string _currentNestedTab = "Deployment";

        [ObservableProperty]
        private bool _isInitializing = true;

        [ObservableProperty]
        private string? _htmlContent;

        public StatementCitationsViewModel()
        {
            LoadSavedTabPreferences();
        }

        public void LoadSavedTabPreferences()
        {
            // Load last selected tabs from preferences, default to Achievement/Deployment
            CurrentMainTab = Preferences.Get(MainTabPreferenceKey, "Achievement");
            CurrentNestedTab = Preferences.Get(NestedTabPreferenceKey, "Deployment");

            System.Diagnostics.Debug.WriteLine($"Loaded preferences - Main: {CurrentMainTab}, Nested: {CurrentNestedTab}");
        }

        public void SaveTabPreferences()
        {
            // Only save if not initializing to avoid overwriting during startup
            if (!IsInitializing)
            {
                Preferences.Set(MainTabPreferenceKey, CurrentMainTab);
                Preferences.Set(NestedTabPreferenceKey, CurrentNestedTab);
                System.Diagnostics.Debug.WriteLine($"Saved preferences - Main: {CurrentMainTab}, Nested: {CurrentNestedTab}");
            }
        }

        public int GetMainTabIndex(string mainTab)
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

        public int GetNestedTabIndex(string nestedTab)
        {
            var tabNames = new[] { "Deployment", "Inspection", "Leadership", "Recognition", "Special", "Staff", "Volunteer", "Miscellaneous" };
            return Array.IndexOf(tabNames, nestedTab);
        }

        public void ClearSavedTabPreferences()
        {
            Preferences.Remove(MainTabPreferenceKey);
            Preferences.Remove(NestedTabPreferenceKey);
            System.Diagnostics.Debug.WriteLine("Cleared saved tab preferences");
        }

        public void HandleMainTabSelectionChanged(int selectedTabIndex)
        {
            CurrentMainTab = selectedTabIndex switch
            {
                0 => "Achievement",
                1 => "Opening",
                2 => "Helping",
                3 => "Citations",
                4 => "Closing",
                _ => "Achievement"
            };

            System.Diagnostics.Debug.WriteLine($"Main tab selected: {CurrentMainTab}");

            // Reset to first nested tab when switching main tabs (except for Closing)
            if (CurrentMainTab != "Closing")
            {
                CurrentNestedTab = "Deployment";
            }

            // Save preferences
            SaveTabPreferences();

            // Load content for new selection
            _ = LoadContentAsync();
        }

        public void HandleNestedTabSelectionChanged(int selectedNestedTabIndex)
        {
            var tabNames = new[] { "Deployment", "Inspection", "Leadership", "Recognition", "Special", "Staff", "Volunteer", "Miscellaneous" };

            if (selectedNestedTabIndex >= 0 && selectedNestedTabIndex < tabNames.Length)
            {
                CurrentNestedTab = tabNames[selectedNestedTabIndex];
                System.Diagnostics.Debug.WriteLine($"Nested tab selected: {CurrentNestedTab}");

                // Save preferences
                SaveTabPreferences();

                // Load content for new selection
                _ = LoadContentAsync();
            }
        }

        public async Task LoadContentAsync()
        {
            try
            {
                string contentKey = CurrentMainTab == "Closing"
                    ? "ClosingSentences"
                    : $"{CurrentMainTab}{CurrentNestedTab}";

                System.Diagnostics.Debug.WriteLine($"Loading content for: {contentKey}");

                // Get the file path for the content
                var filePath = GetHtmlFilePath(contentKey);

                if (!string.IsNullOrEmpty(filePath))
                {
                    // Use the new method that includes CSS styling and handles compressed files & uncompressed files
                    // it check the file extension to determine the method to use mdail 9-12-25
                    string? htmlContent = await GeneratedHtml.LoadAndFormatStatementHtmlAsync(filePath);

                    if (!string.IsNullOrEmpty(htmlContent))
                    {
                        HtmlContent = htmlContent;
                    }
                    else
                    {
                        // Load default content if file not found
                        HtmlContent = GenerateDefaultHtml(contentKey);
                    }
                }
                else
                {
                    // Load default content if no file path mapped
                    HtmlContent = GenerateDefaultHtml(contentKey);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading content: {ex.Message}");

                // Load error content
                HtmlContent = GenerateErrorHtml(CurrentMainTab, CurrentNestedTab, ex.Message);
            }
        }

        public string GetHtmlFilePath(string contentKey)
        {
            // Map content keys to actual file paths in Resources/Raw
            return GeneratedHtml.StatementGetHtmlFilePath(contentKey);
        }
        /*
        public async Task<string?> LoadHtmlFromFileAsync(string filePath)
        {
            try
            {
                // Use the new method that includes CSS styling and handles compressed files
                return await GeneratedHtml.LoadAndFormatStatementHtmlAsync(filePath);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading HTML file {filePath}: {ex.Message}");
                return null;
            }
        }*/

        public string GenerateDefaultHtml(string contentKey)
        {
            return GeneratedHtml.StatementDefaultHtml(contentKey, CurrentMainTab, CurrentNestedTab);
        }

        public string GenerateErrorHtml(string mainTab, string nestedTab, string errorMessage)
        {
            return GeneratedHtml.StatementGenerateErrorHtml(mainTab, nestedTab, errorMessage);
        }
    }
}