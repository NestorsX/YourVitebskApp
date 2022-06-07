using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using YourVitebskApp.Models;
using YourVitebskApp.Views;

namespace YourVitebskApp.Controls
{
    public class TransportSearchHandler : SearchHandler
    {
        protected override void OnQueryChanged(string oldValue, string newValue)
        {
            base.OnQueryChanged(oldValue, newValue);

            if (string.IsNullOrWhiteSpace(newValue))
            {
                ItemsSource = null;
            }
            else
            {
                ItemsSource = (BindingContext as IEnumerable<TransportSearchingModel>).Where(x =>
                x.TransportName.ToLower().Contains(newValue.ToLower())).ToList();
            }
        }

        protected override async void OnItemSelected(object item)
        {
            base.OnItemSelected(item);
            await Shell.Current.GoToAsync($"{nameof(VoatByTransportRoutesPage)}?TransportType={(item as TransportSearchingModel).TransportType}&TransportId={(item as TransportSearchingModel).TransportId}");
        }

        protected override void OnUnfocus()
        {
            base.OnUnfocus();
        }
    }
}
