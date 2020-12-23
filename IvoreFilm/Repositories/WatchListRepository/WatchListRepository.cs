using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IvoreFilm.Models;
using IvoreFilm.Models.DbModels;
using IvoreFilm.Models.DTO;
using IvoreFilm.Models.Service;
using IvoreFilm.Models.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace IvoreFilm.Repositories.WatchListRepository
{
    public class WatchListRepository : IWatchListRepository
    {
        private readonly IvoreFilmContext _context;
        private readonly IIdentityService _keycloaService;
        private readonly WatchListDTO _watchListDto = new WatchListDTO();
        private readonly List<WatchListViewModel> _watchListViewModel = new List<WatchListViewModel>();
        private CancellationToken cancellationToken;

        public WatchListRepository(IvoreFilmContext context, IIdentityService keycloaService)
        {
            _context = context;
            _keycloaService = keycloaService;
        }

        public async Task<ResponseModel> AddMovieToList(string KeycloakId, int movieId)
        {
            var exists =
                await _context.WatchLists.FirstOrDefaultAsync(x => x.UserId == KeycloakId && x.MovieId == movieId,
                    cancellationToken);
            if (exists == null)
            {
                if (_context.AppUsers.FirstOrDefault(x => x.UserId == KeycloakId) != null)
                {
                    await _context.WatchLists.AddAsync(new WatchList
                    {
                        MovieId = movieId,
                        UserId = KeycloakId
                    }, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                    return ResponseModel.ReturnSuccess();
                }

                throw new Exception("User Not Found");
            }

            throw new Exception("Movie Already Added To WatchList");
        }

        public async Task<ResponseModel> AddSerieToList(string KeycloakId, int seriesID)
        {
            var exists =
                await _context.WatchLists.FirstOrDefaultAsync(x => x.UserId == KeycloakId && x.SeriesId == seriesID,
                    cancellationToken);
            if (exists == null)
            {
                if (_context.AppUsers.FirstOrDefault(x => x.UserId == KeycloakId) != null)
                {
                    await _context.WatchLists.AddAsync(new WatchList
                    {
                        SeriesId = seriesID,
                        UserId = KeycloakId
                    }, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                    return ResponseModel.ReturnSuccess();
                }

                throw new Exception("User Not Found");
            }

            throw new Exception("Show Already Added To WatchList");
        }

        public async Task<ResponseModel> RemoveMovieToList(string KeycloakId, int movieId)
        {
            var exists =
                 _context.WatchLists.FirstOrDefault(x => x.UserId == KeycloakId && x.MovieId == movieId);
            if (exists != null)
            {
                _context.Remove(exists);
                _context.SaveChanges();
                return ResponseModel.ReturnSuccess();
         
            }

            throw new Exception("Movie Already Removed From WatchList");
        }

        public async Task<ResponseModel> RemoveSerieToList(string KeycloakId, int seriesID)
        {
            var exists =
                await _context.WatchLists.FirstOrDefaultAsync(x => x.UserId == KeycloakId && x.SeriesId == seriesID,
                    cancellationToken);
            if (exists != null)
            {
                
                    _context.Remove(exists);
                    _context.SaveChanges();
                    return ResponseModel.ReturnSuccess();

            }

            throw new Exception("Show Already Removed From WatchList");
        }

        public async Task<WatchListDTO> GetUsersWatchList(string KeycloakId)
        {
            var watchList = _context.WatchLists.Where(x => x.UserId == KeycloakId).ToList();
            var user = _context.AppUsers.FirstOrDefault(x => x.UserId == KeycloakId);
            if (user == null) throw new Exception("User Not Found");

            _watchListDto.Username = user.FirstName + " " + user.LastName;
            foreach (var watch in watchList)
                if (watch.MovieId != null)
                    _watchListViewModel.Add(new WatchListViewModel
                    {
                        movieName = _context.Movies.FirstOrDefault(x => x.MovieId == watch.MovieId)?.Name
                    });
                else
                    _watchListViewModel.Add(new WatchListViewModel
                    {
                        seriesName = _context.Series.FirstOrDefault(x => x.Id == watch.SeriesId)?.SeriesName
                    });

            _watchListDto.ListViewModels = _watchListViewModel;

            return _watchListDto;
        }
    }
}