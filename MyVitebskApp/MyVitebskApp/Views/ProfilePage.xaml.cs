using MyVitebskApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyVitebskApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
            Email.Text = App.Current.Properties["id"].ToString();
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//Login");
        }
    }
}