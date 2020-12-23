using System.Collections.Generic;
using System.Threading.Tasks;
using IvoreFilm.Models;
using IvoreFilm.Models.DbModels;
using IvoreFilm.Models.ViewModel;

namespace IvoreFilm.Repositories.SeriesRepository
{
    public interface ISerieRepository
    {
        public List<Series> GetSeries();

        public List<Series> GetMoviesByCat(string cat);

        public List<SeriesList> GetEpisodes(int seriesName);

        public Task<ResponseModel> AddSeries(SeriesViewModel series);

        public Task<ResponseModel> AddSeriesList(SeriesListViewModel series);

        public SortedSet<string> GetSerieCategories();
        public Series GetSeriesByname(string serieName);

        public string GetPath(string episodeName);

        public string GetThumbnail(int SeriesId);
    }
}