using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class MovieRepository : Repository<Movie>
    {
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

        public IQueryable<Movie> GetMovieByTitle(string title)
        {
            var query = from m in Entities
                        where m.Title == title
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
    }
}
