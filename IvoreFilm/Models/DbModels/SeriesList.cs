namespace IvoreFilm.Models.DbModels
{
    public class SeriesList
    {
        public int? SeriesId { get; set; }
        public string EpisodeName { get; set; }
        public string EpisodeLength { get; set; }
        public string EpisodePath { get; set; }
        public int Id { get; set; }

        public virtual Series Series { get; set; }
    }
}