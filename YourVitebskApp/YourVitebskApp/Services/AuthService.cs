using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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
    public class AuthService
    {
        private const string _url = AppSettings.BaseApiUrl + "/api/auth/";
        private readonly JsonSerializerOptions _options;
        private readonly HttpClient _client;

        public AuthService()
        {
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async void SaveUserCreds(string token)
        {
            await SecureStorage.SetAsync("Token", token);
            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtSecurityToken = handler.ReadJwtToken(token);
            await SecureStorage.SetAsync("UserId", jwtSecurityToken.Claims.First(x => x.Type == "UserId").Value);
            await SecureStorage.SetAsync("Email", jwtSecurityToken.Claims.First(x => x.Type == "Email").Value);
            await SecureStorage.SetAsync("FirstName", jwtSecurityToken.Claims.First(x => x.Type == "FirstName").Value);
            await SecureStorage.SetAsync("LastName", jwtSecurityToken.Claims.First(x => x.Type == "LastName").Value);
            await SecureStorage.SetAsync("PhoneNumber", jwtSecurityToken.Claims.First(x => x.Type == "PhoneNumber").Value);
            await SecureStorage.SetAsync("IsVisible", jwtSecurityToken.Claims.First(x => x.Type == "IsVisible").Value);
            string image = jwtSecurityToken.Claims.First(x => x.Type == "Image").Value;
            if (string.IsNullOrEmpty(image))
            {
                image = "icon_noavatar.png";
            }
            else
            {
                image = $"{AppSettings.BaseApiUrl}/images/Users/{await SecureStorage.GetAsync("UserId")}/{image}";
            }

            await SecureStorage.SetAsync("Image", image);
            await SecureStorage.SetAsync("Expires", jwtSecurityToken.Claims.First(x => x.Type == "exp").Value);
        }

        public async Task RestorePassword(string email, string firstName)
        {
            var response = await _client.GetAsync(_url + $"restorepassword?email={email}&firstName={firstName}");
            if (!response.IsSuccessStatusCode)
            {
                throw new ArgumentException(JsonSerializer.Deserialize<ResponseModel>(await response.Content.ReadAsStringAsync(), _options).ErrorMessage);
            }
        }

        // Авторизация пользователя и получение токена
        public async Task<string> Login(UserLoginDTO loginCreds)
        {
            var response = await _client.PostAsync(_url + "login/", new StringContent(JsonSerializer.Serialize(loginCreds), Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<ResponseModel>(await response.Content.ReadAsStringAsync(), _options).Content.ToString();
            }
            else
            {
                throw new ArgumentException(JsonSerializer.Deserialize<ResponseModel>(await response.Content.ReadAsStringAsync(), _options).ErrorMessage);
            }
        }

        // Авторизация пользователя и получение токена
        public async Task<string> Register(UserRegisterDTO registerCreds)
        {
            var response = await _client.PostAsync(_url + "register/", new StringContent(JsonSerializer.Serialize(registerCreds), Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<ResponseModel>(await response.Content.ReadAsStringAsync(), _options).Content.ToString();
            }
            else
            {
                throw new ArgumentException(JsonSerializer.Deserialize<ResponseModel>(await response.Content.ReadAsStringAsync(), _options).ErrorMessage);
            }
        }

        // Редактирование данных пользователя и получение актуального токена
        public async Task<string> Update(User user)
        {
            var token = Task.Run(async () => await SecureStorage.GetAsync("Token")).Result;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.PostAsync(_url + "update/", new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
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
