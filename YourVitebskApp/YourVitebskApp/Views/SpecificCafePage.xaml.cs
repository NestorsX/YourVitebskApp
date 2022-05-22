using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YourVitebskApp.Services;
using YourVitebskApp.ViewModels;

namespace YourVitebskApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SpecificCafePage : ContentPage
    {
        public SpecificCafePage()
        {
            InitializeComponent();
            BindingContext = new SpecificCafeViewModel();
        }
    }
}