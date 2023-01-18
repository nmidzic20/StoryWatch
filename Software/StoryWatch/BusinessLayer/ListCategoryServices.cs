using DataAccessLayer.Repositories;
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
        public List<IListCategory> GetListCategories(MediaCategory mediaCategory)
        {
            List<IListCategory> mediaListCategories = new List<IListCategory>();

            switch (mediaCategory)
            {
                case MediaCategory.Movie:
                    var listMovieCategories = GetMovieListCategories();
                    foreach (var lc in listMovieCategories)
                        mediaListCategories.Add(lc);
                    break;

                case MediaCategory.Book:
                    var listBookCategories = GetBookListCategories();
                    foreach (var lc in listBookCategories)
                        mediaListCategories.Add(lc);
                    break;

                case MediaCategory.Game:
                    var listGameCategories = GetGameListCategories();
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

        public List<BookListCategory> GetBookListCategories()
        {
            //TODO
            return new List<BookListCategory>();
        }

        public List<GameListCategory> GetGameListCategories()
        {
            //TODO
            return new List<GameListCategory>();
        }

        public bool AddMovieListCategory(MovieListCategory movieListCategory)
        {
            bool isSuccessful = false;
            using (var repo = new MovieListCategoryRepository())
            {
                int affectedRows = repo.Add(movieListCategory);
                isSuccessful = affectedRows > 0;
            }

            return isSuccessful;
        }
    }
    
}
