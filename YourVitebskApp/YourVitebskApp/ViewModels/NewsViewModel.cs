using System.Collections.Generic;
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
        private ObservableRangeCollection<News> _newsCollection;
        private IEnumerable<News> _newsList;
        private int _currentOffset;
        private bool _isBusy;
        private bool _isMainLayoutVisible;
        private bool _isInternetNotConnected;
        private bool _isRefreshing;
        private bool _isLoadingMore;
        private readonly NewsService _newsService;
        public AsyncCommand PageAppearingCommand { get; set; }
        public AsyncCommand<News> ItemTappedCommand { get; }
        public Command LoadMoreCommand { get; }
        public Command RefreshCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableRangeCollection<News> NewsCollection
        {
            get { return _newsCollection; }
            set
            {
                _newsCollection = value;
                OnPropertyChanged();
            }
        }

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

        public bool IsLoadingMore
        {
            get { return _isLoadingMore; }
            set
            {
                _isLoadingMore = value;
                OnPropertyChanged();
            }
        }

        public NewsViewModel()
        {
            NewsCollection = new ObservableRangeCollection<News>();
            _newsService = new NewsService();
            PageAppearingCommand = new AsyncCommand(OnAppearing);
            ItemTappedCommand = new AsyncCommand<News>(ItemTapped);
            LoadMoreCommand = new Command(LoadMoreData);
            RefreshCommand = new Command(Refresh);
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            IsInternetNotConnected = Connectivity.NetworkAccess != NetworkAccess.Internet;
        }

        private async Task OnAppearing()
        {
            IsBusy = true;
            await LoadData();
            IsBusy = false;
        }

        private async Task LoadData()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    NewsCollection.Clear();
                    NewsList = await _newsService.GetAll();
                    NewsCollection.AddRange(NewsList.Take(5));
                    _currentOffset = 5;
                }
                catch
                {
                    
                }
            }
        }

        private void LoadMoreData()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                IsLoadingMore = true;
                try
                {
                    NewsCollection.AddRange(NewsList.Skip(_currentOffset).Take(5));
                    OnPropertyChanged(nameof(NewsCollection));
                    _currentOffset += 5;
                }
                catch
                {

                }

                IsLoadingMore = false;
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
            await Shell.Current.GoToAsync($"{nameof(SpecificNewsPage)}?NewsId={news.NewsId}");
        }

        private void Refresh()
        {
            IsRefreshing = true;
            LoadData();
            IsRefreshing = false;
        }
    }
}
