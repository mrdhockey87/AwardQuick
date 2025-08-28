using System.Windows.Input;

namespace AwardQuick.CustomControl;

public partial class CustomFlyoutContent : ContentPage
{
    public static readonly BindableProperty ShellReferenceProperty =
        BindableProperty.Create(nameof(ShellReference), typeof(AppShell), typeof(CustomFlyoutContent));

    public AppShell ShellReference
    {
        get => (AppShell)GetValue(ShellReferenceProperty);
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
        if (int.TryParse(parameter, out int selected))
        {
            switch (selected)
            {
                case 0:
                    zero.IsEnabled = false;
                    await ShellReference.OnHomeClicked();
                    zero.IsEnabled = true;
                    break;
                default:
                    break;
            }
        }
    }
}