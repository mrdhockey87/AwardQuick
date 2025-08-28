using AwardQuick.Models;
using CommunityToolkit.Mvvm.Input;

namespace AwardQuick.PageModels
{
    public interface IProjectTaskPageModel
    {
        IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
        bool IsBusy { get; }
    }
}