using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
    public class NewsViewModel : INotifyPropertyChanged
    {
        private IEnumerable<News> _newsList;
        private bool _isBusy;
        private bool _isMainLayoutVisible;
        private bool _isInternetNotConnected;
        private bool _isRefreshing;
        private readonly NewsService _newsService;
        public AsyncCommand<News> ItemTappedCommand { get; }
        public Command LoadMoreCommand { get; }
        public Command RefreshCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerable<News> NewsList
        {
            get { return _newsList; }
            set 
            {
                _newsList = value;
                OnPropertyChanged();
            }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                IsMainLayoutVisible = !_isBusy;
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
                IsBusy = !IsInternetNotConnected;
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

        public NewsViewModel()
        {
            IsBusy = true;
            _newsService = new NewsService();
            ItemTappedCommand = new AsyncCommand<News>(ItemTapped);
            LoadMoreCommand = new Command(LoadMoreData);
            RefreshCommand = new Command(Refresh);
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            IsInternetNotConnected = Connectivity.NetworkAccess != NetworkAccess.Internet;
            LoadData();
            IsBusy = false;
        }

        private async void LoadData()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                IsBusy = true;
                try
                {
                    NewsList = await _newsService.Get(0, 3);
                }
                catch
                {
                    
                }

                IsBusy = false;
            }
        }

        private async void LoadMoreData()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                IsBusy = true;
                try
                {
                    //NewsList.AddRange(await _newsService.Get(NewsList.Count(), 3));
                }
                catch
                {

                }

                IsBusy = false;
            }

        }
        private void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            IsInternetNotConnected = e.NetworkAccess != NetworkAccess.Internet;
        }

        private async Task ItemTapped(News news)
        {
            IsBusy = true;
            await Shell.Current.GoToAsync($"{nameof(SpecificNewsPage)}?NewsId={news.NewsId}");
            IsBusy = false;
        }

        private void Refresh()
        {
            IsRefreshing = true;
            LoadData();
            IsRefreshing = false;
        }
    }
}
