using AwardQuick.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AwardQuick.PageModels
{
    public partial class MainPageModel : ObservableObject, INotifyPropertyChanged
    {

        private string VersionNumberLocal => $"Version {AppVersion.AppVersionNo} (Build {AppVersion.AppBuildNo})";
        public string VersionNumber
        {
            get { return VersionNumberLocal; }
            set
            {
                value = VersionNumberLocal;
                OnPropertyChanged(nameof(VersionNumber));
            }
        }
        public MainPageModel()
        {
        }

    }
}