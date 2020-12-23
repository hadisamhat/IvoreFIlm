using System;
using System.Threading.Tasks;
using IvoreFilm.Helpers;
using IvoreFilm.Helpers.Authorization;
using IvoreFilm.Repositories.SearchRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IvoreFilm.Controllers
{
    [CustomAuth("User")]
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchRepository _searchRepository;
        private readonly NewtonsoftJsonSerializer _serializer;
        private readonly ILogger<SearchController> _log;
        public SearchController(ISearchRepository searchRepository, NewtonsoftJsonSerializer serializer,ILogger<SearchController> log)
        {
            _log = log;
            _searchRepository = searchRepository;
            _serializer = serializer;
        }

        [Route("Search")]
        [HttpGet]
        public async Task<string> GetMovies(string name)
        {
            try
            {
                _log.LogInformation("Search endpoint called");
                return _serializer.Serialize(_searchRepository.Search(name.ToLower()));
            }
            catch (Exception e)
            {
                _log.LogError("couldn't Find Movie/Serie", e, $"Exception({name})", name);
                return StatusCode(500).ToString();
            }
        }
        
        [Route("GetNames")]
        [HttpGet]
        public async Task<string> GetNames()
        {
            try
            {
                _log.LogInformation("GetNames endpoint called");
                return _serializer.Serialize(_searchRepository.GetName());
            }
            catch (Exception e)
            {
                _log.LogError("couldn't Get Name", e);
                return StatusCode(500).ToString();
            }
        }
    }
}