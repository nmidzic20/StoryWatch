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
    }
}
