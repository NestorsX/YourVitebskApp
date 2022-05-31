using Xamarin.Forms;
using System.Windows.Input;
using YourVitebskApp.Views;

namespace YourVitebskApp.Models
{
    public class VoatByTransportItem
    {
        public string tr_n { get; set; }
        public string mar_id { get; set; }
        public string vid_mar_n { get; set; }
        public string ost_id { get; set; }
        public string napr_id { get; set; }
        public string info_marsh { get; set; }

        private Command itemTappedCommand;

        public ICommand ItemTappedCommand
        {
            get
            {
                if (itemTappedCommand == null)
                {
                    itemTappedCommand = new Command<string>(async (key) =>
                    {
                        await App.Current.MainPage.DisplayAlert("", key, "ok");
                        await Shell.Current.GoToAsync($"{nameof(VoatByTransportRoutesPage)}");
                    });
                }

                return itemTappedCommand;
            }
        }
    }
}
