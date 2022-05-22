using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
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
        private bool _isMainLayoutVisible;
        private bool _isInternetNotConnected;
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

        public bool IsMainLayoutVisible
        {
            get { return _isMainLayoutVisible; }
            set
            {
                _isMainLayoutVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsInternetNotConnected
        {
            get { return _isInternetNotConnected; }
            set
            {
                _isInternetNotConnected = value;
                OnPropertyChanged();
                IsMainLayoutVisible = !IsInternetNotConnected;
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
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            IsInternetNotConnected = Connectivity.NetworkAccess != NetworkAccess.Internet;
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

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            IsInternetNotConnected = e.NetworkAccess != NetworkAccess.Internet;
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
