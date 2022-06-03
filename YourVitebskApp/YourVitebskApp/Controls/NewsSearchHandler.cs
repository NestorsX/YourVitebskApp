using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using YourVitebskApp.Models;
using YourVitebskApp.Views;

namespace YourVitebskApp.Controls
{
    public class NewsSearchHandler : SearchHandler
    {
        public IList<News> News { get; set; }

        protected override void OnQueryChanged(string oldValue, string newValue)
        {
            base.OnQueryChanged(oldValue, newValue);

            if (string.IsNullOrWhiteSpace(newValue))
            {
                ItemsSource = null;
            }
            else
            {
                ItemsSource = News.Where(x => x.Title.ToLower().Contains(newValue.ToLower())).ToList();
            }
        }

        protected override async void OnItemSelected(object item)
        {
            base.OnItemSelected(item);
            await Shell.Current.GoToAsync($"{nameof(SpecificNewsPage)}?NewsId={((News)item).NewsId}");
        }

        protected override void OnUnfocus()
        {
            base.OnUnfocus();
        }
    }
}
