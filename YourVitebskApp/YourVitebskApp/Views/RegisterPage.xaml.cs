using YourVitebskApp.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YourVitebskApp.Models;

namespace YourVitebskApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private async void RegisterButton_Clicked(object sender, EventArgs e)
        {
            var userService = new UserService();

            if (Email.Text != null && Password.Text != null)
            {
                User user = new User()
                {
                    UserId = null,
                    Email = Email.Text,
                    Password = Password.Text
                };

                User currentUser = await userService.Add(user);
                Application.Current.Properties["id"] = currentUser.UserId;
                await Shell.Current.GoToAsync("//Main");
            }
            else
            {
                await DisplayAlert("Alert", $"Произошла ошибка", "OK");
            }
        }
    }
}