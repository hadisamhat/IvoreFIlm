using System.Collections.Generic;

namespace IvoreFilm.Models.DbModels
{
    public class AppUser
    {
        public AppUser()
        {
            WatchLists = new HashSet<WatchList>();
        }

        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<WatchList> WatchLists { get; set; }
    }
}