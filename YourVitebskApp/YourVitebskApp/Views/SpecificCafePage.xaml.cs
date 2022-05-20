using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YourVitebskApp.Services;

namespace YourVitebskApp.Views
{
    [QueryProperty(nameof(CafeId), nameof(CafeId))]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SpecificCafePage : ContentPage
    {
        private readonly CafeService _cafeService;
        public string CafeId { get; set; }

        public SpecificCafePage()
        {
            InitializeComponent();
            _cafeService = new CafeService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            int.TryParse(CafeId, out int result);
            try
            {
                BindingContext = await _cafeService.Get(result);
            }
            catch (ArgumentException e)
            {
                await Application.Current.MainPage.DisplayAlert("", e.Message, "OK");
            }
        }
    }
}