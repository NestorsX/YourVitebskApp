using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using YourVitebskApp.Models;

namespace YourVitebskApp.Services
{
    public class TransportSheduleService
    {
        private readonly JsonSerializerOptions _options;
        private readonly HttpClient _client;

        public TransportSheduleService()
        {
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        // Получаем список автобусов
        public async Task<VoatData> GetTransportsInfo()
        {
            string response = await _client.GetStringAsync("https://voat.by/schedule/transps");
            return JsonSerializer.Deserialize<VoatData>(response, _options);
        }
    }
}
