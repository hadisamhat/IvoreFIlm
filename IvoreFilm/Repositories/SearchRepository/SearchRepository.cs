using System;
using System.Collections.Generic;
using System.Linq;
using IvoreFilm.Models;
using IvoreFilm.Models.ViewModel;

namespace IvoreFilm.Repositories.SearchRepository
{
    public class SearchRepository : ISearchRepository
    {
        private readonly IvoreFilmContext _context;

        public SearchRepository(IvoreFilmContext context)
        {
            _context = context;
        }

        public dynamic Search(string name)
        {
            var movies = _context.Movies.FirstOrDefault(x => x.Name == name);
            var series = _context.Series.FirstOrDefault(x => x.SeriesName == name);
            if (movies != null) return movies;
            if (series != null) return series;

            throw new Exception("No Result");
        }

        public List<string> GetName()
        {
            var movies = _context.Movies.Select(x=>x.Name).ToList();
            var series = _context.Series.Select(x => x.SeriesName).ToList();
            
            return movies.Concat(series).ToList();
        }
    }
}