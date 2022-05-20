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
    public class PostersViewModel : INotifyPropertyChanged
    {
        private IEnumerable<Poster> _postersList;
        private bool _isBusy;
        private bool _isRefreshing;
        private readonly PosterService _postersService;
        public AsyncCommand<Poster> ItemTappedCommand { get; }
        public Command RefreshCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerable<Poster> PostersList
        {
            get { return _postersList; }
            set
            {
                _postersList = value;
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

        public PostersViewModel()
        {
            IsBusy = true;
            _postersService = new PosterService();
            ItemTappedCommand = new AsyncCommand<Poster>(ItemTapped);
            RefreshCommand = new Command(Refresh);
            AddData();
            IsBusy = false;
        }

        private async void AddData()
        {
            PostersList = await _postersService.Get();
        }

        private void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private async Task ItemTapped(Poster poster)
        {
            IsBusy = true;
            await Shell.Current.GoToAsync($"{nameof(PostersPage)}/{nameof(SpecificPosterPage)}?PosterId={poster.PosterId}");
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
