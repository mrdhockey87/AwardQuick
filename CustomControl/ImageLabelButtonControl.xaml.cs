using System.Windows.Input;

namespace AwardQuick.CustomControl;

public partial class ImageLabelButtonControl : ContentView
{
    public static readonly BindableProperty ImageSourceProperty =
        BindableProperty.Create(nameof(ImageSource), typeof(ImageSource), typeof(ImageLabelButtonControl));

    public static readonly BindableProperty ImageAspectProperty =
        BindableProperty.Create(nameof(ImageAspect), typeof(Aspect), typeof(ImageLabelButtonControl), Aspect.AspectFit);

    public static readonly BindableProperty ImageWidthProperty =
        BindableProperty.Create(nameof(ImageWidth), typeof(double), typeof(ImageLabelButtonControl), 50.0);

    public static readonly BindableProperty ImageHeightProperty =
        BindableProperty.Create(nameof(ImageHeight), typeof(double), typeof(ImageLabelButtonControl), 50.0);

    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(ImageLabelButtonControl));

    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(ImageLabelButtonControl), Colors.Black);

    public static readonly BindableProperty TextSizeProperty =
        BindableProperty.Create(nameof(TextSize), typeof(double), typeof(ImageLabelButtonControl), 14.0);

    public static readonly BindableProperty LabelMarginProperty =
        BindableProperty.Create(nameof(LabelMargin), typeof(Thickness), typeof(ImageLabelButtonControl), new Thickness(10, 0, 0, 0));

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ImageLabelButtonControl));

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ImageLabelButtonControl));

    public static readonly BindableProperty NormalBackgroundColorProperty =
        BindableProperty.Create(nameof(NormalBackgroundColor), typeof(Color), typeof(ImageLabelButtonControl), Colors.Transparent,
            propertyChanged: OnVisualStatePropertyChanged);

    public static readonly BindableProperty NormalTextColorProperty =
        BindableProperty.Create(nameof(NormalTextColor), typeof(Color), typeof(ImageLabelButtonControl), Colors.Black,
            propertyChanged: OnVisualStatePropertyChanged);

    public static readonly BindableProperty HoverBackgroundColorProperty =
        BindableProperty.Create(nameof(HoverBackgroundColor), typeof(Color), typeof(ImageLabelButtonControl), Colors.LightGray);

    public static readonly BindableProperty HoverTextColorProperty =
        BindableProperty.Create(nameof(HoverTextColor), typeof(Color), typeof(ImageLabelButtonControl), Colors.Black);

    public static readonly BindableProperty PressedBackgroundColorProperty =
        BindableProperty.Create(nameof(PressedBackgroundColor), typeof(Color), typeof(ImageLabelButtonControl), Colors.Gray);

    public static readonly BindableProperty PressedTextColorProperty =
        BindableProperty.Create(nameof(PressedTextColor), typeof(Color), typeof(ImageLabelButtonControl), Colors.White);

    public static readonly BindableProperty DisabledBackgroundColorProperty =
        BindableProperty.Create(nameof(DisabledBackgroundColor), typeof(Color), typeof(ImageLabelButtonControl), Colors.LightGray);

    public static readonly BindableProperty DisabledTextColorProperty =
        BindableProperty.Create(nameof(DisabledTextColor), typeof(Color), typeof(ImageLabelButtonControl), Colors.Gray);

    public ImageSource ImageSource
    {
        get => (ImageSource)GetValue(ImageSourceProperty);
        set => SetValue(ImageSourceProperty, value);
    }

    public Aspect ImageAspect
    {
        get => (Aspect)GetValue(ImageAspectProperty);
        set => SetValue(ImageAspectProperty, value);
    }

    public double ImageWidth
    {
        get => (double)GetValue(ImageWidthProperty);
        set => SetValue(ImageWidthProperty, value);
    }

    public double ImageHeight
    {
        get => (double)GetValue(ImageHeightProperty);
        set => SetValue(ImageHeightProperty, value);
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }

    public double TextSize
    {
        get => (double)GetValue(TextSizeProperty);
        set => SetValue(TextSizeProperty, value);
    }

    public Thickness LabelMargin
    {
        get => (Thickness)GetValue(LabelMarginProperty);
        set => SetValue(LabelMarginProperty, value);
    }

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public Color NormalBackgroundColor
    {
        get => (Color)GetValue(NormalBackgroundColorProperty);
        set => SetValue(NormalBackgroundColorProperty, value);
    }

    public Color NormalTextColor
    {
        get => (Color)GetValue(NormalTextColorProperty);
        set => SetValue(NormalTextColorProperty, value);
    }

    public Color HoverBackgroundColor
    {
        get => (Color)GetValue(HoverBackgroundColorProperty);
        set => SetValue(HoverBackgroundColorProperty, value);
    }

    public Color HoverTextColor
    {
        get => (Color)GetValue(HoverTextColorProperty);
        set => SetValue(HoverTextColorProperty, value);
    }

    public Color PressedBackgroundColor
    {
        get => (Color)GetValue(PressedBackgroundColorProperty);
        set => SetValue(PressedBackgroundColorProperty, value);
    }

    public Color PressedTextColor
    {
        get => (Color)GetValue(PressedTextColorProperty);
        set => SetValue(PressedTextColorProperty, value);
    }

    public Color DisabledBackgroundColor
    {
        get => (Color)GetValue(DisabledBackgroundColorProperty);
        set => SetValue(DisabledBackgroundColorProperty, value);
    }

    public Color DisabledTextColor
    {
        get => (Color)GetValue(DisabledTextColorProperty);
        set => SetValue(DisabledTextColorProperty, value);
    }

    public ImageLabelButtonControl()
    {
        InitializeComponent();

        // Ensure visual reflects enabled state initially
        ApplyVisualState(IsEnabled ? "Normal" : "Disabled");
    }

    private static void OnVisualStatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ImageLabelButtonControl control)
        {
            control.ApplyVisualState(control.IsEnabled ? "Normal" : "Disabled");
        }
    }

    protected override void OnPropertyChanged(string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName == IsEnabledProperty.PropertyName)
        {
            ApplyVisualState(IsEnabled ? "Normal" : "Disabled");
        }
    }

    // Event handlers wired from XAML
    private void OnOverlayPressed(object? sender, EventArgs e)
    {
        ApplyVisualStateInternal("Pressed");
    }

    private void OnOverlayReleased(object? sender, EventArgs e)
    {
        // Return to Normal when released. If you want Hover behavior, add pointer events.
        ApplyVisualStateInternal(IsEnabled ? "Normal" : "Disabled");
    }

    private void OnOverlayClicked(object? sender, EventArgs e)
    {
        // Ensure click also results in normal visual after action
        ApplyVisualStateInternal(IsEnabled ? "Normal" : "Disabled");
    }

    // Keep original public API
    public void SetVisualState(string state)
    {
        ApplyVisualState(state);
    }

    // ApplyVisualState updates the visual properties of the inner elements directly
    private void ApplyVisualState(string state)
    {
        ApplyVisualStateInternal(state);
    }

    private void ApplyVisualStateInternal(string state)
    {
        // Decide background/text values based on state
        Color bg;
        Color txt;

        switch (state)
        {
            case "Pressed":
                bg = PressedBackgroundColor;
                txt = PressedTextColor;
                break;
            case "Hover":
                bg = HoverBackgroundColor;
                txt = HoverTextColor;
                break;
            case "Disabled":
                bg = DisabledBackgroundColor;
                txt = DisabledTextColor;
                break;
            case "Normal":
            default:
                bg = NormalBackgroundColor;
                txt = NormalTextColor;
                break;
        }

        // Update the control BackgroundColor (keeps previous behavior)
        BackgroundColor = bg;

        // Update the named borders and label directly so the image + label appear as one element
        try
        {
            image.BackgroundColor = bg;
            label.BackgroundColor = bg;
            label.TextColor = txt;
        }
        catch
        {
            // In case names not resolved yet, swallow errors silently
        }

        // Also update the bindable TextColor property so bindings remain consistent
        SetValue(TextColorProperty, txt);
    }
}