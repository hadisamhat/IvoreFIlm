using System.Threading.Tasks;
using IvoreFilm.Models;
using IvoreFilm.Models.DTO;

namespace IvoreFilm.Repositories.WatchListRepository
{
    public interface IWatchListRepository
    {
        public Task<ResponseModel> AddMovieToList(string KeycloakId, int movieId);

        public Task<ResponseModel> AddSerieToList(string KeycloakId, int seriesID);
        
        public Task<ResponseModel> RemoveMovieToList(string KeycloakId, int movieId);

        public Task<ResponseModel> RemoveSerieToList(string KeycloakId, int seriesID);

        public Task<WatchListDTO> GetUsersWatchList(string KeycloakId);
    }
}