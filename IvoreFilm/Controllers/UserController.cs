using System;
using System.Threading.Tasks;
using IvoreFilm.Helpers;
using IvoreFilm.Helpers.KeycloakHelpers;
using IvoreFilm.Models.ViewModel;
using IvoreFilm.Repositories.UserRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IvoreFilm.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private NewtonsoftJsonSerializer _serializer = new NewtonsoftJsonSerializer();
        private readonly ILogger<UserController> _log;
        
        public UserController(IUserRepository userRepository,ILogger<UserController> log)
        {
            _userRepository = userRepository;
            _log = log;
        }

        [AllowAnonymous]
        [Route("Register()")]
        [HttpPost]
        public async Task<ActionResult> Register([FromBody] UsersViewModel usersViewModel)
        {
            
            try
            {
                _log.LogInformation($"Register(UsersViewModel {usersViewModel}) endpoint called");
                return Ok(await _userRepository.AddUser(usersViewModel));
            }
            catch (Exception e)
            {
                
                _log.LogError("couldn't Add User", e, $"Exception({usersViewModel})", usersViewModel);
                return StatusCode(500);
            }
        }
        
        [AllowAnonymous]
        [Route("Login()")]
        [HttpPost]
        public async Task<TokenResponse> Add([FromBody] TokenRequest tokenRequest)
        {
            
            try
            {
                _log.LogInformation($"Add(TokenRequest {tokenRequest.Username}) endpoint called");
                return await _userRepository.Login(tokenRequest);
            }
            catch (Exception e)
            {
                _log.LogError("couldn't Login  User", e, $"Exception({tokenRequest.Username})", tokenRequest.Username);
                return null;
            }
        }
        
        [AllowAnonymous]
        [Route("Logout()")]
        [HttpPost]
        public async Task<ActionResult> Logout([FromHeader] LogoutRequest logout)
        {
            
            try
            {
                _log.LogInformation($"Logout(LogoutRequest {logout.Token}) endpoint called");
                return Ok(await _userRepository.Logout(logout));
            }
            catch (Exception e)
            {
                _log.LogError("couldn't Logout  User", e);
                return null;
            }
        }

        [AllowAnonymous]
        [Route("UserProfile")]
        [HttpGet]
        public async Task<ActionResult> UserProfile(string token)
        {
           
            try
            {
                _log.LogInformation($"UserProfile(UserProfile {token}) endpoint called");
                return Ok(await _userRepository.UserProfile(token));
            }
            catch (Exception e)
            {
                _log.LogError("couldn't Logout  User", e);
                return null;
            }
        }
    }
}