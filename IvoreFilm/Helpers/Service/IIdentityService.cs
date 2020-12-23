using System.Collections.Generic;
using System.Threading.Tasks;
using IvoreFilm.Helpers.KeycloakHelpers;
using Keycloak.Net.Models.Users;

namespace IvoreFilm.Models.Service
{
    public interface IIdentityService
    {
        TokenResponse? GetToken(TokenRequest request);
        UserInfo GetUserInfo(string token);
        void Logout(LogoutRequest request);
        Task<User> GetOneUserByEmailAsync(string email);
        Task<User?> CreateUserAndSetPasswordAsync(User request, string password, Dictionary<string, string> fields);
     
    }
}