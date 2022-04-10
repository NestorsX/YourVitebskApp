using YourVitebskApp.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YourVitebskApp.Models;

namespace YourVitebskApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            Routing.RegisterRoute("//RegisterPage",typeof(RegisterPage));
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            LoadingWheel.IsRunning = true;
            var userService = new UserService();
            if (Email.Text != null && Password.Text != null)
            {
                User currentUser = await userService.Get(Email.Text, Password.Text);
                if (currentUser != null)
                {
                    await DisplayAlert("Alert", $"{currentUser.UserId} | {currentUser.Email}", "OK");
                    Application.Current.Properties["id"] = currentUser.UserId;
                    await Shell.Current.GoToAsync("//Main");
                }
                else
                {
                    await DisplayAlert("Alert", $"Неверный логин или пароль", "OK");
                }
            }

            LoadingWheel.IsRunning = false;
        }

        private async void RegisterButton_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(RegisterPage)}");
        }
    }
}