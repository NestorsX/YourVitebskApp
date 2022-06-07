using System.Collections.Generic;

namespace YourVitebskApp.Models
{
    public class News
    {
        public int? NewsId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ExternalLink { get; set; }
        public string TitleImage { get; set; }
        public IEnumerable<string> Images { get; set; }
    }
}
