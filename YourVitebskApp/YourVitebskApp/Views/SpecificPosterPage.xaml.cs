using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YourVitebskApp.Services;

namespace YourVitebskApp.Views
{
    [QueryProperty(nameof(PosterId), nameof(PosterId))]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SpecificPosterPage : ContentPage
    {
        private readonly PosterService _posterService;
        public string PosterId { get; set; }

        public SpecificPosterPage()
        {
            InitializeComponent();
            _posterService = new PosterService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            int.TryParse(PosterId, out int result);
            try
            {
                BindingContext = await _posterService.Get(result);
            }
            catch (ArgumentException e)
            {
                await Application.Current.MainPage.DisplayAlert("", e.Message, "OK");
            }
        }
    }
}