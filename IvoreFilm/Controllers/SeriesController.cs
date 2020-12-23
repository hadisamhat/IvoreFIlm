using System;
using System.Linq;
using System.Threading.Tasks;
using IvoreFilm.Helpers;
using IvoreFilm.Helpers.Authorization;
using IvoreFilm.Helpers.ImageHelper;
using IvoreFilm.Models.ViewModel;
using IvoreFilm.Repositories.SeriesRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IvoreFilm.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class SeriesController : ControllerBase
    {
        private readonly ISerieRepository _serieRepository;
        private readonly NewtonsoftJsonSerializer _serializer;
        private readonly ILogger<SeriesController> _log;
        private readonly IImageHelper _helper;
        public SeriesController(NewtonsoftJsonSerializer serializer, ISerieRepository serieRepository,ILogger<SeriesController> log,IImageHelper helper)
        {
            _serializer = serializer;
            _serieRepository = serieRepository;
            _log = log;
            _helper = helper;
        }
        

        [CustomAuth("Admin")]
        [Route("AddSeries()")]
        [HttpPost]
        public async Task<ActionResult> AddSeries([FromBody] SeriesViewModel series)
        {
            try
            {
                _log.LogInformation("AddSeries() endpoint called");
                if (series.SeriesThumbnail.ToLower().Contains("http"))
                {
                    var res = _helper.SaveImage(series.SeriesThumbnail, "Serie");
                    series.SeriesThumbnail = res;
                    return Ok(await _serieRepository.AddSeries(series));
                }
                
                
                return Ok(await _serieRepository.AddSeries(series));
     
            }
            catch (Exception e)
            {
                _log.LogError("couldn't Add Series", e, $"Exception({series})", series);
                return StatusCode(500);
            }
        }

        [CustomAuth("Admin")]
        [Route("AddEpisodes()")]
        [HttpPost]
        public async Task<ActionResult> AddEpisodes([FromBody] SeriesListViewModel seriesList)
        {
            
            try
            {
                _log.LogInformation("AddEpisodes() endpoint called");
                return Ok(await _serieRepository.AddSeriesList(seriesList));
            }
            catch (Exception e)
            {
                _log.LogError("couldn't Add Episodes", e, $"Exception({seriesList})", seriesList);
                return StatusCode(500);
            }
        }

        [CustomAuth("User")]
        [Route("GetSeries")]
        [HttpGet]
        public string GetSeries()
        {
            
            try
            {
                _log.LogInformation("GetSeries() endpoint called");
                return _serializer.Serialize(_serieRepository.GetSeries());
            }
            catch (Exception e)
            {
                _log.LogError("couldn't Get Series", e);
                return StatusCode(500).ToString();
            }
        }

        [CustomAuth("User")]
        [Route("GetSeriesByCategory/{category?}")]
        [HttpGet]
        public string GetSeriesByCat(string? category)
        {
            try
            {
                _log.LogInformation("GetSeriesByCat() endpoint called");
                return _serializer.Serialize(_serieRepository.GetMoviesByCat(category));
            }
            catch (Exception e)
            {
                _log.LogError("couldn't Get Category", e, $"Exception({category})", category);
                return StatusCode(500).ToString();
            }
            
        }

        [CustomAuth("User")]
        [Route("GetEpisodes")]
        [HttpGet]
        public string GetEpisodes(int serieId)
        {
            try
            {
                _log.LogInformation($"GetEpisodes(int {serieId}) endpoint called");
                return _serializer.Serialize(_serieRepository.GetEpisodes(serieId));
            }
            catch (Exception e)
            {
                _log.LogError("couldn't Get Episodes Of", e, $"Exception({serieId})", serieId);
                return StatusCode(500).ToString();
            }
            
        }

        [CustomAuth("User")]
        [Route("GetSeriesByname")]
        [HttpGet]
        public string GetSeriesByname(string serieName)
        {
            
            try
            {
                _log.LogInformation($"GetSeriesByname(string {serieName}) endpoint called");
                return _serializer.Serialize(_serieRepository.GetSeriesByname(serieName));
            }
            catch (Exception e)
            {
                _log.LogError("couldn't Get Series By name", e, $"Exception({serieName})", serieName);
                return StatusCode(500).ToString();
            }

        }


        [CustomAuth("User")]
        [Route("GetSeriesCategrories")]
        [HttpGet]
        public string GetSeriesCategrories()
        {
            try
            {
                _log.LogInformation($"GetSeriesCategrories() endpoint called");
                return _serializer.Serialize(_serieRepository.GetSerieCategories());
            }
            catch (Exception e)
            {
                _log.LogError("couldn't Get Series By name", e);
                return StatusCode(500).ToString();
            }
        }

        [Route("StreamSerie")]
        [HttpGet]
        public FileResult StreamSerie(string episodeName)
        {
            
            try
            {
                _log.LogInformation($"StreamSerie(string {episodeName}) endpoint called");
                var path = _serieRepository.GetPath(episodeName);
                return PhysicalFile(path, "application/octet-stream", true);
            }
            catch (Exception e)
            {
                _log.LogError("couldn't Stream Series", e);
                return null;
            }
        }

        [Route("GetSeriesThumbnail")]
        [HttpGet]
        public FileResult GetSeriesThumbnail(int seriesId)
        {
            
            try
            {
                _log.LogInformation($"GetSeriesThumbnail(int {seriesId}) endpoint called");
                var path = _serieRepository.GetThumbnail(seriesId);
                var b = System.IO.File.ReadAllBytes(path); // You can use your own method over here.         
                return File(b, "image/jpeg");
            }
            catch (Exception e)
            {
                _log.LogError("couldn't Get Thumble", e);
                return null;
            }
        }
    }
}