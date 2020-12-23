using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using IvoreFilm.Helpers;
using IvoreFilm.Helpers.KeycloakHelpers;
using Keycloak.Net;
using Keycloak.Net.Models.Users;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace IvoreFilm.Models.Service
{
    public class IdentityService : IIdentityService
    {
        private readonly string _apiUri;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _realM;

        public IdentityService(IConfiguration configuration)
        {
            var configuration1 = configuration;
            _realM = configuration1.GetValue<string>("IdentitySettings:Realm");
            _apiUri = configuration1.GetValue<string>("IdentitySettings:ApiUrl");
            _clientId = configuration1.GetValue<string>("IdentitySettings:ClientId");
            _clientSecret = configuration1.GetValue<string>("IdentitySettings:ClientSecret");
            var username = configuration1.GetValue<string>("IdentitySettings:Username");
            var password = configuration1.GetValue<string>("IdentitySettings:Password");
            KeycloakClient = new KeycloakClient(_apiUri, username, password);
        }

        private KeycloakClient KeycloakClient { get; }

        public TokenResponse? GetToken(TokenRequest request)
        {
            var restClient = new RestClient($"{_apiUri}/auth/realms/{_realM}/protocol/openid-connect/token");

            var restRequest = new RestRequest(Method.POST)
            {
                RequestFormat = DataFormat.Json,
                JsonSerializer = new NewtonsoftJsonSerializer()
            };

            restRequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            restRequest.AddParameter("client_id", _clientId);
            restRequest.AddParameter("client_secret", _clientSecret);
            restRequest.AddParameter("username", request.Username);
            restRequest.AddParameter("password", request.Password);
            restRequest.AddParameter("grant_type", "password");

            var restResponse = restClient.Execute<TokenResponse>(restRequest);

            if (restResponse.StatusCode != HttpStatusCode.OK || restResponse.ErrorException != null) return null;

            return restResponse.Data;
        }

        public TokenResponse? RefreshToken(string refreshToken)
        {
            var restClient = new RestClient($"{_apiUri}/auth/realms/{_realM}/protocol/openid-connect/token");

            var restRequest = new RestRequest(Method.POST)
            {
                RequestFormat = DataFormat.Json,
                JsonSerializer = new NewtonsoftJsonSerializer()
            };

            restRequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            restRequest.AddParameter("client_id", _clientId);
            restRequest.AddParameter("client_secret", _clientSecret);
            restRequest.AddParameter("refresh_token", refreshToken);
            restRequest.AddParameter("grant_type", "refresh_token");

            var restResponse = restClient.Execute<TokenResponse>(restRequest);

            if (restResponse.StatusCode != HttpStatusCode.OK || restResponse.ErrorException != null) return null;

            return restResponse.Data;
        }

        public UserInfo GetUserInfo(string token)
        {
            var restClient = new RestClient($"{_apiUri}/auth/realms/{_realM}/protocol/openid-connect");

            var restRequest = new RestRequest("userinfo", Method.GET)
            {
                RequestFormat = DataFormat.Json,
                JsonSerializer = new NewtonsoftJsonSerializer()
            };

            restRequest.AddHeader("Authorization", $"Bearer {token}");

            var restResponse = restClient.Execute<UserInfo>(restRequest);

            if (restResponse.ErrorException != null) throw restResponse.ErrorException;

            if (restResponse.StatusCode >= HttpStatusCode.Ambiguous) throw new Exception(restResponse.Content);

            return restResponse.Data;
        }

        public void Logout(LogoutRequest request)
        {
            var restClient = new RestClient($"{_apiUri}/auth/realms/{_realM}/protocol/openid-connect/logout");

            var restRequest = new RestRequest(Method.POST)
            {
                RequestFormat = DataFormat.Json,
                JsonSerializer = new NewtonsoftJsonSerializer()
            };

            restRequest.AddHeader("Authorization", $"Bearer {request.Token}");

            restRequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            restRequest.AddParameter("client_id", _clientId);
            restRequest.AddParameter("client_secret", _clientSecret);
            restRequest.AddParameter("refresh_token", request.RefreshToken);

            restClient.Execute(restRequest);
        }

        public async Task<User> GetUserAsync(string id)
        {
            return await KeycloakClient.GetUserAsync(_realM, id);
        }

        public async Task<List<User>> GetUsersByIdAsync(List<string> ids)
        {
            var users = await KeycloakClient.GetUsersAsync(_realM);
            return users.Where(x => ids.Contains(x.Id)).ToList();
        }

        public async Task<List<User>> GetUsersByEmailAsync(string email)
        {
            var users = await KeycloakClient.GetUsersAsync(_realM, email: email);
            return users.ToList();
        }

        public async Task<User> GetOneUserByEmailAsync(string email)
        {
            var users = await KeycloakClient.GetUsersAsync(_realM, email: email);
            return users.FirstOrDefault();
        }

        public async Task<User?> CreateUserAndSetPasswordAsync(User request, string password,
            Dictionary<string, string> fields)
        {
            User user = null;

            var create = await KeycloakClient.CreateUserAsync(_realM, request);

            if (create)
            {
                user = await GetOneUserByEmailAsync(request.Email);
            }

            if (user != null)
            {
                var resetPassword = await KeycloakClient.ResetUserPasswordAsync(_realM, user.Id, new Credentials
                {
                    Temporary = false,
                    Type = "password",
                    Value = password
                });

                if (resetPassword)
                {
                    user.Attributes ??= new Dictionary<string, IEnumerable<string>>();

                    foreach (var (k, v) in fields) user.Attributes[k] = new[] {v};

                    await KeycloakClient.UpdateUserAsync(_realM, user.Id, user);
                    return user;
                }
            }

            return null;
        }

        public async Task<bool> ResetPasswordAsync(string id, string password)
        {
            return await KeycloakClient.ResetUserPasswordAsync(_realM, id, new Credentials
            {
                Temporary = false,
                Type = "password",
                Value = password
            });
        }

        public async Task<bool> UpdateUserAsync(string userId, User user)
        {
            return await KeycloakClient.UpdateUserAsync(_realM, userId, user);
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            return await KeycloakClient.DeleteUserAsync(_realM, userId);
        }
    }
}