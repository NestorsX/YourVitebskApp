using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YourVitebskApp.ViewModels;

namespace YourVitebskApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddCommentPage : ContentPage
    {
        public AddCommentPage()
        {
            InitializeComponent();
            BindingContext = new AddCommentViewModel();
        }
    }
}