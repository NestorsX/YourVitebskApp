using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Essentials;
using YourVitebskApp.Models;

namespace YourVitebskApp.Services
{
    public class BusService
    {
        private const string _url = AppSettings.BaseApiUrl + "buses/";
        private readonly JsonSerializerOptions _options;
        private readonly HttpClient _client;

        public BusService()
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

        // Получаем список автобусов
        public async Task<IEnumerable<Bus>> Get()
        {
            string response = await _client.GetStringAsync(_url + "buses");
            return JsonSerializer.Deserialize<IEnumerable<Bus>>(response, _options);
        }

        // Получаем маршруты автобуса по его id
        public async Task<IEnumerable<BusRoute>> Get(int id)
        {
            var response = await _client.GetAsync(_url + $"routes/{id}");
            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<IEnumerable<BusRoute>>(
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

        // Получаем расписание на определенной остановке
        public async Task<BusShedule> Update(User user)
        {
            var response = await _client.PostAsync(_url + "shedule", new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<BusShedule>(
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
