using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AwardQuick.ViewModels;

public class PdfViewModel : ObservableObject
{

   // public IAsyncRelayCommand OnEmailFormCommand { get; }
   // public IAsyncRelayCommand OnPrintCommand { get; }
    public PdfViewModel()
    {
       // OnEmailFormCommand = new AsyncRelayCommand(OnEmailFormCommandExecute);
       // OnPrintCommand = new AsyncRelayCommand(OnPrintCommandExecute);
    }
    /* private async Task OnPrintCommandExecute()
     {

         var mainWindow = Microsoft.Maui.Controls.Application.Current?.Windows?.FirstOrDefault();
         var nav = mainWindow?.Page?.Navigation;
         if (nav is null)            return;

         //await nav.PushAsync(new PdfViewFilled(PdfPageData));
         // Implement email functionality here
         await Task.CompletedTask;
     }



     private async Task OnEmailFormCommandExecute()
     {
         // Implement email functionality here
        // await Task.CompletedTask;
     }
 */
}