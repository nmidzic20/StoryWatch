using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class GenreRepository : Repository<Genre>
    {
        public int Update(Genre entity, bool saveChanges = true)
        {
            var genre = Entities.SingleOrDefault(e => e.Id == entity.Id);
            genre.Name = entity.Name;

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
