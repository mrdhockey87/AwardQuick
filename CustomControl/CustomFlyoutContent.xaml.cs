using Microsoft.Maui.Controls;
using System.Windows.Input;

namespace AwardQuick.CustomControl;

public partial class CustomFlyoutContent : ContentView
{
    public static readonly BindableProperty ShellReferenceProperty =
        BindableProperty.Create(nameof(ShellReference), typeof(AppShell), typeof(CustomFlyoutContent),
             propertyChanged: OnShellReferenceChanged);

    private static void OnShellReferenceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is CustomFlyoutContent customFlyout)
        {
            System.Diagnostics.Debug.WriteLine($"ShellReference changed: {oldValue} -> {newValue}");
            if (newValue is AppShell shell)
            {
                System.Diagnostics.Debug.WriteLine($"ShellReference successfully set to: {shell.GetType().Name}");
            }
        }
    }

    public AppShell? ShellReference
    {
        get => (AppShell?)GetValue(ShellReferenceProperty);
        set => SetValue(ShellReferenceProperty, value);
    }
    public ICommand TapGestureRecognizerCommand { get; private set; }
    public CustomFlyoutContent()
	{
		InitializeComponent();
        TapGestureRecognizerCommand = new Command<string>(OnTapped);
        BindingContext = this;
    }
    private async void OnTapped(string parameter)
    {
        if (string.IsNullOrEmpty(parameter))
        {
            System.Diagnostics.Debug.WriteLine("Parameter is null or empty");
            return;
        }

        if (ShellReference == null)
        {
            System.Diagnostics.Debug.WriteLine("ShellReference is null");
            return;
        }

        switch (parameter.ToLower())
        {
            case "home":
                home.IsEnabled = false;
                await ShellReference.OnHomeClicked();
                home.IsEnabled = true;
                break;
            case "statement":
                statement.IsEnabled = false;
                await ShellReference.OnMenuItemClicked("StatementCitations");
                statement.IsEnabled = true;
                break;
            case "forms":
                forms.IsEnabled = false;
                await ShellReference.OnMenuItemClicked("Forms");
                forms.IsEnabled = true;
                break;
            case "examples":
                examples.IsEnabled = false;
                await ShellReference.OnMenuItemClicked("Examples");
                examples.IsEnabled = true;
                break;
            case "letters":
                letters.IsEnabled = false;
                await ShellReference.OnMenuItemClicked("LettersMemos");
                letters.IsEnabled = true;
                break;
            case "webresources":
                webresources.IsEnabled = false;
                await ShellReference.OnMenuItemClicked("WebResources");
                webresources.IsEnabled = true;
                break;
            case "references":
                references.IsEnabled = false;
                await ShellReference.OnMenuItemClicked("References");
                references.IsEnabled = true;
                break;
            case "writing":
                writing.IsEnabled = false;
                await ShellReference.OnMenuItemClicked("WritingTools");
                writing.IsEnabled = true;
                break;
            case "updates":
                updates.IsEnabled = false;
                await ShellReference.OnUpdateClicked("updates");
                updates.IsEnabled = true;
                break;
            case "support":
                support.IsEnabled = false;
                await ShellReference.OnSupportClicked("support");
                support.IsEnabled = true;
                break;
            case "about":
                about.IsEnabled = false;
                await ShellReference.OnAboutClicked("about");
                about.IsEnabled = true;
                break;
            case "licenses":
                licenses.IsEnabled = false;
                await ShellReference.OnLicensesClicked("licenses");
                licenses.IsEnabled = true;
                break;
            case "mentormilitary":
                mentormilitary.IsEnabled = false;
                await ShellReference.OnVisitMentorMilClicked("mentormilitary");
                mentormilitary.IsEnabled = true;
                break;
            default:
                home.IsEnabled = false;
                await ShellReference.OnHomeClicked();
                home.IsEnabled = true;
                break;
        }
    }
}