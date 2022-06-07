using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using YourVitebskApp.Models;
using YourVitebskApp.Views;

namespace YourVitebskApp.Controls
{
    internal class PeopleSearchHandler : SearchHandler
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
                ItemsSource = (BindingContext as IEnumerable<UsersListItem>).Where(x =>
                    x.FirstName.ToLower().Contains(newValue.ToLower()) ||
                    x.LastName.ToLower().Contains(newValue.ToLower()) ||
                    x.PhoneNumber.ToLower().Contains(newValue.ToLower())).ToList();
            }
        }

        protected override async void OnItemSelected(object item)
        {
            base.OnItemSelected(item);
            if (!(item as UsersListItem).PhoneNumber.Equals("Номер телефона не указан"))
            {
                await Task.Run(() => PhoneDialer.Open((item as UsersListItem).PhoneNumber));
            }
        }

        protected override void OnUnfocus()
        {
            base.OnUnfocus();
        }
    }
}
