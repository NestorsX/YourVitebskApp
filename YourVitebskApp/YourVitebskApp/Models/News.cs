using Xamarin.Essentials;
using Xamarin.Forms;

namespace YourVitebskApp.Models
{
    public class News
    {
        public int? NewsId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ExternalLink { get; set; }

        public Command TapCommand => new Command<string>(async (url) => await Launcher.OpenAsync(url));
    }
}
