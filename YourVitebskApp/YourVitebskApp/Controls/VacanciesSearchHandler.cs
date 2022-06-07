using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using YourVitebskApp.Models;
using YourVitebskApp.Views;

namespace YourVitebskApp.Controls
{
    internal class VacanciesSearchHandler : SearchHandler
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
                ItemsSource = (BindingContext as IEnumerable<Vacancy>).Where(x =>
                    x.Title.ToLower().Contains(newValue.ToLower()) ||
                    x.CompanyName.ToLower().Contains(newValue.ToLower()) ||
                    x.Salary.ToLower().Contains(newValue.ToLower())).ToList();
            }
        }

        protected override async void OnItemSelected(object item)
        {
            base.OnItemSelected(item);
            await Shell.Current.GoToAsync($"{nameof(VacanciesPage)}/{nameof(SpecificVacancyPage)}?VacancyId={(item as Vacancy).VacancyId}");
        }

        protected override void OnUnfocus()
        {
            base.OnUnfocus();
        }
    }
}
