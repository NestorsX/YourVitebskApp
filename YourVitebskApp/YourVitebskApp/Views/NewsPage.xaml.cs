using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YourVitebskApp.Models;

namespace YourVitebskApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewsPage : ContentPage
    {
        public List<News> NewsList { get; set; }
        public NewsPage()
        {
            InitializeComponent();
            NewsList = new List<News>()
            {
                new News() { Id = 1, Title = "Новость 1", Description = "Описание 1", Image = "news_example.png"},
                new News() { Id = 2, Title = "Новость 2", Description = "Описание 2", Image = "news_example.png"},
                new News() { Id = 3, Title = "Новость 3", Description = "Описание 3", Image = "news_example.png"},
                new News() { Id = 4, Title = "Новость 4", Description = "Описание 4", Image = "news_example.png"},
                new News() { Id = 5, Title = "Новость 5", Description = "Описание 5", Image = "news_example.png"}
            };

            this.BindingContext = this;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        public async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            News selectedNews = e.Item as News;
            if (selectedNews != null)
                await DisplayAlert("Выбранна новость ", $"\"{selectedNews.Title}\"", "OK");
        }
    }
}