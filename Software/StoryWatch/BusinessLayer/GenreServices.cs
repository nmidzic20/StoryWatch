using DataAccessLayer.Repositories;
using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class GenreServices
    {
        public Genre AddGenre(Genre genre)
        {
            bool isSuccessful = false;

            var existingGenre = GetGenreByName(genre.Name);
            if (existingGenre != null)
                return existingGenre;

            using (var repo = new GenreRepository())
            {
                int affectedRows = repo.Add(genre);
                isSuccessful = affectedRows > 0;

            }

            return genre;

        }

        public List<Genre> GetAllGenres()
        {
            using (var repo = new GenreRepository())
            {
                return repo.GetAll().ToList();
            }
        }

        public Genre GetGenreById(int id)
        {
            using (var repo = new GenreRepository())
            {
                return GetAllGenres().FirstOrDefault(g => g.Id == id);
            }
        }

        public Genre GetGenreByName(string name)
        {
            using (var repo = new GenreRepository())
            {
                return GetAllGenres().FirstOrDefault(g => g.Name == name);
            }
        }

        public Genre UpdateGenre(Genre oldGenre, Genre newGenre)
        {
            using (var repo = new GenreRepository())
            {
                return repo.Update(oldGenre, newGenre);
            }
        }

        public Genre UpdateGameGenre(Genre oldGenre, Genre newGenre)
        {
            using (var db = new GenreRepository())
            {
                return db.UpdateGameGenre(oldGenre, newGenre);
            }
        }

        public List<Genre> GetGenresForUser(User loggedUser)
        {
            using (var db = new GenreRepository())
            {
                return db.GetAllGenresForUser(loggedUser).ToList();

            }
        }

        public List<Genre> GetGameGenresForUser(User loggedUser)
        {
            using (var db = new GenreRepository())
            {
                return db.GetAllGameGenresForUser(loggedUser).ToList();

            }
        }
    }
}
