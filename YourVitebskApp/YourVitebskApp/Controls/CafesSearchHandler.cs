using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using YourVitebskApp.Models;
using YourVitebskApp.Views;

namespace YourVitebskApp.Controls
{
    public class CafesSearchHandler : SearchHandler
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
                ItemsSource = (BindingContext as IEnumerable<Cafe>).Where(x => 
                    x.Title.ToLower().Contains(newValue.ToLower()) ||
                    x.CafeType.ToLower().Contains(newValue.ToLower()) ||
                    x.Address.ToLower().Contains(newValue.ToLower())).ToList();
            }
        }

        protected override async void OnItemSelected(object item)
        {
            base.OnItemSelected(item);
            await Shell.Current.GoToAsync($"{nameof(CafesPage)}/{nameof(SpecificCafePage)}?CafeId={(item as Cafe).CafeId}");
        }

        protected override void OnUnfocus()
        {
            base.OnUnfocus();
        }
    }
}
