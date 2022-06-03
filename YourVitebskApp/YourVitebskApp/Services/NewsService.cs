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
    public class NewsService
    {
        private const string _url = AppSettings.BaseApiUrl + "news/";
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
            string response = await _client.GetStringAsync(_url + "all");
            return JsonSerializer.Deserialize<IEnumerable<News>>(response, _options);
        }

        // Получаем новость по id
        public async Task<News> Get(int id)
        {
            var response = await _client.GetAsync(_url + id);
            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<News>(
                    JsonSerializer.Deserialize<ResponseModel>(
                        await response.Content.ReadAsStringAsync(),
                        _options).Content.ToString(), 
                    _options);
            }
            else
            {
                throw new ArgumentException(JsonSerializer.Deserialize<ResponseModel>(await response.Content.ReadAsStringAsync(), _options).ErrorMessage);
            }
        }
    }
}
