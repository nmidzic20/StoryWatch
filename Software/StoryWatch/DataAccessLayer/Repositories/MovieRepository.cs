using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class MovieRepository : Repository<Movie>
    {
        public override IQueryable<Movie> GetAll()
        {
            return Entities.Include(e => e.Genre);
        }

        public override int Add(Movie entity, bool saveChanges = true)
        {
            var genre = Context.Genres.SingleOrDefault(g => g.Id == entity.Genre.Id);

            entity.Genre = genre;

            Entities.Add(entity);

            if (saveChanges)
            {
                return SaveChanges();
            }
            else
            {
                return 0;
            }
        
        }

        public int AddMovieToList(MovieListItem movieListItem, bool saveChanges = true)
        {
            Context.MovieListItems.Add(movieListItem);
            if (saveChanges)
            {
                return Context.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public int DeleteMovieFromList(MovieListItem movieListItem, bool saveChanges = true)
        {
            int isSuccessful;

            Context.MovieListItems.Attach(movieListItem);
            Context.MovieListItems.Remove(movieListItem);

            if (saveChanges)
            {
                isSuccessful = Context.SaveChanges();
            }
            else
            {
                isSuccessful = 0;
            }

            //check if any list for any user, still references this movie
            //if not, no sense in keeping the movie in DB - delete it as well, not just MovieListItem/movie on this specific list
            if (Context.MovieListItems.Count(m => m.Id_Movies == movieListItem.Id_Movies) == 0)
            {
                var unusedMovie = Entities.SingleOrDefault(m => m.Id == movieListItem.Id_Movies);
                Delete(unusedMovie);
            }

            return isSuccessful;
        }

        public IQueryable<Movie> GetMovieByTitle(string title)
        {
            var query = from m in Entities.Include("Genre")
                        where m.Title == title
                        select m;

            return query;
        }

        public IQueryable<Movie> GetMovieByTMDBId(string TMDB_ID)
        {
            var query = from m in Entities.Include("Genre")
                        where m.TMDB_ID == TMDB_ID
                        select m;

            return query;
        }

        public IQueryable<Movie> GetMovieById(int id)
        {
            var query = from m in Entities.Include("Genre")
                        where m.Id == id
                        select m;

            return query;
        }

        public List<Movie> GetMoviesForList(MovieListCategory movieListCategory, User loggedUser)
        {
            //Context.Users.Attach(loggedUser);
            var user = Context.Users.Where(u => u.Id == loggedUser.Id).Single();
            var movie_ids = user.MovieListItems.Where(l => l.Id_MovieListCategories == movieListCategory.Id).Select(l => l.Id_Movies).ToList();

            List<Movie> movies = new List<Movie>();

            foreach (var movie in Entities)
                if (movie_ids.Exists(id => id == movie.Id))
                    movies.Add(movie);

            return movies;
        }

        public int Update(Movie entity, bool saveChanges = true)
        {
            var movie = Entities.SingleOrDefault(e => e.Id == entity.Id);
            movie.Title = entity.Title;
            movie.Description = entity.Description;
            movie.ReleaseDate = entity.ReleaseDate;
            movie.Countries = entity.Countries;
            movie.Trailer_URL = entity.Trailer_URL;
            movie.Genre = Context.Genres.SingleOrDefault(g => g.Id == entity.Genre.Id);

            if (saveChanges)
            {
                return SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public int UpdateMovieListItem(MovieListItem movieListItem, int idNewList, bool saveChanges = true)
        {
            
            MovieListItem movieListItemDB = Context.MovieListItems.Where(ml => 
                                        ml.Id_Movies == movieListItem.Id_Movies && 
                                        ml.Id_Users == movieListItem.Id_Users && 
                                        ml.Id_MovieListCategories == movieListItem.Id_MovieListCategories).SingleOrDefault();

            Context.MovieListItems.Remove(movieListItemDB);
            Context.MovieListItems.Add(new MovieListItem
            {
                Id_Movies = movieListItem.Id_Movies,
                Id_MovieListCategories = idNewList,
                Id_Users = movieListItem.Id_Users
            });

            if (saveChanges)
            {
                return Context.SaveChanges();
            }
            else
            {
                return 0;
            }
        }
    }
}
