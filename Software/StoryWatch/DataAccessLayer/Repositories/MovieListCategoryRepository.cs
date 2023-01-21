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
        public IQueryable<MovieListCategory> GetMovieListCategories()
        {
            var query = from l in Entities
                        select l;

            return query;
        }

        


    }
}
