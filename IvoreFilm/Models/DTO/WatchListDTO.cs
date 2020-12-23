using System.Collections.Generic;
using IvoreFilm.Models.ViewModel;

namespace IvoreFilm.Models.DTO
{
    public class WatchListDTO
    {
        public string Username { get; set; }
        public List<WatchListViewModel> ListViewModels { get; set; }
    }
}