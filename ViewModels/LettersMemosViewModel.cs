using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AwardQuick.Overlays;
using AwardQuick.Utilities;
using AwardQuick.Views;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AwardQuick.ViewModels;

public partial class LettersMemosViewModel : ObservableObject
{
    private static LettersMemosView? LandMView;

    public void SetView(LettersMemosView landmview) => LandMView = landmview;
    private readonly AssetMaterializerService _materializer = new();


    public IAsyncRelayCommand OpenLoLCommand { get; }
    public IAsyncRelayCommand OpenAoOCommand { get; }
    public IAsyncRelayCommand OpenRoOCommand { get; }
    public IAsyncRelayCommand OpenMemoCommand { get; }
    public IAsyncRelayCommand ViewReasonsLoL { get; }

    public LettersMemosViewModel()
    {
        OpenLoLCommand = new AsyncRelayCommand(OpenLoLCommandExecute);
        OpenAoOCommand = new AsyncRelayCommand(OpenAoOCommandExecute);
        OpenRoOCommand = new AsyncRelayCommand(OpenRoOCommandExecute);
        OpenMemoCommand = new AsyncRelayCommand(OpenMemoCommandExecute);
        ViewReasonsLoL = new AsyncRelayCommand(ViewReasonsLoLExecute);
    }
    private Task OpenLoLCommandExecute() => OpenInDocAsync("LetterofLateness.doc.gz");
    private Task OpenAoOCommandExecute() => OpenInDocAsync("AmendmentofOrders.doc.gz");
    private Task OpenRoOCommandExecute() => OpenInDocAsync("RevocationofOrders.doc.gz");
    private Task OpenMemoCommandExecute() => OpenInDocAsync("Memorandum.doc.gz");
    private Task ViewReasonsLoLExecute() => OpenReasonsPage();

    private async Task OpenReasonsPage()
    {
        if (LandMView != null)
        {
            await LandMView.ProgressOverlay.ShowAsync();
        }
        try
        {
            await Shell.Current.GoToAsync("Reasons");
            if (LandMView != null)
            {
                await LandMView.ProgressOverlay.Hide();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Navigation failed: {ex}");
            if (LandMView != null)
            {
                await LandMView.ProgressOverlay.Hide();
                await LandMView.DisplayAlertAsync("Navigation failed", ex.Message, "OK");
            }
        }
    }

    private async Task OpenInDocAsync(string gzipFileName)
    {
        try
        {
            if (LandMView != null)
            {
                await LandMView.ProgressOverlay.ShowAsync();
            }
            var packagedPath = gzipFileName.Contains('/') ? gzipFileName : $"Letters\\{gzipFileName}";

            // Try to materialize and open
            string destPath;
            try
            {
                destPath = await _materializer.MaterializeAssetAsync(packagedPath);
            }
            catch (Exception)
            {
                if (LandMView != null)
                {
                    await LandMView.ProgressOverlay.Hide();
                    await LandMView.DisplayAlertAsync("Unable to resolve Doc path.", "Path is null or file missing.", "OK");
                }
                return;
            }

            // Set constants to the file name only (as requested)
            Constants.PdfFileName = Path.GetFileName(destPath);

            // Open with the system default viewer
            await _materializer.OpenWithDefaultAppAsync(destPath);
            if (LandMView != null)
            {
                await LandMView.ProgressOverlay.Hide();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Navigation failed: {ex}");
            if (LandMView != null)
            {
                await LandMView.ProgressOverlay.Hide();
                await LandMView.DisplayAlertAsync("Navigation failed", ex.Message, "OK");
            }
        }
    }
}