using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YourVitebskApp.ViewModels;

namespace YourVitebskApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RestorePasswordPage : ContentPage
    {
        public RestorePasswordPage()
        {
            InitializeComponent();
            BindingContext = new RestorePasswordViewModel();
        }
    }
}