using System;
using System.Threading.Tasks;
using IvoreFilm.Helpers;
using IvoreFilm.Helpers.Authorization;
using IvoreFilm.Helpers.TokenHelper;
using IvoreFilm.Models.ViewModel;
using IvoreFilm.Repositories.WatchListRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IvoreFilm.Controllers
{
    [CustomAuth("User")]
    [Route("api/[controller]")]
    [ApiController]
    public class WatchListController : ControllerBase
    {
        private readonly NewtonsoftJsonSerializer _serializer;
        private readonly IWatchListRepository _watchList;
        private readonly DecodeJwtToken _tokenHelper = new DecodeJwtToken();
        private readonly ILogger<WatchListController> _log;
        
        public WatchListController(IWatchListRepository watchList, NewtonsoftJsonSerializer serializer,ILogger<WatchListController> log)
        {
            _watchList = watchList;
            _serializer = serializer;
            _log = log;
        }

        [Route("AddMovieToList()")]
        [HttpPost]
        public async Task<ActionResult> AddMovieToList([FromBody] MovieWatchList movie)
        {
            
            try
            {
                
                var keycloakId = _tokenHelper.ValidateToken(movie.token);
                _log.LogInformation($"AddMovieToList(MovieWatchList {movie.movieId}) endpoint called");
                return Ok(await _watchList.AddMovieToList(keycloakId, movie.movieId));
                
            }
            catch (Exception e)
            {
                _log.LogError("couldn't Add Movie To user ", e, $"Exception({movie.movieId})", movie.movieId);
                return StatusCode(500);
            }
        }

        [Route("AddSerieToList()")]
        [HttpPost]
        public async Task<ActionResult> AddSerieToList([FromBody] ShowWatchList show)
        {
            
            
            try
            {
                
                var keycloakId = _tokenHelper.ValidateToken(show.token);
                _log.LogInformation($"AddSerieToList(ShowWatchList {keycloakId}) endpoint called");
                return Ok(await _watchList.AddSerieToList(keycloakId, show.seriesId));
                
            }
            catch (Exception e)
            {
                _log.LogError("couldn't Add Series To user ", e, $"Exception({show.seriesId})", show.seriesId);
                return StatusCode(500);
            }
        }
        
        [Route("RemoveMovieFromList()")]
        [HttpPost]
        public async Task<ActionResult> RemoveMovieFromList([FromBody] MovieWatchList movie)
        {
            
            try
            {
                
                var keycloakId = _tokenHelper.ValidateToken(movie.token);
                _log.LogInformation($"RemoveMovieFromList(MovieWatchList {keycloakId}) endpoint called");
                return Ok(await _watchList.RemoveMovieToList(keycloakId, movie.movieId));
                
            }
            catch (Exception e)
            {
                _log.LogError("couldn't Remove Movie From  List ", e, $"Exception({movie.movieId})", movie.movieId);
                return StatusCode(500);
            }
        }

        [Route("RemoveSerieFromList()")]
        [HttpPost]
        public async Task<ActionResult> RemoveSerieFromList([FromBody] ShowWatchList show)
        {
            try
            {

                var keycloakId = _tokenHelper.ValidateToken(show.token);
                _log.LogInformation($"RemoveSerieFromList(MovieWatchList {show.token}) endpoint called");
                return Ok(await _watchList.RemoveSerieToList(keycloakId, show.seriesId));

            }
            catch (Exception e)
            {
                _log.LogError("couldn't Remove Series From  List ", e, $"Exception({show.seriesId})",show.seriesId);
                return StatusCode(500);
            }
        }

        [Route("GetList")]
        [HttpGet]
        public async Task<string> GetList(string token)
        {
            
            try
            {

                var keycloakId = _tokenHelper.ValidateToken(token);
                _log.LogInformation($"GetList(string {keycloakId}) endpoint called");
                return _serializer.Serialize(_watchList.GetUsersWatchList(keycloakId));

            }
            catch (Exception e)
            {
                _log.LogError("couldn't Remove Series From  List ", e);
                return StatusCode(500).ToString();
            }
        }
    }
}