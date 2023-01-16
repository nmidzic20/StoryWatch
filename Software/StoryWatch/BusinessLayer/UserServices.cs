using DataAccessLayer;
using DataAccessLayer.Repositories;
using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
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
                return repo.GetSpecific(username) as User;
            }
        }

        public int Add(User user)
        {
            if (KorisnikPostoji(user.Username))
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

        private bool KorisnikPostoji(string username)
        {
            using (var repo = new UserRepository())
            {
                var korisnik = repo.GetSpecific(username);
                
                if(korisnik.Any())
                {
                    return true;
                }
                return false;
            }
        }
    }
}
