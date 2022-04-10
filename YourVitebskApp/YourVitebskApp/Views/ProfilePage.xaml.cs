using YourVitebskApp.Models;
using YourVitebskApp.Services;
using YourVitebskApp.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace YourVitebskApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
            Routing.RegisterRoute("//SettingsPage", typeof(SettingsPage));
            Email.Text = App.Current.Properties["id"].ToString();
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//Login");
        }

        private async void Settings_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(SettingsPage)}");
        }
    }
}