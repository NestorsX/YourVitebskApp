using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using YourVitebskApp.Services;

namespace YourVitebskApp.ViewModels
{
    public class AddCommentViewModel : INotifyPropertyChanged, IQueryAttributable
    {
        private bool _isRecommend;
        private bool _isUnrecommend;
        private string _message;
        private bool _isBusy;
        private bool _isMainLayoutVisible;
        private bool _isInternetNotConnected;
        private bool _isRefreshing;
        private readonly CommentService _commentService;
        public Command SendCommentCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;
        public int ServiceId { get; set; }
        public int CafeId { get; set; }

        public bool IsRecommend
        {
            get { return _isRecommend; }
            set
            {
                _isRecommend = value;
                OnPropertyChanged();
            }
        }

        public bool IsUnrecommend
        {
            get { return _isUnrecommend; }
            set
            {
                _isUnrecommend = value;
                OnPropertyChanged();
            }
        }

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
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

        public AddCommentViewModel()
        {
            IsBusy = true;
            _commentService = new CommentService();
            SendCommentCommand = new Command(SendComment);
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            IsInternetNotConnected = Connectivity.NetworkAccess != NetworkAccess.Internet;
            IsBusy = false;
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
            if (query.TryGetValue("ServiceId", out string param))
            {
                int.TryParse(param, out int id);
                ServiceId = id;
            }

            if (query.TryGetValue("CafeId", out param))
            {
                int.TryParse(param, out int id);
                CafeId = id;
            }
        }

        private async void SendComment()
        {
            IsRefreshing = true;
            await App.Current.MainPage.DisplayAlert("recommend", IsRecommend.ToString(), "OK");
            await App.Current.MainPage.DisplayAlert("unrecommend", $"{IsUnrecommend}", "OK");
            await App.Current.MainPage.DisplayAlert("", $"{ServiceId}", "OK");
            await App.Current.MainPage.DisplayAlert("", $"{CafeId}", "OK");
            await _commentService.AddComment(new Models.CommentDTO
            {
                UserId = Convert.ToInt32(await SecureStorage.GetAsync("UserId")),
                ServiceId = ServiceId,
                ItemId = CafeId,
                IsRecommend = IsRecommend,
                Message = Message,
                PublishDate = DateTime.Now
            });
            //
            IsRefreshing = false;
        }
    }
}
