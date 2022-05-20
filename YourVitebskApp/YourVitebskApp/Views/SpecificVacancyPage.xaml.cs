using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YourVitebskApp.Services;

namespace YourVitebskApp.Views
{
    [QueryProperty(nameof(VacancyId), nameof(VacancyId))]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SpecificVacancyPage : ContentPage
    {
        private readonly VacancyService _vacancyService;
        public string VacancyId { get; set; }

        public SpecificVacancyPage()
        {
            InitializeComponent();
            _vacancyService = new VacancyService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            int.TryParse(VacancyId, out int result);
            try
            {
                BindingContext = await _vacancyService.Get(result);
            }
            catch (ArgumentException e)
            {
                await Application.Current.MainPage.DisplayAlert("", e.Message, "OK");
            }
        }
    }
}