using System;
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
        public async Task<IEnumerable<UsersListItem>> GetAll(int id)
        {
            var response = await _client.GetStringAsync($"{_url}/all/{id}");
            var result = JsonSerializer.Deserialize<IEnumerable<UsersListItem>>(response, _options);
            foreach (var item in result)
            {
                if (!string.IsNullOrWhiteSpace(item.Image))
                {
                    item.Image = $"{AppSettings.BaseApiUrl}/images/Users/{item.UserId}/{item.Image}";
                    continue;
                }

                item.Image = "icon_noavatar.png";
            }

            return result;
        }

        // Получаем кол-во комментариев пользователя по id
        public async Task<string> GetCommentsCount(int id)
        {
            var response = await _client.GetAsync($"{_url}/commentscount/{id}");
            if(response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<ResponseModel>(await response.Content.ReadAsStringAsync(), _options).Content.ToString();
            }
            else
            {
                throw new ArgumentException(JsonSerializer.Deserialize<ResponseModel>(await response.Content.ReadAsStringAsync(), _options).ErrorMessage);
            }
        }
    }
}
