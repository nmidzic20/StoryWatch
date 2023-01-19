using DataAccessLayer;
using DataAccessLayer.Repositories;
using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    /// <summary>
    /// Autor: Hrvoje Lukšić
    /// Namjena: rad s korisnicima, autentifikacija pri logiranju i registraciji
    /// </summary>
    public class UserServices
    {
        public List<User> GetAll()
        {
            using (var repo = new UserRepository())
            {
                return repo.GetAll().ToList();
            }
        }

        public bool Login(User loginUser)
        {
            var foundUser = GetSpecific(loginUser.Username);

            if (foundUser == null || loginUser.Password != foundUser.Password)
            {
                return false;
            }

            return true;
        }

        public User GetSpecific(string username)
        {
            using (var repo = new UserRepository())
            {
                var result = repo.GetSpecific(username);

                if (!result.Any())
                {
                    return null;
                }
                return result.ToList().First();
            }
        }

        public int Add(User user)
        {
            // provjerava postoji li već korisnik koji se registrira
            if (GetSpecific(user.Username) != null)
            {
                return -1;
            }

            using (var repo = new UserRepository())
            {
                return repo.Add(user);
            }
        }

        

        

        public int Delete(User user)
        {
            using (var repo = new UserRepository())
            {
                return repo.Delete(user);
            }
        }
    }
}
