using AwardQuick.Overlays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwardQuick.CustomServices
{
    public interface IDialogService
    {
        Task ShowAlertAsync(string title, string message, string okText = "OK", string? iconSource = null);
        Task<bool> ShowConfirmAsync(string title, string message, string okText = "Yes", string cancelText = "No", string? iconSource = null);
        Task<string> ShowPromptAsync(string title, string message, string placeholder = "", string okText = "OK", string cancelText = "Cancel", string? iconSource = null);
    }

    public class ShellDialogService : IDialogService
    {
        public async Task ShowAlertAsync(string title, string message, string okText = "OK", string? iconSource = null)
        {
            await CustomDialogPage.ShowAlertAsync(title, message, okText, iconSource ?? string.Empty);
        }

        public async Task<bool> ShowConfirmAsync(string title, string message, string okText = "Yes", string cancelText = "No", string? iconSource = null)
        {
            return await CustomDialogPage.ShowConfirmAsync(title, message, okText, cancelText, iconSource ?? string.Empty);
        }

        public async Task<string> ShowPromptAsync(string title, string message, string placeholder = "",
            string okText = "OK", string cancelText = "Cancel", string? iconSource = null)
        {
            return await CustomDialogPage.ShowPromptAsync(title, message, placeholder, okText, cancelText, iconSource ?? string.Empty);
        }
    }
}
