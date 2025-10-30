using AwardQuick.Views;
using CommunityToolkit.Maui;
using Maui.PDFView;
using Microsoft.Extensions.Logging;
using PdfSharp.Fonts;
using Syncfusion.Maui.Toolkit.Hosting;

namespace AwardQuick
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            // Initialize PDFsharp font resolver
            GlobalFontSettings.FontResolver = new PdfFormFramework.Services.SystemFontResolver();
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureSyncfusionToolkit()
                .UseMauiPdfView()
                .ConfigureMauiHandlers(handlers =>
                {
#if IOS || MACCATALYST
    				handlers.AddHandler<Microsoft.Maui.Controls.CollectionView, Microsoft.Maui.Controls.Handlers.Items2.CollectionViewHandler2>();
#endif
                })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Font Awesome 7 Brands-Regular-400.otf", "FontAwesomeBrands");
                    fonts.AddFont("Font Awesome 7 Free-Regular-400.otf", "FontAwesomeRegular");
                    fonts.AddFont("Font Awesome 7 Free-Solid-900.otf", "FontAwesomeSolid");
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("SegoeUI-Semibold.ttf", "SegoeSemibold");
                    fonts.AddFont("FluentSystemIcons-Regular.ttf", FluentUI.FontFamily);
                });
            // Register services and pages
            builder.Services.AddTransient<PdfFileView>();
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
            builder.Services.AddTransient<StatementCitationsViewModel>();
            builder.Services.AddTransient<StatementCitationsView>();
            builder.Services.AddSingleton<WebResourcesViewModel>();
            builder.Services.AddSingleton<WebResourcesView>();
            builder.Services.AddSingleton<WritingToolsViewModel>();
            builder.Services.AddSingleton<WritingToolsView>(); 
            builder.Services.AddSingleton<IPdfService, PdfService>();
            return builder.Build();
        }
    }
}
