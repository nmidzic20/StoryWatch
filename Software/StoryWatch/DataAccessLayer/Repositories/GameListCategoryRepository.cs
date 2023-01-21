using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class GameListCategoryRepository : Repository<GameListCategory>
    {
        public IQueryable<GameListCategory> GetGameListCategories()
        {
            var query = from l in Entities
                        select l;

            return query;
        }
    }
}
