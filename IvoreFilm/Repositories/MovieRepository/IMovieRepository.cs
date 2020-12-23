using System.Collections.Generic;
using System.Threading.Tasks;
using IvoreFilm.Models;
using IvoreFilm.Models.DbModels;
using IvoreFilm.Models.ViewModel;

namespace IvoreFilm.Repositories.MovieRepository
{
    public interface IMovieRepository
    {
        public Task<ResponseModel> AddMovie(MovieViewModel movies);

        public List<Movie> GetMovies();

        public List<Movie> GetMoviesByCat(string cat);

        public SortedSet<string> GetMovieCategories();

        public Movie GetMovieByName(string movieName);

        public string GetPath(int movieId);

        public string GetThumbnail(int movieId);
    }
}