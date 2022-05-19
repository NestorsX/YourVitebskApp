using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using YourVitebskApp.Models;
using YourVitebskApp.Services;

namespace YourVitebskApp.ViewModels
{
    public class NewsViewModel : INotifyPropertyChanged
    {
        private IEnumerable<News> _newsList;
        private bool _isBusy;
        private NewsService _newsService;
        public AsyncCommand<News> ItemTappedCommand { get; }
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
                OnPropertyChanged();
            }
        }

        public NewsViewModel()
        {
            IsBusy = true;
            _newsService = new NewsService();
            ItemTappedCommand = new AsyncCommand<News>(ItemTapped);
            RefreshCommand = new Command(Refresh);
            AddData();
            IsBusy = false;
        }

        private async void AddData()
        {
            NewsList = await _newsService.Get();
        }

        private void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private async Task ItemTapped(News news)
        {
            IsBusy = true;
            await Application.Current.MainPage.DisplayAlert("WOW!", news.NewsId.ToString(), "OK");
            IsBusy = false;
        }

        private void Refresh()
        {
            IsBusy = true;
            AddData();
            IsBusy = false;
        }
    }
}
