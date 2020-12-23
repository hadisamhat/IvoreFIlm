namespace IvoreFilm.Models.DbModels
{
    public class WatchList
    {
        public string UserId { get; set; }
        public int? MovieId { get; set; }
        public int? SeriesId { get; set; }
        public int Id { get; set; }

        public virtual Movie Movie { get; set; }
        public virtual Series Series { get; set; }
        public virtual AppUser User { get; set; }
    }
}