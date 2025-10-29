using AwardQuick.Overlays;
using Microsoft.Maui.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwardQuick.Services
{
    public class ProgressService
    {
        private static ProgressService? _instance;
        private ProgressOverlay _overlay;

        public static ProgressService Instance => _instance ??= new ProgressService();

        private ProgressService()
        {
            _overlay = new ProgressOverlay();
        }
        public ProgressOverlay GetOverlay()
        {
            return _overlay;
        }
        public async Task ShowProgressAsync()
        {
            // Use IApplication instance from DI instead of the obsolete Application.Current
            if (Application.Current == null || Application.Current.Windows.Count == 0) return;
            var currentWindow = Application.Current.Windows[0];
            if (currentWindow?.Page == null) return;
            Page currentPage = ProgressService.GetVisiblePage();
            // Make sure the overlay is in the visual tree but hidden
            if (_overlay.Parent == null && currentPage != null)
            {
                AbsoluteLayout.SetLayoutBounds(_overlay, new Rect(0, 0, 1, 1));
                AbsoluteLayout.SetLayoutFlags(_overlay, AbsoluteLayoutFlags.All);
                // Find the content area of the page to add our overlay
                if (currentPage is ContentPage contentPage)
                {
                    // If the content isn't already in an AbsoluteLayout, we need to restructure it
                    if (contentPage.Content is not AbsoluteLayout)
                    {
                        var originalContent = contentPage.Content;
                        var absoluteLayout = new AbsoluteLayout();
                        contentPage.Content = absoluteLayout;
                        if (originalContent != null)
                        {
                            absoluteLayout.Children.Add(originalContent);
                            AbsoluteLayout.SetLayoutBounds(originalContent, new Rect(0, 0, 1, 1));
                            AbsoluteLayout.SetLayoutFlags(originalContent, AbsoluteLayoutFlags.All);
                        }
                        absoluteLayout.Children.Add(_overlay);
                    }
                    else
                    {
                        (contentPage.Content as AbsoluteLayout)?.Children.Add(_overlay);
                    }
                }
            }
            // Show the overlay
            _overlay.IsVisible = true;
            await Task.Delay(50); // Small delay to ensure UI updates
        }

        public void HideProgress()
        {
            _overlay.IsVisible = false;
        }

        private static Page GetVisiblePage()
        {
            // Access the main window directly
            if (Application.Current == null || Application.Current.Windows.Count == 0)
                return new ContentPage();
            var currentWindow = Application.Current.Windows[0];
            var mainPage = currentWindow?.Page;
            if (mainPage is Shell shell)
            {
                return shell.CurrentPage;
            }
            else if (mainPage is NavigationPage navPage)
            {
                return navPage.CurrentPage;
            }
#pragma warning disable CS8603 // Possible null reference return.
            return mainPage;
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}
