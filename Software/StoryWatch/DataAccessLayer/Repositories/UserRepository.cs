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
            return from e in Entities 
                   where e.Username == username 
                   select e;
        }
    }
}
