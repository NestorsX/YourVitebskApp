using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YourVitebskApp.Services;

namespace YourVitebskApp.Views
{
    [QueryProperty(nameof(NewsId), nameof(NewsId))]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SpecificNewsPage : ContentPage
    {
        private readonly NewsService _newsService;
        public string NewsId { get; set; }

        public SpecificNewsPage()
        {
            InitializeComponent();
            _newsService = new NewsService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            int.TryParse(NewsId, out int result);
            try
            {
                BindingContext = await _newsService.Get(result);
            }
            catch (ArgumentException e)
            {
                await Application.Current.MainPage.DisplayAlert("", e.Message, "OK");
            }
        }
    }
}