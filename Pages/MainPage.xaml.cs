using AwardQuick.Models;
using AwardQuick.PageModels;

namespace AwardQuick.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
            VersionLabel.Text = model.VersionNumber;    
        }


    }
}