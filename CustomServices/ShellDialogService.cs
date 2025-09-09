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
        Task ShowAlertAsync(string title, string message, string okText = "OK");
        Task<bool> ShowConfirmAsync(string title, string message, string okText = "Yes", string cancelText = "No");
        Task<string> ShowPromptAsync(string title, string message, string placeholder = "", string okText = "OK", string cancelText = "Cancel");
    }

    public class ShellDialogService : IDialogService
    {
        public async Task ShowAlertAsync(string title, string message, string okText = "OK")
        {
            await CustomDialogPage.ShowAlertAsync(title, message, okText);
        }

        public async Task<bool> ShowConfirmAsync(string title, string message, string okText = "Yes", string cancelText = "No")
        {
            return await CustomDialogPage.ShowConfirmAsync(title, message, okText, cancelText);
        }

        public async Task<string> ShowPromptAsync(string title, string message, string placeholder = "",
            string okText = "OK", string cancelText = "Cancel")
        {
            return await CustomDialogPage.ShowPromptAsync(title, message, placeholder, okText, cancelText);
        }
    }
}
