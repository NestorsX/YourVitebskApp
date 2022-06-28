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
    public class SpecificCafeViewModel : INotifyPropertyChanged, IQueryAttributable
    {
        private ObservableRangeCollection<Comment> _commentsCollection;
        private IEnumerable<Comment> _commentsList;
        private Cafe _cafe;
        private int _currentOffset;
        private bool _isLinkAvailable;
        private bool _isBusy;
        private bool _isMainLayoutVisible;
        private bool _isInternetNotConnected;
        private bool _isRefreshing;
        private bool _isLoadingMore;
        private readonly CafeService _cafeService;
        private readonly CommentService _commentService;
        public event PropertyChangedEventHandler PropertyChanged;
        public AsyncCommand<string> TapCommand { get; set; }
        public Command LoadMoreCommand { get; }
        public Command AddCommentCommand { get; set; }
        public Command RefreshCommand { get; }
        public int CafeId { get; set; }

        public ObservableRangeCollection<Comment> CommentsCollection
        {
            get { return _commentsCollection; }
            set
            {
                _commentsCollection = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<Comment> CommentsList
        {
            get { return _commentsList; }
            set
            {
                _commentsList = value;
                OnPropertyChanged();
            }
        }

        public Cafe Cafe
        {
            get { return _cafe; }
            set
            {
                _cafe = value;
                OnPropertyChanged();
            }
        }

        public bool IsLinkAvailable
        {
            get { return _isLinkAvailable; }
            set
            {
                _isLinkAvailable = value;
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

        public SpecificCafeViewModel()
        {
            CommentsCollection = new ObservableRangeCollection<Comment>();
            _cafeService = new CafeService();
            _commentService = new CommentService();
            TapCommand = new AsyncCommand<string>(OpenURL);
            LoadMoreCommand = new Command(LoadMoreData);
            AddCommentCommand = new Command(AddComment);
            RefreshCommand = new Command(Refresh);
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            IsInternetNotConnected = Connectivity.NetworkAccess != NetworkAccess.Internet;
        }

        private async Task LoadData()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    CommentsCollection.Clear();
                    Cafe = await _cafeService.Get(CafeId);
                    IsLinkAvailable = Cafe.ExternalLink != null;
                    CommentsList = await _commentService.GetAll(1, CafeId);
                    CommentsCollection.AddRange(CommentsList.Take(5));
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
                    CommentsCollection.AddRange(CommentsList.Skip(_currentOffset).Take(5));
                    OnPropertyChanged(nameof(CommentsCollection));
                    _currentOffset += 5;
                }
                catch
                {

                }

                IsLoadingMore = false;
            }
        }

        private async Task OpenURL(string url)
        {
            await Browser.OpenAsync(url, new BrowserLaunchOptions
            {
                LaunchMode = BrowserLaunchMode.SystemPreferred,
                TitleMode = BrowserTitleMode.Show
            });
        }

        private async void AddComment()
        {
            await Shell.Current.GoToAsync($"{nameof(SpecificCafePage)}/{nameof(AddCommentPage)}?ServiceId={1}&ItemId={CafeId}");
        }

        private async void Refresh()
        {
            IsRefreshing = true;
            await LoadData();
            IsRefreshing = false;
        }

        private void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            IsInternetNotConnected = e.NetworkAccess != NetworkAccess.Internet;
        }

        public async void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            IsBusy = true;
            if (query.TryGetValue("CafeId", out string param))
            {
                int.TryParse(param, out int id);
                CafeId = id;
                await LoadData();
            }

            IsBusy = false;
        }
    }
}
