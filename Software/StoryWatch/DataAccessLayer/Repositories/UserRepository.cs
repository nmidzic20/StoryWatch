using EntitiesLayer;
using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    /// <summary>
    /// Autor: Hrvoje Lukšić
    /// Namjena: repozitorij za korisnike
    /// </summary>
    public class UserRepository : Repository<User>
    {
        public UserRepository() : base() { }

        public IQueryable<User> GetSpecific(string username)
        {
            return from e in Entities.Include("GameListCategories.GameListItems") 
                   where e.Username == username 
                   select e;
        }

        public int Update(User entity, IListCategory newListCategory, MediaCategory mediaCategory, bool saveChanges = true)
        {
            var user = Entities.SingleOrDefault(p => p.Username == entity.Username);

            switch (mediaCategory)
            {
                case MediaCategory.Movie:
                    Context.MovieListCategories.Attach(newListCategory as MovieListCategory);
                    user.MovieListCategories.Add(newListCategory as MovieListCategory);
                    break;

                case MediaCategory.Book:
                    Context.BookListCategories.Attach(newListCategory as BookListCategory);
                    user.BookListCategories.Add(newListCategory as BookListCategory);
                    break;

                case MediaCategory.Game:
                    Context.GameListCategories.Attach(newListCategory as GameListCategory);
                    user.GameListCategories.Add(newListCategory as GameListCategory);
                    break;

            }


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
