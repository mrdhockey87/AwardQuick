namespace AwardQuick.Overlays;

public partial class CustomDialogPage : ContentPage
{
    private TaskCompletionSource<DialogResult> _tcs;
    private DialogType _dialogType;

    public enum DialogType
    {
        Alert,
        Confirm,
        Prompt
    }

    public class DialogResult
    {
        public bool? BoolResult { get; set; }
        public string? TextResult { get; set; }
        public bool WasConfirmed => BoolResult == true;
    }

    public CustomDialogPage()
    {
        InitializeComponent();
        _tcs = new TaskCompletionSource<DialogResult>();
    }

    // Helper method to get the current window
    private static Window? GetCurrentWindow()
    {
        var windows = Application.Current?.Windows;
        return windows?.Count > 0 ? windows[0] : null;
    }
    // Modal navigation methods using Window navigation
    public static async Task ShowAlertAsync(string title, string message, string okText = "OK")
    {
        var dialogPage = new CustomDialogPage();
        dialogPage.SetupAlert(title, message, okText);

        // Use Window-based navigation for .NET 9
        var window = GetCurrentWindow();
        if (window?.Page is Shell shell)
        {
            await shell.Navigation.PushModalAsync(dialogPage, true);
        }
        else if (window?.Page != null)
        {
            await window.Page.Navigation.PushModalAsync(dialogPage, true);
        }

        await dialogPage._tcs.Task;
    }

    public static async Task<bool> ShowConfirmAsync(string title, string message,
        string okText = "Yes", string cancelText = "No")
    {
        var dialogPage = new CustomDialogPage();
        dialogPage.SetupConfirm(title, message, okText, cancelText);

        var window = GetCurrentWindow();
        if (window?.Page is Shell shell)
        {
            await shell.Navigation.PushModalAsync(dialogPage, true);
        }
        else if (window?.Page != null)
        {
            await window.Page.Navigation.PushModalAsync(dialogPage, true);
        }

        var result = await dialogPage._tcs.Task;
        return result.WasConfirmed;
    }

    public static async Task<string> ShowPromptAsync(string title, string message,
        string placeholder = "", string okText = "OK", string cancelText = "Cancel")
    {
        var dialogPage = new CustomDialogPage();
        dialogPage.SetupPrompt(title, message, placeholder, okText, cancelText);

        var window = GetCurrentWindow();
        if (window?.Page is Shell shell)
        {
            await shell.Navigation.PushModalAsync(dialogPage, true);
        }
        else if (window?.Page != null)
        {
            await window.Page.Navigation.PushModalAsync(dialogPage, true);
        }

        var result = await dialogPage._tcs.Task;
        return result.TextResult ?? string.Empty;
    }

    // Updated ShowAlertAsync with optional icon
    public static async Task ShowAlertAsync(string title, string message, string okText = "OK", string iconSource = null)
    {
        var dialogPage = new CustomDialogPage();
        dialogPage.SetupAlert(title, message, okText, iconSource);

        var window = GetCurrentWindow();
        if (window?.Page is Shell shell)
        {
            await shell.Navigation.PushModalAsync(dialogPage, true);
        }
        else if (window?.Page != null)
        {
            await window.Page.Navigation.PushModalAsync(dialogPage, true);
        }
        
        await dialogPage._tcs.Task;
    }

    // Updated ShowConfirmAsync with optional icon
    public static async Task<bool> ShowConfirmAsync(string title, string message,
        string okText = "Yes", string cancelText = "No", string iconSource = null)
    {
        var dialogPage = new CustomDialogPage();
        dialogPage.SetupConfirm(title, message, okText, cancelText, iconSource);

        var window = GetCurrentWindow();
        if (window?.Page is Shell shell)
        {
            await shell.Navigation.PushModalAsync(dialogPage, true);
        }
        else if (window?.Page != null)
        {
            await window.Page.Navigation.PushModalAsync(dialogPage, true);
        }

        var result = await dialogPage._tcs.Task;
        return result.WasConfirmed;
    }

    // Updated ShowPromptAsync with optional icon
    public static async Task<string> ShowPromptAsync(string title, string message,
        string placeholder = "", string okText = "OK", string cancelText = "Cancel", string iconSource = null)
    {
        var dialogPage = new CustomDialogPage();
        dialogPage.SetupPrompt(title, message, placeholder, okText, cancelText, iconSource);

        var window = GetCurrentWindow();
        if (window?.Page is Shell shell)
        {
            await shell.Navigation.PushModalAsync(dialogPage, true);
        }
        else if (window?.Page != null)
        {
            await window.Page.Navigation.PushModalAsync(dialogPage, true);
        }

        var result = await dialogPage._tcs.Task;
        return result.TextResult ?? string.Empty;
    }

    // Updated setup methods
    private void SetupAlert(string title, string message, string okText, string iconSource = null)
    {
        _dialogType = DialogType.Alert;
        _tcs = new TaskCompletionSource<DialogResult>();

        SetupIcon(iconSource);
        TitleLabel.Text = title;
        MessageLabel.Text = message;
        OkButton.Text = okText;

        CancelButton.IsVisible = false;
        InputContainer.IsVisible = false;
    }

    private void SetupConfirm(string title, string message, string okText, string cancelText, string iconSource = null)
    {
        _dialogType = DialogType.Confirm;
        _tcs = new TaskCompletionSource<DialogResult>();

        SetupIcon(iconSource);
        TitleLabel.Text = title;
        MessageLabel.Text = message;
        OkButton.Text = okText;
        CancelButton.Text = cancelText;

        CancelButton.IsVisible = true;
        InputContainer.IsVisible = false;
    }

    private void SetupPrompt(string title, string message, string placeholder, string okText, string cancelText, string iconSource = null)
    {
        _dialogType = DialogType.Prompt;
        _tcs = new TaskCompletionSource<DialogResult>();

        SetupIcon(iconSource);
        TitleLabel.Text = title;
        MessageLabel.Text = message;
        InputEntry.Placeholder = placeholder;
        OkButton.Text = okText;
        CancelButton.Text = cancelText;

        CancelButton.IsVisible = true;
        InputContainer.IsVisible = true;

        // Focus the input when shown
        Dispatcher.DispatchDelayed(TimeSpan.FromMilliseconds(100), () => InputEntry.Focus());
    }

    // Helper method to setup the icon
    private void SetupIcon(string iconSource)
    {
        if (!string.IsNullOrEmpty(iconSource))
        {
            IconImage.Source = iconSource;
            IconImage.IsVisible = true;
        }
        else
        {
            IconImage.IsVisible = false;
        }
    }

    private async void OnOkClicked(object sender, EventArgs e)
    {
        var result = new DialogResult();

        switch (_dialogType)
        {
            case DialogType.Alert:
                result.BoolResult = true;
                break;
            case DialogType.Confirm:
                result.BoolResult = true;
                break;
            case DialogType.Prompt:
                result.BoolResult = true;
                result.TextResult = InputEntry.Text;
                break;
        }

        _tcs?.SetResult(result);

        // Use Window-based navigation for closing
        var window = GetCurrentWindow();
        if (window?.Page is Shell shell)
        {
            await shell.Navigation.PopModalAsync(true);
        }
        else if (window?.Page != null)
        {
            await window.Page.Navigation.PopModalAsync(true);
        }
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        var result = new DialogResult { BoolResult = false };
        _tcs?.SetResult(result);

        var window = GetCurrentWindow();
        if (window?.Page is Shell shell)
        {
            await shell.Navigation.PopModalAsync(true);
        }
        else if (window?.Page != null)
        {
            await window.Page.Navigation.PopModalAsync(true);
        }
    }

    protected override bool OnBackButtonPressed()
    {
        var result = new DialogResult { BoolResult = false };
        _tcs?.SetResult(result);
        return base.OnBackButtonPressed();
    }
}