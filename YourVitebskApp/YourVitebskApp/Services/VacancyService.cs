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
    public class VacancyService
    {
        private const string _url = "http://yourvitebsk.somee.com/api/vacancies/";
        private readonly JsonSerializerOptions _options;
        private readonly HttpClient _client;

        public VacancyService()
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

        // Получаем список вакансий
        public async Task<IEnumerable<Vacancy>> Get()
        {
            string response = await _client.GetStringAsync(_url + "all");
            return JsonSerializer.Deserialize<IEnumerable<Vacancy>>(response, _options);
        }

        // Получаем вакансию по id
        public async Task<Vacancy> Get(int id)
        {
            var response = await _client.GetAsync(_url + id);
            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<Vacancy>(
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
