using AwardQuick.Views;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Toolkit.Hosting;

namespace AwardQuick
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureSyncfusionToolkit()
                .ConfigureMauiHandlers(handlers =>
                {
#if IOS || MACCATALYST
    				handlers.AddHandler<Microsoft.Maui.Controls.CollectionView, Microsoft.Maui.Controls.Handlers.Items2.CollectionViewHandler2>();
#endif
                })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("SegoeUI-Semibold.ttf", "SegoeSemibold");
                    fonts.AddFont("FluentSystemIcons-Regular.ttf", FluentUI.FontFamily);
                });
            // Register services and pages
            builder.Services.AddSingleton<ExamplesViewModel>();
            builder.Services.AddSingleton<ExamplesView>();
            builder.Services.AddSingleton<FormsViewModel>();
            builder.Services.AddSingleton<FormsView>();
            builder.Services.AddSingleton<LettersMemosViewModel>();
            builder.Services.AddSingleton<LettersMemosView>();
            builder.Services.AddSingleton<LicenseAgreementViewModel>();
            builder.Services.AddSingleton<LicenseAgreementView>();
            builder.Services.AddSingleton<MainPageViewModel>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<ReferencesViewModel>();
            builder.Services.AddSingleton<ReferencesView>();
            builder.Services.AddSingleton<StatementCitationsViewModel>();
            builder.Services.AddSingleton<StatementCitationsView>();
            builder.Services.AddSingleton<WebResourcesViewModel>();
            builder.Services.AddSingleton<WebResourcesView>();
            builder.Services.AddSingleton<WritingToolsViewModel>();
            builder.Services.AddSingleton<WritingToolsView>();
            return builder.Build();
        }
    }
}
