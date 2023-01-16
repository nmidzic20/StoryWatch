using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        }

        public virtual IQueryable<T> GetAll()
        {
            return from e in Entities select e;
        }

        public virtual int Add(T entity)
        {
            Entities.Add(entity);
            return Context.SaveChanges();
        }

        public virtual int Delete(T entity)
        {
            Entities.Remove(entity);
            return Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
