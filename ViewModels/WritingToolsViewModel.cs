using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AwardQuick.Utilities;

using CommunityToolkit.Mvvm.ComponentModel;

namespace AwardQuick.ViewModels
{
    public partial class WritingToolsViewModel : ObservableObject
    {
        // Preference keys for storing last selected tabs
        private const string MainTabPreferenceKey = "WritingTools_LastMainTab";
        [ObservableProperty]
        private string _currentMainTab = "Guidelines";
        [ObservableProperty]
        private bool _isInitializing = true;

        [ObservableProperty]
        private string? _htmlContent;
        public WritingToolsViewModel()
        {

        }


        public int GetMainTabIndex(string mainTab)
        {
            return mainTab switch
            {
                "Guidelines" => 0,
                "Word Combos" => 1,
                "Citation Generator" => 2,
                "Phrases" => 3,
                "Adjectives" => 4,
                "Nouns" => 5,
                "Verbs " => 6,
                _ => 0 // Default to Achievement
            };
        }

        public string GenerateDefaultHtml(string contentKey)
        {
            //Need to fix the GeneratedHtml to work for single tabed content _DefaultTab is temp for now mdail 11-21-25
            var _DefaultTab = "";
            return GeneratedHtml.StatementDefaultHtml(contentKey, CurrentMainTab, _DefaultTab);
        }

        public string GenerateErrorHtml(string mainTab, string nestedTab, string errorMessage)
        {
            return GeneratedHtml.StatementGenerateErrorHtml(mainTab, nestedTab, errorMessage);
        }
    }
}
