using System.Collections.Generic;

#nullable disable

namespace IvoreFilm.Models.DbModels
{
    public class Series
    {
        public Series()
        {
            SeriesLists = new HashSet<SeriesList>();
            WatchLists = new HashSet<WatchList>();
        }

        public int Id { get; set; }
        public string SeriesName { get; set; }
        public string SeriesDescription { get; set; }
        public int NumberEpisodes { get; set; }
        public string ReleaseDate { get; set; }
        public string SeriesThumbnail { get; set; }
        public string Category { get; set; }

        public virtual ICollection<SeriesList> SeriesLists { get; set; }
        public virtual ICollection<WatchList> WatchLists { get; set; }
    }
}