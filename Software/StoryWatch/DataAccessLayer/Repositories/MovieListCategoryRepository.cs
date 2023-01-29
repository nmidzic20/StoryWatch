using EntitiesLayer.Entities;
using Syncfusion.DocIO.DLS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    /// <summary>
    /// Function: database communication for movie list management
    /// Author: Noa Midžić
    /// </summary>
    public class MovieListCategoryRepository : Repository<MovieListCategory>
    {
        public IQueryable<MovieListCategory> GetMovieListCategories()
        {
            var query = from l in Entities
                        select l;

            return query;
        }

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

        public int UpdateListForUser(MovieListCategory entity, User loggedUser, bool saveChanges = true)
        {
            var movieListCategory = Entities.SingleOrDefault(e => e.Id == entity.Id);
            movieListCategory.Title = entity.Title;
            movieListCategory.Color = entity.Color;

            if (saveChanges)
            {
                return SaveChanges();
            }
            else
            {
                return 0;
            }
        }
    }
}
