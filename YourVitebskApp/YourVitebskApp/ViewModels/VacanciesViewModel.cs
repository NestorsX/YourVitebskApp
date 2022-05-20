using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using YourVitebskApp.Models;
using YourVitebskApp.Services;
using YourVitebskApp.Views;

namespace YourVitebskApp.ViewModels
{
    public class VacanciesViewModel : INotifyPropertyChanged
    {
        private IEnumerable<Vacancy> _vacanciesList;
        private bool _isBusy;
        private bool _isRefreshing;
        private readonly VacancyService _vacancyService;
        public AsyncCommand<Vacancy> ItemTappedCommand { get; }
        public Command RefreshCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerable<Vacancy> VacanciesList
        {
            get { return _vacanciesList; }
            set
            {
                _vacanciesList = value;
                OnPropertyChanged();
            }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }

        public VacanciesViewModel()
        {
            IsBusy = true;
            _vacancyService = new VacancyService();
            ItemTappedCommand = new AsyncCommand<Vacancy>(ItemTapped);
            RefreshCommand = new Command(Refresh);
            AddData();
            IsBusy = false;
        }

        private async void AddData()
        {
            VacanciesList = await _vacancyService.Get();
        }

        private void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private async Task ItemTapped(Vacancy cafe)
        {
            IsBusy = true;
            await Shell.Current.GoToAsync($"{nameof(VacanciesPage)}/{nameof(SpecificVacancyPage)}?VacancyId={cafe.VacancyId}");
            IsBusy = false;
        }

        private void Refresh()
        {
            IsRefreshing = true;
            AddData();
            IsRefreshing = false;
        }
    }
}
