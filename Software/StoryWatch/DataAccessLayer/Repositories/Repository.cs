using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
            try
            { 
                return Context.SaveChanges();
                
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }
    }
}
