using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Essentials;
using YourVitebskApp.Helpers;
using YourVitebskApp.Models;

namespace YourVitebskApp.Services
{
    public class CommentService
    {
        private const string _url = AppSettings.BaseApiUrl + "/api/comments";
        private readonly JsonSerializerOptions _options;
        private readonly HttpClient _client;

        public CommentService()
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

        // Получаем список комментариев по объекту сервиса
        public async Task<IEnumerable<Comment>> Get(int serviceId, int itemId, int offset, int count)
        {
            string response = await _client.GetStringAsync($"{_url}/all?serviceId={serviceId}&itemId={itemId}&offset={offset}&count={count}");
            return JsonSerializer.Deserialize<IEnumerable<Comment>>(response, _options);
        }

        // добавление нового комментария
        public async Task AddComment(CommentDTO newComment)
        {
            await _client.PostAsync($"{_url}/", new StringContent(JsonSerializer.Serialize(newComment), Encoding.UTF8, "application/json"));
        }
    }
}
