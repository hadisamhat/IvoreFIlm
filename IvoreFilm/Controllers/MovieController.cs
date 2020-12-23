using System;
using System.Linq;
using System.Threading.Tasks;
using IvoreFilm.Helpers;
using IvoreFilm.Helpers.Authorization;
using IvoreFilm.Helpers.ImageHelper;
using IvoreFilm.Models.ViewModel;
using IvoreFilm.Repositories.MovieRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IvoreFilm.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;
        private readonly NewtonsoftJsonSerializer _serializer;
        private readonly ILogger<MovieController> _log;
        private readonly IImageHelper _helper;
        public MovieController(IMovieRepository movieRepository, NewtonsoftJsonSerializer serializer,ILogger<MovieController> log, IImageHelper helper)
        {
            _movieRepository = movieRepository;
            _serializer = serializer;
            _log = log;
            _helper = helper;
        }
        
        [CustomAuth("Admin")]
        [Route("AddMovie()")]
        [HttpPost]
        public async Task<ActionResult> AddMovie([FromBody] MovieViewModel movie)
        {
            try
            {
               
                if (movie.Thumbnail.ToLower().Contains("http"))
                {
                    _log.LogInformation("AddMovie() endpoint called");
                    var res = _helper.SaveImage(movie.Thumbnail, "Movie");
                    movie.Thumbnail = res;
                    return Ok(await _movieRepository.AddMovie(movie));
                }
                
                
                return Ok(await _movieRepository.AddMovie(movie));
                
               
            }
            catch (Exception e)
            {
                _log.LogError("couldn't Add Movie", e, $"Exception({movie})", movie);
                return StatusCode(500);
            }
            
        }

        [CustomAuth("User")]
        [Route("GetMovies")]
        [HttpGet]
        public async Task<ActionResult> GetMovies()
        {
            
            var res = _movieRepository.GetMovies();
            bool isEmpty = !res.Any();
            if (!isEmpty)
            {
                _log.LogInformation("GetMoviesByCategory/{category?} endpoint called");
                return Ok(_serializer.Serialize(res));
            }
           
            _log.LogError("Couldn't Fetch Movie");
            return StatusCode(500);
        }
        
        [CustomAuth("User")]
        [Route("GetMoviesByCategory/{category?}")]
        [HttpGet]
        public async Task<ActionResult>  GetMoviesByCat(string? category)
        {
            var res = _movieRepository.GetMoviesByCat(category);
            bool isEmpty = !res.Any();
            if (!isEmpty)
            {
                _log.LogInformation("GetMoviesByCategory/{category?} endpoint called");
                return Ok(_serializer.Serialize(res));
            }
           
            _log.LogError("couldn't Get Movies By Category", $"Category ({category}) Not Found",category);
            return StatusCode(500);
            
            
        }
        
        [CustomAuth("User")]
        [Route("GetMovieByName")]
        [HttpGet]
        public async Task<ActionResult>  GetMovieByName(string movieName)
        {    
            
            var res = _movieRepository.GetMovieByName(movieName);
            if (res != null)
            {
                _log.LogInformation("GetMovieByName/ endpoint called");
                return Ok(_serializer.Serialize(res));
            }
           
            _log.LogError("couldn't Get Movies By Name", _serializer.Serialize(res));
            return StatusCode(500);
        }

        [CustomAuth("User")]
        [Route("GetMoviesCategrories")]
        [HttpGet]
        public async Task<ActionResult>  GetMoviesCategrories()
        {
            var res = _movieRepository.GetMovieCategories();
            bool isEmpty = !res.Any();
            if (!isEmpty)
            {
                _log.LogInformation("GetMoviesCategrories/ endpoint called");
                return Ok(_serializer.Serialize(res));
            }
           
            _log.LogError("couldn't Get Movies Categpry");
            return StatusCode(500);
        }

        [AllowAnonymous]
        [Route("StreamMovie")]
        [HttpGet]
        public FileResult StreamMovie(int movieId)
        {
            
            try
            {
                _log.LogInformation("StreamMovie/ endpoint called");
                var path = _movieRepository.GetPath(movieId);
                return PhysicalFile(path, "application/octet-stream", true);
            }
            catch (Exception e)
            {
                _log.LogError("couldn't Get Stream Movie", e,$"Exception({movieId})",movieId);
                return null;
            }
            
            
        }

        [AllowAnonymous]
        [Route("GetThumbnail")]
        [HttpGet]
        public FileResult GetThumbnail(int movieId)
        {
            
            try
            {
                _log.LogInformation("GetThumbnailMovie/ endpoint called");
                var path = _movieRepository.GetThumbnail(movieId);
                var b = System.IO.File.ReadAllBytes(path);       
                return File(b, "image/jpeg");
            }
            catch (Exception e)
            {
                _log.LogError("couldn't Get Movie Thumbnail", e,$"Exception({movieId})",movieId);
                return null;
            }
        }
    }
}