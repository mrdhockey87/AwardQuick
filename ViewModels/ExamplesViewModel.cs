using AwardQuick.Utilities;
using AwardQuick.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AwardQuick.ViewModels
{
    public partial class ExamplesViewModel : ObservableObject
    {
        private readonly IPdfService _pdfService;
        public IAsyncRelayCommand DAForm638Command { get; }
        public IAsyncRelayCommand DAForm7594Command { get; }
        public IAsyncRelayCommand LegonOfMeritCommand { get; }
        public IAsyncRelayCommand MedalOfHonorCommand { get; }
        public ICommand OperationOfficerCommand { get; }
        public ICommand FirstSergeantCommand { get; }
        public ICommand ExecutiveOfficerCommand { get; }
        public ICommand PLTSergeantCommand { get; }
        public ICommand CCVersion1Command { get; }
        public ICommand S1NCOICCommand { get; }
        public ICommand CCVersion2Command { get; }
        public ICommand S4NCOICCommand { get; }
        public ICommand CCVersion3Command { get; }
        public ICommand S3OpsSGMCommand { get; }
        public ICommand CCVersion4Command { get; }
        public ICommand CSMCommand { get; }
        private static ExamplesView ExView;
        public void SetView(ExamplesView exview) => ExView = exview;        
        public ExamplesViewModel() : this(ServiceHelper.GetService<IPdfService>()) { }
        public ExamplesViewModel(IPdfService pdfService)
        {
            DAForm638Command = new AsyncRelayCommand(DAForm638CommandExecute);
            DAForm7594Command = new AsyncRelayCommand(DAForm7594CommandExecute);
            LegonOfMeritCommand = new AsyncRelayCommand(LegonOfMeritCommandExecute);
            MedalOfHonorCommand = new AsyncRelayCommand(MedalOfHonorCommandExecute);
            OperationOfficerCommand = new AsyncRelayCommand(OperationOfficerCommandExecute);
            FirstSergeantCommand = new AsyncRelayCommand(FirstSergeantCommandExecute);
            ExecutiveOfficerCommand = new AsyncRelayCommand(ExecutiveOfficerCommandExecute);
            PLTSergeantCommand = new AsyncRelayCommand(PLTSergeantCommandExecute);
            CCVersion1Command = new AsyncRelayCommand(CCVersion1CommandExecute);
            S1NCOICCommand = new AsyncRelayCommand(S1NCOICCommandExecute);
            CCVersion2Command = new AsyncRelayCommand(CCVersion2CommandExecute);
            S4NCOICCommand = new AsyncRelayCommand(S4NCOICCommandExecute);
            CCVersion3Command = new AsyncRelayCommand(CCVersion3CommandExecute);
            S3OpsSGMCommand = new AsyncRelayCommand(S3OpsSGMCommandExecute);
            CCVersion4Command = new AsyncRelayCommand(CCVersion4CommandExecute);
            CSMCommand = new AsyncRelayCommand(CSMCommandExecute);
            _pdfService = pdfService;
        }


        private Task DAForm638CommandExecute() => OpenInPdfViewAsync("ExampleDAFORM638.pdf.gz");
        private Task DAForm7594CommandExecute() => OpenInPdfViewAsync("ExampleDAFORM7594.pdf.gz");
        private Task LegonOfMeritCommandExecute() => OpenInPdfViewAsync("LOM.pdf.gz");
        private Task MedalOfHonorCommandExecute() => OpenInPdfViewAsync("MedalOfHonorCitation.pdf.gz");
        private Task OperationOfficerCommandExecute() => OpenInPdfViewAsync("BSMNarrativeandCitationSPO.pdf.gz");
        private Task FirstSergeantCommandExecute() => OpenInPdfViewAsync("BSMNarrative1SG.pdf.gz");
        private Task ExecutiveOfficerCommandExecute() => OpenInPdfViewAsync("BSMNarrativeandCitationXO.pdf.gz");
        private Task PLTSergeantCommandExecute() => OpenInPdfViewAsync("BSMNarrativePLTSergeant.pdf.gz");
        private Task CCVersion1CommandExecute() => OpenInPdfViewAsync("BSMNarrativeandCitationCdr.pdf.gz");
        private Task S1NCOICCommandExecute() => OpenInPdfViewAsync("BSMNarrativeS1NCOIC.pdf.gz");
        private Task CCVersion2CommandExecute() => OpenInPdfViewAsync("BSMNarrativeCdr2.pdf.gz");
        private Task S4NCOICCommandExecute() => OpenInPdfViewAsync("BSMNarrativeS4NCOIC.pdf.gz");
        private Task CCVersion3CommandExecute() => OpenInPdfViewAsync("BSMNarrativeCdr3.pdf.gz");
        private Task S3OpsSGMCommandExecute() => OpenInPdfViewAsync("BSMNarrativeS3OpsSGM.pdf.gz");
        private Task CCVersion4CommandExecute() => OpenInPdfViewAsync("BSMNarrativeCdr4.pdf.gz");
        private Task CSMCommandExecute() => OpenInPdfViewAsync("BSMNarrativeandCitationCSM.pdf.gz");

        // Replace the helper to navigate using the parameter dictionary (no manual encoding)
        private async Task OpenInPdfViewAsync(string gzipFileName)
        {
            try
            {
                var packagedPath = gzipFileName.Contains('/') ? gzipFileName : $"Examples\\{gzipFileName}";

                // Materialize to a local .pdf and capture the path
                //var localPdfPath = await _pdfService.MaterializePdfFromAssetsAsync(packagedPath);
                if (string.IsNullOrWhiteSpace(packagedPath) || !File.Exists(packagedPath))
                {
                    if (ExView != null)
                        await ExView.DisplayAlert("Unable to resolve PDF path.", "Path is null or file missing.", "OK");
                    return;
                }

                Constants.PdfFileName = packagedPath;
                await MainThread.InvokeOnMainThreadAsync(async () => await Shell.Current.GoToAsync("PdfViewer"));
                //await Shell.Current.GoToAsync("PdfViewer");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Navigation failed: {ex}");
                if (ExView != null)
                    await ExView.DisplayAlert("Navigation failed", ex.Message, "OK");
            }
        }
    }
}