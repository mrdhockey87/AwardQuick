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
        public IAsyncRelayCommand DAForm638Command { get; }
        public IAsyncRelayCommand DAForm7594Command { get; }
        public IAsyncRelayCommand LegonOfMeritCommand { get; }
        public IAsyncRelayCommand MedalOfHonorCommand { get; }
        public ExamplesViewModel()
        {
            DAForm638Command = new AsyncRelayCommand(DAForm638CommandExecute);
            DAForm7594Command = new AsyncRelayCommand(DAForm7594CommandExecute);
            LegonOfMeritCommand = new AsyncRelayCommand(LegonOfMeritCommandExecute);
            MedalOfHonorCommand = new AsyncRelayCommand(MedalOfHonorCommandExecute);
        }

        private async Task DAForm638CommandExecute()
        {
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                // Implementation for DAForm638Command
            });
        }
        private async Task DAForm7594CommandExecute()
        {
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                // Implementation for DAForm7594Command
            });
        }
        private async Task LegonOfMeritCommandExecute()
        {
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                // Implementation for LegonOfMeritCommand
            });
        }
        private async Task MedalOfHonorCommandExecute()
        {
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                // Implementation for MedalOfHonorCommand
            });
        }
    }
}
