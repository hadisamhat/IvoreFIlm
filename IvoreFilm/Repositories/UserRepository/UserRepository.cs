using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IvoreFilm.Helpers.KeycloakHelpers;
using IvoreFilm.Models;
using IvoreFilm.Models.DbModels;
using IvoreFilm.Models.Service;
using IvoreFilm.Models.ViewModel;
using Keycloak.Net.Models.Users;

namespace IvoreFilm.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly IvoreFilmContext _context;
        private readonly IIdentityService _keycloakService;
        private AppUser _appUser;
        private CancellationToken _cancellationToken;

        public UserRepository(IIdentityService identityService, IvoreFilmContext context)
        {
            _keycloakService = identityService;
            _context = context;
        }

        public async Task<ResponseModel> AddUser(UsersViewModel usersViewModel)
        {
            var email = usersViewModel.Email.Trim().ToLower();
            var existingUser = await _keycloakService.GetOneUserByEmailAsync(email);
            if (existingUser == null)
            {
                var keycloakUser = await _keycloakService.CreateUserAndSetPasswordAsync(new User
                {
                    UserName = usersViewModel.Email,
                    Email = usersViewModel.Email,
                    Enabled = true,
                    FirstName = usersViewModel.FirstName,
                    LastName = usersViewModel.LastName
                }, usersViewModel.Password, new Dictionary<string, string>
                {
                    {"Roles", "User"}
                });

                if (keycloakUser == null) throw new Exception("Something went wrong! Sign up has failed");

                _appUser = new AppUser
                {
                    FirstName = usersViewModel.FirstName,
                    LastName = usersViewModel.LastName,
                    UserId = new Guid(keycloakUser.Id).ToString()
                };
                _context.AppUsers.Add(_appUser);
                await _context.SaveChangesAsync(_cancellationToken);
                return ResponseModel.ReturnSuccess();
            }

            throw new Exception("User ALready Exist!");
        }

        public async Task<TokenResponse> Login(TokenRequest request)
        {
            var loginResponse = _keycloakService.GetToken(request);
            if (loginResponse == null)
                throw new UnauthorizedAccessException("The provided credentials are not correct");

            return loginResponse;
        }

        public async Task<ResponseModel> Logout(LogoutRequest request)
        {
            _keycloakService.Logout(request);
            return ResponseModel.ReturnSuccess();
        }

        public async Task<UserInfo> UserProfile(string token)
        {
            var userInfo = _keycloakService.GetUserInfo(token);
            if (userInfo == null)
                throw new UnauthorizedAccessException("No User Found");

            return userInfo;
        }
    }
}