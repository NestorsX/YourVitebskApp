using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Essentials;
using Xamarin.Forms;
using YourVitebskApp.Services;

namespace YourVitebskApp.ViewModels
{
    public class AddCommentViewModel : INotifyPropertyChanged, IQueryAttributable
    {
        private bool _isRecommend;
        private string _message;
        private string _error;
        private bool _isBusy;
        private bool _isError;
        private bool _isMainLayoutVisible;
        private bool _isInternetNotConnected;
        private readonly CommentService _commentService;
        public Command SendCommentCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;
        public int ServiceId { get; set; }
        public int ItemId { get; set; }

        public bool IsRecommend
        {
            get { return _isRecommend; }
            set
            {
                _isRecommend = value;
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

        public string Error
        {
            get { return _error; }
            set
            {
                _error = value;
                IsError = true;
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

        public bool IsError
        {
            get { return _isError; }
            set
            {
                _isError = value;
                OnPropertyChanged();
                if (IsError)
                {
                    OnPropertyChanged(nameof(DisplayMessage));
                }
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

        public string DisplayMessage
        {
            get { return _error; }
        }

        public AddCommentViewModel()
        {
            _commentService = new CommentService();
            SendCommentCommand = new Command(SendComment);
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            IsInternetNotConnected = Connectivity.NetworkAccess != NetworkAccess.Internet;
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

            if (query.TryGetValue("ItemId", out param))
            {
                int.TryParse(param, out int id);
                ItemId = id;
            }
        }

        private async void SendComment()
        {
            IsBusy = true;
            try
            {
                await _commentService.AddComment(new Models.CommentDTO
                {
                    UserId = Convert.ToInt32(await SecureStorage.GetAsync("UserId")),
                    ServiceId = ServiceId,
                    ItemId = ItemId,
                    IsRecommend = IsRecommend,
                    Message = Message,
                    PublishDate = DateTime.UtcNow
                });

                await Shell.Current.GoToAsync("..");
                IsBusy = false;
            }
            catch(ArgumentException e)
            {
                Error = e.Message;
                IsBusy = false;
            }
        }
    }
}
