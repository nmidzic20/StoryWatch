using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class MovieListCategoryRepository : Repository<MovieListCategory>
    {
        public void DeleteListForUser(MovieListCategory movieListCategory, User loggedUser)
        {
            var movieRepo = new MovieRepository();
            var moviesOnThisList = movieRepo.GetMoviesForList(movieListCategory, loggedUser);
            foreach (var movie in moviesOnThisList)
            {
                //since movies are deleted with the list
                //check if any list for any user, still references these movies
                //if not, no sense in keeping the movie in DB - delete it as well
                if (Context.MovieListItems.Count(m => m.Id_Movies == movie.Id) == 1)
                {
                    var unusedMovie = Context.Movies.SingleOrDefault(m => m.Id == movie.Id);
                    Context.Movies.Remove(unusedMovie);
                    SaveChanges();
                }
            }

            Context.Users.Attach(loggedUser);
            var movieListCategoryToDelete = loggedUser.MovieListCategories
                .FirstOrDefault(ml => ml.Id == movieListCategory.Id);

            loggedUser.MovieListCategories.Remove(movieListCategoryToDelete);
            SaveChanges();
        }

        public IQueryable<MovieListCategory> GetMovieListCategories()
        {
            var query = from l in Entities
                        select l;

            return query;
        }

        


    }
}
