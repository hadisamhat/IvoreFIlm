using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IvoreFilm.Models;
using IvoreFilm.Models.DbModels;
using IvoreFilm.Models.ViewModel;
using Microsoft.Extensions.Logging;

namespace IvoreFilm.Repositories.MovieRepository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly IvoreFilmContext _context;
        public MovieRepository(IvoreFilmContext context)
        {
            _context = context;
        }

        private CancellationToken cancellationToken { get; set; }

        public async Task<ResponseModel> AddMovie(MovieViewModel movies)
        {
            await _context.Movies.AddAsync(new Movie
            {
                Name = movies.Name.Trim().ToLower(),
                Path = movies.Path,
                Category = movies.Category,
                Description = movies.Description,
                Length = movies.Length,
                Thumbnail = movies.Thumbnail
            }, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return ResponseModel.ReturnSuccess();
        }

        public List<Movie> GetMovies()
        {
            return _context.Movies.OrderBy(c=>c.Category).ToList();
        }

        public List<Movie> GetMoviesByCat(string cat)
        {
            return _context.Movies.Where(x => x.Category == cat).OrderBy(x=>x.Category).ToList();
        }

        public SortedSet<string> GetMovieCategories()
        {
            var cat = new SortedSet<string>();
            var movies = _context.Movies.ToList();
            foreach (var movie in movies) cat.Add(movie.Category);
            
            return cat;
        }

        public Movie GetMovieByName(string movieName)
        {
            var movie = _context.Movies.FirstOrDefault(x => x.Name == movieName);

            if (movie == null) throw new Exception("No Movie Found");

            return movie;
        }

        public string GetPath(int movieId)
        {
            var movie = _context.Movies.FirstOrDefault(x => x.MovieId == movieId);

            if (movie == null) throw new Exception("No Movie Found");

            return movie.Path;
        }

        public string GetThumbnail(int movieId)
        {
            var movie = _context.Movies.FirstOrDefault(x => x.MovieId == movieId);
            if (movie == null) throw new Exception("No Thumbnail Found");

            return movie.Thumbnail;
        }
    }
}