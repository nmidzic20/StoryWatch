using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class BookListCategoryRepository : Repository<BookListCategory>
    {
        public IQueryable<BookListCategory> GetBookListCategories()
        {
            var query = from l in Entities
                        select l;

            return query;
        }
    }
}
