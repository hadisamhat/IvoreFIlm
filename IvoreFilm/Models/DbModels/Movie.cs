using System.Collections.Generic;

namespace IvoreFilm.Models.DbModels
{
    public class Movie
    {
        public Movie()
        {
            WatchLists = new HashSet<WatchList>();
        }

        public int MovieId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Length { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public string Thumbnail { get; set; }

        public virtual ICollection<WatchList> WatchLists { get; set; }
    }
}