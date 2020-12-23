using System.Collections.Generic;
using IvoreFilm.Models.ViewModel;

namespace IvoreFilm.Repositories.SearchRepository
{
    public interface ISearchRepository
    {
        public dynamic Search(string name);
        
        public List<string> GetName();
    }
}