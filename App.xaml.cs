using AwardQuick.CustomServices;
using AwardQuick.Sizes;

namespace AwardQuick
{
    public partial class App : Application
    {
        private static readonly string LOGTAG = "App";
        public App()
        {
            try
            {
                InitializeComponent();
                InitializeAppAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(LOGTAG + " error: " + ex.Message);
            }
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
        private async Task InitializeAppAsync()
        {
            try
            {
                await LoadDeviceSpecificSizes();
            }
            catch (Exception ex)
            {
                Console.WriteLine(LOGTAG + " error: " + ex.Message);
            }
            //MainPage = new AppShell();
        }
        //Check to see if this is a small tablet or any other device and load the correct sizes file mdail 7-7-23
        private async Task LoadDeviceSpecificSizes()
        {
            try
            {
                await Task.Run(() =>
                {
                    //the AppSizses.xaml file fixes the sizes for deifferent devices, however tablet 8 inches or smaller need to use differnt sizes
                    //than the fill sets so we need to checnk for that and load a different sizes file for them mdail 7-7-23
                    var isTablet = DeviceInfo.Idiom == DeviceIdiom.Tablet;
                    if (isTablet)
                    {
                        var screenWidthPixels = DeviceDisplay.MainDisplayInfo.Width;
                        var screenHeightPixels = DeviceDisplay.MainDisplayInfo.Height;
                        var screenDensity = DeviceDisplay.MainDisplayInfo.Density;
                        // Correct the calculation for screen width and height in inches
                        var screenWidthInches = screenWidthPixels / screenDensity / 160;
                        // Assuming screenDensity is in DPI and 160 DPI is the base density.
                        var screenHeightInches = screenHeightPixels / screenDensity / 160;
                        // Calculate the diagonal screen size in inches correctly
                        var screenSizeInches = Math.Sqrt(Math.Pow(screenWidthInches, 2) + Math.Pow(screenHeightInches, 2));
                        if (screenSizeInches <= 8) // Devices with less than 10 inches are considered small tablets
                        {
                            AddResourceDictionary(new SmallTabletSizes());
                        }
                        else
                        {
                            AddResourceDictionary(new AppSizes());
                        }
                    }
                    else
                    {
                        AddResourceDictionary(new AppSizes());
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(LOGTAG + " error: " + ex.Message);
            }
        }
        private void AddResourceDictionary(ResourceDictionary resourceDictionary)
        {
            try
            {
                Resources.MergedDictionaries.Add(resourceDictionary);
            }
            catch (Exception ex)
            {
                Console.WriteLine(LOGTAG + " error: " + ex.Message);
            }
        }
    }
}