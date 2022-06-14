using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Essentials;
using YourVitebskApp.Helpers;
using YourVitebskApp.Models;

namespace YourVitebskApp.Services
{
    public class UserService
    {
        private const string _url = AppSettings.BaseApiUrl + "/api/users";
        private readonly JsonSerializerOptions _options;
        private readonly HttpClient _client;
        public UserService()
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

        // Получаем список пользователей за исключением указанного id
        public async Task<IEnumerable<UsersListItem>> Get(int id, int offset, int count)
        {
            string response = await _client.GetStringAsync($"{_url}/all/{id}?offset={offset}&count={count}");
            var result = JsonSerializer.Deserialize<IEnumerable<UsersListItem>>(response, _options);
            foreach (var item in result)
            {
                item.Image = $"{AppSettings.BaseApiUrl}/images/Users/{item.UserId}/{item.Image}";
            }

            return result;
        }
    }
}
