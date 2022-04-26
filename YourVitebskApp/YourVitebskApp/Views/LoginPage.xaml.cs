using YourVitebskApp.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YourVitebskApp.Models;
using YourVitebskApp.ViewModels;

namespace YourVitebskApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            Routing.RegisterRoute("//RegisterPage", typeof(RegisterPage));
            BindingContext = new LoginViewModel();
        }
    }
}