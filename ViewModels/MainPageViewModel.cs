using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AwardQuick.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {

        private string VersionNumberLocal
        {
            get { return $"Version {AppVersion.AppVersionNo}"; }
            set { }
        }
        public string VersionNumber
        {
            get { return VersionNumberLocal; }
            set
            {
                VersionNumberLocal = value;
                OnPropertyChanged(nameof(VersionNumber));
            }
        }
        public MainPageViewModel()
        {
        }

    }
}