using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourVitebskApp.Models;
using YourVitebskApp.Services;

namespace YourVitebskApp.SearchingData
{
    public static class NewsData
    {
        public static IList<News> News { get; private set; }

        static NewsData()
        {
            var service = new NewsService();
            News = Task.Run(async () => await service.Get()).Result.ToList();
        }
    }
}
