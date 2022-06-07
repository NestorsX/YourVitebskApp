using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using YourVitebskApp.Models;
using YourVitebskApp.Views;

namespace YourVitebskApp.Controls
{
    internal class PostersSearchHandler : SearchHandler
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
                ItemsSource = (BindingContext as IEnumerable<Poster>).Where(x =>
                    x.Title.ToLower().Contains(newValue.ToLower()) ||
                    x.PosterType.ToLower().Contains(newValue.ToLower()) ||
                    x.Address.ToLower().Contains(newValue.ToLower())).ToList();
            }
        }

        protected override async void OnItemSelected(object item)
        {
            base.OnItemSelected(item);
            await Shell.Current.GoToAsync($"{nameof(PostersPage)}/{nameof(SpecificPosterPage)}?PosterId={(item as Poster).PosterId}");
        }

        protected override void OnUnfocus()
        {
            base.OnUnfocus();
        }
    }
}
