using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using YourVitebskApp.Models;
using YourVitebskApp.Services;
using YourVitebskApp.Views;

namespace YourVitebskApp.ViewModels
{
    public class SpecificPosterViewModel : INotifyPropertyChanged, IQueryAttributable
    {
        private ObservableRangeCollection<Comment> _commentsCollection;
        private IEnumerable<Comment> _commentsList;
        private Poster _poster;
        private int _currentOffset;
        private bool _isBusy;
        private bool _isMainLayoutVisible;
        private bool _isInternetNotConnected;
        private bool _isRefreshing;
        private bool _isLoadingMore;
        private readonly PosterService _posterService;
        private readonly CommentService _commentService;
        public event PropertyChangedEventHandler PropertyChanged;
        public Command TapCommand { get; set; }
        public Command LoadMoreCommand { get; }
        public Command AddCommentCommand { get; set; }
        public Command RefreshCommand { get; }
        public int PosterId { get; set; }

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

        public Poster Poster
        {
            get { return _poster; }
            set
            {
                _poster = value;
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

        public SpecificPosterViewModel()
        {
            CommentsCollection = new ObservableRangeCollection<Comment>();
            _posterService = new PosterService();
            _commentService = new CommentService();
            TapCommand = new Command<string>(async (url) => await Launcher.OpenAsync(url));
            LoadMoreCommand = new Command(LoadMoreData);
            AddCommentCommand = new Command(AddComment);
            RefreshCommand = new Command(Refresh);
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            IsInternetNotConnected = Connectivity.NetworkAccess != NetworkAccess.Internet;
        }

        private async void LoadData()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    CommentsCollection.Clear();
                    Poster = await _posterService.Get(PosterId);
                    CommentsList = await _commentService.GetAll(2, PosterId);
                    CommentsCollection.AddRange(CommentsList.Take(5));
                    _currentOffset = 5;
                }
                catch
                {

                }
            }
            Poster = await _posterService.Get(PosterId);
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

        public async void AddComment()
        {
            IsBusy = true;
            await Shell.Current.GoToAsync($"{nameof(SpecificPosterPage)}/{nameof(AddCommentPage)}?ServiceId={2}&ItemId={PosterId}");
            IsBusy = false;
        }

        private void Refresh()
        {
            IsRefreshing = true;
            LoadData();
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

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            if (query.TryGetValue("PosterId", out string param))
            {
                int.TryParse(param, out int id);
                PosterId = id;
                LoadData();
            }
        }
    }
}
