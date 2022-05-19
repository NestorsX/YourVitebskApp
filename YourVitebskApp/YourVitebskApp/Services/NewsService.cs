using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Essentials;
using YourVitebskApp.Models;

namespace YourVitebskApp.Services
{
    public class NewsService
    {
        private const string _url = "http://yourvitebsk.somee.com/api/news/news/";
        private readonly JsonSerializerOptions _options;
        private readonly HttpClient _client;

        public NewsService()
        {
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            var token = Task.Run(async () => await SecureStorage.GetAsync("Token")).Result;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        // Получаем список новостей
        public async Task<IEnumerable<News>> Get()
        {
            string result = await _client.GetStringAsync(_url + "all");
            return JsonSerializer.Deserialize<IEnumerable<News>>(result, _options);
        }

        // Получаем новость по id
        public async Task<News> Get(int id)
        {
            string result = await _client.GetStringAsync(_url + id);
            return JsonSerializer.Deserialize<News>(result, _options);
        }
    }
}
