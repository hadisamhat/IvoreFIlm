using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IvoreFilm.Models;
using IvoreFilm.Models.DbModels;
using IvoreFilm.Models.ViewModel;

namespace IvoreFilm.Repositories.SeriesRepository
{
    public class SeriesRepository : ISerieRepository
    {
        private readonly IvoreFilmContext _context;
        private CancellationToken cancellationToken;

        public SeriesRepository(IvoreFilmContext context)
        {
            _context = context;
        }

        public List<Series> GetSeries()
        {
            return _context.Series.OrderBy(x=>x.Category).ToList();
        }

        public List<Series> GetMoviesByCat(string cat)
        {
            return _context.Series.Where(x => x.Category == cat).ToList();
        }

        public List<SeriesList> GetEpisodes(int seriesId)
        {
            var seriesList = _context.SeriesLists.Where(x=>x.SeriesId == seriesId).OrderBy(x=>x.Id)
                .ToList();


            if (seriesList == null) throw new Exception("No Series Found");

            return seriesList;
        }

        public async Task<ResponseModel> AddSeries(SeriesViewModel series)
        {
            await _context.Series.AddAsync(new Series
            {
                SeriesName = series.SeriesName.Trim().ToLower(),
                ReleaseDate = series.ReleaseDate,
                SeriesDescription = series.SeriesDescription,
                NumberEpisodes = series.NumberEpisodes,
                Category = series.Category,
                SeriesThumbnail = series.SeriesThumbnail
            }, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
            return ResponseModel.ReturnSuccess();
        }

        public async Task<ResponseModel> AddSeriesList(SeriesListViewModel series)
        {
            var serie = _context.Series.FirstOrDefault(x => x.SeriesName == series.SeriesName);
            if (serie != null)
            {
                await _context.SeriesLists.AddAsync(new SeriesList
                {
                    SeriesId = serie.Id,
                    EpisodeLength = series.EpisodeLength,
                    EpisodeName = series.EpisodeName,
                    EpisodePath = series.EpisodePath
                }, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return ResponseModel.ReturnSuccess();
            }

            throw new Exception("No Series Found");
        }

        public SortedSet<string> GetSerieCategories()
        {
            var cat = new SortedSet<string>();
            var series = _context.Series.ToList();
            foreach (var serie in series) cat.Add(serie.Category);

            return cat;
        }

        public Series GetSeriesByname(string serieName)
        {
            var series = _context.Series.FirstOrDefault(x => x.SeriesName == serieName);

            if (series == null) throw new Exception("No Series Found");

            return series;
        }

        public string GetPath(string episodeName)
        {
            var series = _context.SeriesLists.FirstOrDefault(x => x.EpisodeName == episodeName);
            if (series == null) throw new Exception("No Series Found");

            return series.EpisodePath;
        }

        public string GetThumbnail(int SeriesId)
        {
            var series = _context.Series.FirstOrDefault(x => x.Id == SeriesId);
            if (series == null) throw new Exception("No Thumbnail Found");

            return series.SeriesThumbnail;
        }
    }
}