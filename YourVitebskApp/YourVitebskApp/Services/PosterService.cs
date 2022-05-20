using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Essentials;
using YourVitebskApp.Models;

namespace YourVitebskApp.Services
{
    public class PosterService
    {
        private const string _url = "http://yourvitebsk.somee.com/api/posters/";
        private readonly JsonSerializerOptions _options;
        private readonly HttpClient _client;

        public PosterService()
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

        // Получаем список афиш
        public async Task<IEnumerable<Poster>> Get()
        {
            string response = await _client.GetStringAsync(_url + "all");
            return JsonSerializer.Deserialize<IEnumerable<Poster>>(response, _options);
        }

        // Получаем афишу по id
        public async Task<Poster> Get(int id)
        {
            var response = await _client.GetAsync(_url + id);
            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<Poster>(
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
