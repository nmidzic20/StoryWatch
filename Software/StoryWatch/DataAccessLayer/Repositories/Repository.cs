using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    /// <summary>
    /// Autor: Hrvoje Lukšić
    /// Namjena: roditeljska klasa za komunikaciju s bazom podataka
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Repository<T> : IDisposable where T : class
    {
        protected StoryWatchModel Context { get; set; }
        protected DbSet<T> Entities { get; set; }

        public Repository()
        {
            Context = new StoryWatchModel();
            Entities = Context.Set<T>();
        }

        public virtual IQueryable<T> GetAll()
        {
            return from e in Entities select e;
        }

        public virtual int Add(T entity, bool saveChanges = true)
        {
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


        public virtual int Delete(T entity, bool saveChanges = true)
        {
            Entities.Attach(entity);
            Entities.Remove(entity);
            if (saveChanges)
            {
                return SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public void Dispose()
        {
            Context.Dispose();
        }

        public virtual int SaveChanges()
        {
            return Context.SaveChanges();
        }
    }
}
