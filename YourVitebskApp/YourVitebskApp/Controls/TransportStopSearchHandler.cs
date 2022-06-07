using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using YourVitebskApp.Models;
using YourVitebskApp.Views;

namespace YourVitebskApp.Controls
{
    public class TransportStopSearchHandler : SearchHandler
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
                ItemsSource = (BindingContext as IEnumerable<TransportStopSearchingModel>).Where(x =>
                x.StopName.ToLower().Contains(newValue.ToLower())).ToList();
            }
        }

        protected override async void OnItemSelected(object item)
        {
            base.OnItemSelected(item);
            await Shell.Current.GoToAsync($"{nameof(VoatByTransportShedulePage)}?" +
                            $"TransportId={(item as TransportStopSearchingModel).TransportId}&" +
                            $"DirectionId={(item as TransportStopSearchingModel).DirectionId}&" +
                            $"StopId={(item as TransportStopSearchingModel).TransportStopId}&" +
                            $"TransportType={(item as TransportStopSearchingModel).TransportType}");
        }

        protected override void OnUnfocus()
        {
            base.OnUnfocus();
        }
    }
}
