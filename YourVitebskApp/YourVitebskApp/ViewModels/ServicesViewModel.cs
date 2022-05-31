using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using YourVitebskApp.Views;

namespace YourVitebskApp.ViewModels
{
    public class ServicesViewModel : INotifyPropertyChanged
    {
        private bool _isBusy;
        private bool _isMainLayoutVisible;
        private bool _isInternetNotConnected;
        public event PropertyChangedEventHandler PropertyChanged;
        public Command GoToTransportSheduleCommand { get; }
        public Command GoToPostersCommand { get; }
        public Command GoToCafesCommand { get; }
        public Command GoToVacanciesCommand { get; }

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

        public ServicesViewModel()
        {
            IsBusy = true;
            GoToTransportSheduleCommand = new Command(async () => await GoToTransportShedule());
            GoToPostersCommand = new Command(async () => await GoToPosters());
            GoToCafesCommand = new Command(async () => await GoToCafes());
            GoToVacanciesCommand = new Command(async () => await GoToVacancies());
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

        private async Task GoToCafes()
        {
            IsBusy = true;
            await Shell.Current.GoToAsync($"{nameof(ServicesPage)}/{nameof(CafesPage)}");
            IsBusy = false;
        }

        private async Task GoToTransportShedule()
        {
            IsBusy = true;
            await Shell.Current.GoToAsync($"{nameof(ServicesPage)}/{nameof(VoatByTransportsPage)}");
            IsBusy = false;
        }

        private async Task GoToPosters()
        {
            IsBusy = true;
            await Shell.Current.GoToAsync($"{nameof(ServicesPage)}/{nameof(PostersPage)}");
            IsBusy = false;
        }

        private async Task GoToVacancies()
        {
            IsBusy = true;
            await Shell.Current.GoToAsync($"{nameof(ServicesPage)}/{nameof(VacanciesPage)}");
            IsBusy = false;
        }
    }
}
