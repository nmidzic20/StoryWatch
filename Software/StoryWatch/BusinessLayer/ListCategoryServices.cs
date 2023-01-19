using DataAccessLayer.Repositories;
using EntitiesLayer;
using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class ListCategoryServices
    {
        public List<IListCategory> GetListCategories(MediaCategory mediaCategory, User loggedUser)
        {
            List<IListCategory> mediaListCategories = new List<IListCategory>();

            switch (mediaCategory)
            {
                case MediaCategory.Movie:
                    var listMovieCategories = GetMovieListCategoriesForUser(loggedUser);
                    foreach (var lc in listMovieCategories)
                        mediaListCategories.Add(lc);
                    break;

                case MediaCategory.Book:
                    var listBookCategories = GetBookListCategoriesForUser(loggedUser);
                    foreach (var lc in listBookCategories)
                        mediaListCategories.Add(lc);
                    break;

                case MediaCategory.Game:
                    var listGameCategories = GetGameListCategoriesForUser(loggedUser);
                    foreach (var lc in listGameCategories)
                        mediaListCategories.Add(lc);
                    break;
            }

            return mediaListCategories;
        }

        public List<MovieListCategory> GetMovieListCategories()
        {
            using (var repo = new MovieListCategoryRepository())
            {
                return repo.GetAll().ToList();
            }
           
        }

        public List<MovieListCategory> GetMovieListCategoriesForUser(User loggedUser)
        {
            using (var repo = new UserRepository())
            {
                User user = repo.GetSpecific(loggedUser.Username).FirstOrDefault();
                return user.MovieListCategories.ToList();
            }
        }

        public List<BookListCategory> GetBookListCategoriesForUser(User loggedUser)
        {
            using (var repo = new UserRepository())
            {
                User user = repo.GetSpecific(loggedUser.Username).FirstOrDefault();
                return user.BookListCategories.ToList();
            }
        }

        public List<GameListCategory> GetGameListCategoriesForUser(User loggedUser)
        {
            using (var repo = new UserRepository())
            {
                User user = repo.GetSpecific(loggedUser.Username).FirstOrDefault();
                return user.GameListCategories.ToList();
            }
        }

        public bool AddMovieListCategory(MovieListCategory movieListCategory, User loggedUser)
        {
            bool isSuccessful = false;
            using (var repo = new MovieListCategoryRepository())
            {
                MovieListCategory mc = repo.GetMovieListCategories().ToList().FirstOrDefault(l => l.Title == movieListCategory.Title);

                if (mc != null)
                {
                    movieListCategory = mc;
                    isSuccessful = true;
                }
                else
                {
                    int affectedRows = repo.Add(movieListCategory);
                    isSuccessful = affectedRows > 0;
                }
                
            }
       
            if (isSuccessful)
            {
                using (var repo = new UserRepository())
                {
                    repo.Update(loggedUser, movieListCategory, MediaCategory.Movie);
                }
            }

            return isSuccessful;
        }

        public bool CreateDefaultLists()
        {
            bool isSuccessful = false;

            return isSuccessful;
        }
    }
    
}
