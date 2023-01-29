using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    /// <summary>
    /// Function: database communication for genre management
    /// Author: Noa Midžić
    /// </summary>
    public class GenreRepository : Repository<Genre>
    {
        public IQueryable<Genre> GetAllGenresForUser(User loggedUser)
        {
            var userMovies = Context.MovieListItems.Where(ml => ml.Id_Users == loggedUser.Id);
            return Context.Movies.Where(m => userMovies.Any(um => um.Id_Movies == m.Id)).Select(m => m.Genre);
        }

        public IQueryable<Genre> GetAllGameGenresForUser(User loggedUser)
        {
            var userGames = Context.GameListItems.Where(gl => gl.Id_Users == loggedUser.Id);
            return Context.Games.Where(g => userGames.Any(ug => ug.Id_Games == g.Id)).Select(m => m.Genre);
        }

        public Genre Update(Genre oldGenre, Genre newGenre, bool saveChanges = true)
        {
            //check if any other media, beside the media which is being updated,
            //still references the old genre - if not, delete that genre
            var countBooks = Context.Books.Count(b => b.Genre != null && b.Genre.Id == oldGenre.Id);
            var countMovies = Context.Movies.Count(m => m.Genre != null && m.Genre.Id == oldGenre.Id);
            var countGames = Context.Games.Count(g => g.Genre != null && g.Genre.Id == oldGenre.Id);

            if (countBooks + countMovies + countGames <= 1)
            {
                Entities.Attach(oldGenre);
                Entities.Remove(oldGenre);
                SaveChanges();
            }

            //find if a genre with the new name already exists
            //if yes, return that genre
            //if not, add new genre and return the added one
            var genre = Entities.SingleOrDefault(e => e.Name == newGenre.Name);

            if (genre != null)
            {
                return genre;
            }
            else
            {
                Add(newGenre);
                SaveChanges();
                return newGenre;
            }
        }

        public Genre UpdateGameGenre(Genre oldGenre, Genre newGenre, bool saveChanges = true)
        {
            //check if any other book, beside the movie which is being updated,
            //still references the old genre - if not, delete that genre
            if (Context.Games.Count(m => m.Genre != null && m.Genre.Id == oldGenre.Id) <= 1)
            {
                Entities.Attach(oldGenre);
                Entities.Remove(oldGenre);
                SaveChanges();
            }

            //find if a genre with the new name already exists
            //if yes, return that genre
            //if not, add new genre and return the added one
            var genre = Entities.SingleOrDefault(e => e.Name == newGenre.Name);

            if (genre != null)
            {
                return genre;
            }
            else
            {
                Add(newGenre);
                SaveChanges();
                return newGenre;
            }
        }

        public void DeleteGenreWithoutMedia(Genre genreToDelete)
        {
            var countBooks = Context.Books.Count(b => b.Genre != null && b.Genre.Id == genreToDelete.Id);
            var countMovies = Context.Movies.Count(b => b.Genre != null && b.Genre.Id == genreToDelete.Id);
            var countGames = Context.Games.Count(b => b.Genre != null && b.Genre.Id == genreToDelete.Id);

            if (countBooks + countMovies + countGames <= 1)
            {
                Entities.Attach(genreToDelete);
                Entities.Remove(genreToDelete);
                SaveChanges();
            }
        }
    }
}
