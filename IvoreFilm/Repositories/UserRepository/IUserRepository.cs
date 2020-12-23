using System.Threading.Tasks;
using IvoreFilm.Helpers.KeycloakHelpers;
using IvoreFilm.Models;
using IvoreFilm.Models.ViewModel;

namespace IvoreFilm.Repositories.UserRepository
{
    public interface IUserRepository
    {
        public Task<ResponseModel> AddUser(UsersViewModel user);

        public Task<TokenResponse> Login(TokenRequest request);

        public Task<ResponseModel> Logout(LogoutRequest request);

        public Task<UserInfo> UserProfile(string token);
    }
}