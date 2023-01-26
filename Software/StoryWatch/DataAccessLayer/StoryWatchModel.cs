using EntitiesLayer.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DataAccessLayer
{
    public partial class StoryWatchModel : DbContext
    {
        public StoryWatchModel()
            : base("name=StoryWatchModel")
        {
        }

        public virtual DbSet<BookListCategory> BookListCategories { get; set; }
        public virtual DbSet<BookListItem> BookListItems { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<GameListCategory> GameListCategories { get; set; }
        public virtual DbSet<GameListItem> GameListItems { get; set; }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<MovieListCategory> MovieListCategories { get; set; }
        public virtual DbSet<MovieListItem> MovieListItems { get; set; }
        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookListCategory>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<BookListCategory>()
                .Property(e => e.Color)
                .IsUnicode(false);

            modelBuilder.Entity<BookListCategory>()
                .HasMany(e => e.BookListItems)
                .WithRequired(e => e.BookListCategory)
                .HasForeignKey(e => e.Id_BookListCategories);

            modelBuilder.Entity<BookListCategory>()
                .HasMany(e => e.Books)
                .WithMany(e => e.BookListCategories)
                .Map(m => m.ToTable("BookListCategories_Books").MapLeftKey("Id_BookListCategories").MapRightKey("Id_Books"));

            modelBuilder.Entity<BookListCategory>()
                .HasMany(e => e.Users)
                .WithMany(e => e.BookListCategories)
                .Map(m => m.ToTable("Users_BookListCategories").MapLeftKey("Id_BookListCategories").MapRightKey("Id_Users"));

            modelBuilder.Entity<Book>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<Book>()
                .Property(e => e.Author)
                .IsUnicode(false);

            modelBuilder.Entity<Book>()
                .Property(e => e.Summary)
                .IsUnicode(false);

            modelBuilder.Entity<Book>()
                .HasMany(e => e.BookListItems)
                .WithRequired(e => e.Book)
                .HasForeignKey(e => e.Id_Books);

            modelBuilder.Entity<GameListCategory>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<GameListCategory>()
                .Property(e => e.Color)
                .IsUnicode(false);

            modelBuilder.Entity<GameListCategory>()
                .HasMany(e => e.GameListItems)
                .WithRequired(e => e.GameListCategory)
                .HasForeignKey(e => e.Id_GameListCategories);

            modelBuilder.Entity<GameListCategory>()
                .HasMany(e => e.Games)
                .WithMany(e => e.GameListCategories)
                .Map(m => m.ToTable("GameListCategories_Games").MapLeftKey("Id_GameListCategories").MapRightKey("Id_Games"));

            modelBuilder.Entity<GameListCategory>()
                .HasMany(e => e.Users)
                .WithMany(e => e.GameListCategories)
                .Map(m => m.ToTable("Users_GameListCategories").MapLeftKey("Id_GameListCategories").MapRightKey("Id_Users"));

            modelBuilder.Entity<Game>()
                .Property(e => e.Title)
                .IsUnicode(false);
            
            modelBuilder.Entity<Game>()
                .Property(e => e.Summary)
                .IsUnicode(false);

            modelBuilder.Entity<Game>()
                .Property(e => e.IGDB_Id)
                .IsUnicode(false);

            modelBuilder.Entity<Game>()
                .Property(e => e.Company)
                .IsUnicode(false);

            modelBuilder.Entity<Game>()
                .Property(e => e.Genres)
                .IsUnicode(false);

            modelBuilder.Entity<Game>()
                .Property(e => e.Release_Date)
                .IsUnicode(false);

            modelBuilder.Entity<Game>()
                .HasMany(e => e.GameListItems)
                .WithRequired(e => e.Game)
                .HasForeignKey(e => e.Id_Games);

            modelBuilder.Entity<Genre>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<MovieListCategory>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<MovieListCategory>()
                .Property(e => e.Color)
                .IsUnicode(false);

            modelBuilder.Entity<MovieListCategory>()
                .HasMany(e => e.MovieListItems)
                .WithRequired(e => e.MovieListCategory)
                .HasForeignKey(e => e.Id_MovieListCategories);

            modelBuilder.Entity<MovieListCategory>()
                .HasMany(e => e.Movies)
                .WithMany(e => e.MovieListCategories)
                .Map(m => m.ToTable("MovieListCategories_Movies").MapLeftKey("Id_MovieListCategories").MapRightKey("Id_Movies"));

            modelBuilder.Entity<MovieListCategory>()
                .HasMany(e => e.Users)
                .WithMany(e => e.MovieListCategories)
                .Map(m => m.ToTable("Users_MovieListCategories").MapLeftKey("Id_MovieListCategories").MapRightKey("Id_Users"));

            modelBuilder.Entity<Movie>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<Movie>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Movie>()
                .Property(e => e.Trailer_URL)
                .IsUnicode(false);

            modelBuilder.Entity<Movie>()
                .Property(e => e.TMDB_ID)
                .IsUnicode(false);

            modelBuilder.Entity<Movie>()
                .Property(e => e.Countries)
                .IsUnicode(false);

            modelBuilder.Entity<Movie>()
                .Property(e => e.ReleaseDate)
                .IsUnicode(false);

            modelBuilder.Entity<Movie>()
                .HasMany(e => e.MovieListItems)
                .WithRequired(e => e.Movy)
                .HasForeignKey(e => e.Id_Movies);

            modelBuilder.Entity<User>()
                .Property(e => e.Username)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.BookListItems)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.Id_Users);

            modelBuilder.Entity<User>()
                .HasMany(e => e.GameListItems)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.Id_Users);

            modelBuilder.Entity<User>()
                .HasMany(e => e.MovieListItems)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.Id_Users);
        }
    }
}
