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

        // Получаем транспорт
        public async Task<IEnumerable<VoatByTransportData>> GetTransportsInfo()
        {
            string response = await _client.GetStringAsync("https://voat.by/schedule/transps");
            IEnumerable<VoatByTransportData> result = JsonSerializer.Deserialize<VoatByContent<VoatByTransportData>>(response, _options).data;
            foreach (var item in result)
            {
                string transportType = item.attributes.vid_tr;
                foreach (var transport in item.attributes.transpes)
                {
                    transport.vid_tr = transportType;
                }
            }

            return result;
        }

        // Получаем маршруты транспорта
        public async Task<IEnumerable<VoatByRoutesData>> GetTransportRoutes(string transportId)
        {
            string response = await _client.GetStringAsync($"https://voat.by/schedule/ost-by-marshes?mar={transportId}");
            return JsonSerializer.Deserialize<VoatByContent<VoatByRoutesData>>(response, _options).data;
        }

        // Получаем расписание транспорта на определенной остановке по выбранному маршруту
        public async Task<IEnumerable<VoatBySheduleData>> GetTransportShedule(string transportId, string directionId, string stopId, string day, string transportType)
        {
            string response = await _client.GetStringAsync($"https://voat.by/schedule/times?mar={transportId}&napr={directionId}&stopping={stopId}&type_day={day}&type_tr={transportType}");
            return JsonSerializer.Deserialize<VoatByContent<VoatBySheduleData>>(response, _options).data;
        }
    }
}
