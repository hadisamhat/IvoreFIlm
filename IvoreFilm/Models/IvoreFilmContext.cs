using IvoreFilm.Models.DbModels;
using Microsoft.EntityFrameworkCore;


namespace IvoreFilm
{
    public partial class IvoreFilmContext : DbContext
    {
        public IvoreFilmContext()
        {
        }

        public IvoreFilmContext(DbContextOptions<IvoreFilmContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AppUser> AppUsers { get; set; }
        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<Series> Series { get; set; }
        public virtual DbSet<SeriesList> SeriesLists { get; set; }
        public virtual DbSet<WatchList> WatchLists { get; set; }

 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("appusers_pk");

                entity.Property(e => e.FirstName).IsRequired();

                entity.Property(e => e.LastName).IsRequired();
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.HasIndex(e => e.MovieId, "movies_movieid_uindex")
                    .IsUnique();

                entity.Property(e => e.Category).IsRequired();

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Length).IsRequired();

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.Path).IsRequired();

                entity.Property(e => e.Thumbnail).IsRequired();
            });

            modelBuilder.Entity<Series>(entity =>
            {
                entity.Property(e => e.ReleaseDate).IsRequired();

                entity.Property(e => e.SeriesDescription).IsRequired();

                entity.Property(e => e.SeriesName).IsRequired();

                entity.Property(e => e.SeriesThumbnail).IsRequired();
            });

            modelBuilder.Entity<SeriesList>(entity =>
            {
                entity.ToTable("SeriesList");

                entity.HasOne(d => d.Series)
                    .WithMany(p => p.SeriesLists)
                    .HasForeignKey(d => d.SeriesId)
                    .HasConstraintName("SeriesId");
            });

            modelBuilder.Entity<WatchList>(entity =>
            {
                entity.ToTable("WatchList");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.WatchLists)
                    .HasForeignKey(d => d.MovieId)
                    .HasConstraintName("MovieId");

                entity.HasOne(d => d.Series)
                    .WithMany(p => p.WatchLists)
                    .HasForeignKey(d => d.SeriesId)
                    .HasConstraintName("SeriesId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.WatchLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("UserId");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
