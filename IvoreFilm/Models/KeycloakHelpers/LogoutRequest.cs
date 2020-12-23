namespace IvoreFilm.Helpers.KeycloakHelpers
{
    public class LogoutRequest
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}