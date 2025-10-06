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

        private async Task DAForm638CommandExecute()
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await _pdfService.OpenPdfFromAssetsAsync("ExampleDAFORM638.pdf.gz");
            });
        }
        private async Task DAForm7594CommandExecute()
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await _pdfService.OpenPdfFromAssetsAsync("ExampleDAFORM7594.pdf.gz");
            });
        }
        private async Task LegonOfMeritCommandExecute()
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await _pdfService.OpenPdfFromAssetsAsync("LOM.pdf.gz");
            });
        }
        private async Task MedalOfHonorCommandExecute()
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await _pdfService.OpenPdfFromAssetsAsync("MedalOfHonorCitation.pdf.gz");
            });
        }
        private async Task OperationOfficerCommandExecute()
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await _pdfService.OpenPdfFromAssetsAsync("BSMNarrativeandCitationSPO.pdf.pdf.gz");
            });
        }
        private async Task FirstSergeantCommandExecute()
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await _pdfService.OpenPdfFromAssetsAsync("BSMNarrative1SG.pdf.gz");
            });
        }
        private async Task ExecutiveOfficerCommandExecute()
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await _pdfService.OpenPdfFromAssetsAsync("BSMNarrativeandCitationXO.pdf.gz");
            });
        }
        private async Task PLTSergeantCommandExecute()
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await _pdfService.OpenPdfFromAssetsAsync("BSMNarrativePLTSergeant.pdf.gz");
            });
        }
        private async Task CCVersion1CommandExecute()
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await _pdfService.OpenPdfFromAssetsAsync("BSMNarrativeandCitationCdr.pdf.gz");
            });
        }
        private async Task S1NCOICCommandExecute()
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await _pdfService.OpenPdfFromAssetsAsync("BSMNarrativeS1NCOIC.pdf.gz");
            });
        }
        private async Task CCVersion2CommandExecute()
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await _pdfService.OpenPdfFromAssetsAsync("BSMNarrativeCdr2.pdf.pdf.gz");
            });
        }
        private async Task S4NCOICCommandExecute()
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await _pdfService.OpenPdfFromAssetsAsync("BSMNarrativeS4NCOIC.pdf.gz");
            });
        }
        private async Task CCVersion3CommandExecute()
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await _pdfService.OpenPdfFromAssetsAsync("BSMNarrativeCdr3.pdfgz");
            });
        }
        private async Task S3OpsSGMCommandExecute()
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await _pdfService.OpenPdfFromAssetsAsync("BSMNarrativeS3OpsSGM.pdf.gz");
            });
        }
        private async Task CCVersion4CommandExecute()
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await _pdfService.OpenPdfFromAssetsAsync("BSMNarrativeCdr4.pdf.gz");
            });
        }
        private async Task CSMCommandExecute()
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await _pdfService.OpenPdfFromAssetsAsync("BSMNarrativeandCitationCSM.pdf.gz");
            });
        }
    }
}