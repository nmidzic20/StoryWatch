using DataAccessLayer.Repositories;
using EntitiesLayer;
using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

        public List<BookListCategory> GetBookListCategories() 
        {
            using (var db = new BookListCategoryRepository())
            {
                return db.GetAll().ToList();
            }
        }

        public List<GameListCategory> GetGameListCategories()
        {
            using (var db = new GameListCategoryRepository())
            {
                return db.GetAll().ToList();
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

        public bool AddBookListCategory(BookListCategory bookListCategory, User loggedUser)
        {
            bool isSuccessful = false;
            using (var db = new BookListCategoryRepository())
            {
                BookListCategory bc = db.GetBookListCategories().ToList().FirstOrDefault(l => l.Title == bookListCategory.Title);

                if (bc != null)
                {
                    bookListCategory = bc;
                    isSuccessful = true;
                }
                else
                {
                    int affectedRows = db.Add(bookListCategory);
                    isSuccessful = affectedRows > 0;
                }

            }

            if (isSuccessful)
            {
                using (var repo = new UserRepository())
                {
                    repo.Update(loggedUser, bookListCategory, MediaCategory.Book);
                }
            }

            return isSuccessful;
        }
        
        public bool AddGameListCategory(GameListCategory gameListCategory, User loggedUser)
        {
            bool isSuccessful = false;
            using (var db = new GameListCategoryRepository())
            {
                GameListCategory gc = db.GetGameListCategories().ToList().FirstOrDefault(l => l.Title == gameListCategory.Title);

                if (gc != null)
                {
                    gameListCategory = gc;
                    isSuccessful = true;
                }
                else
                {
                    int affectedRows = db.Add(gameListCategory);
                    isSuccessful = affectedRows > 0;
                }

            }

            if (isSuccessful)
            {
                using (var repo = new UserRepository())
                {
                    repo.Update(loggedUser, gameListCategory, MediaCategory.Game);
                }
            }

            return isSuccessful;
        }

        public void CreateDefaultLists(MediaCategory mediaCategory, User loggedUser)
        {
            switch (mediaCategory)
            {
                case MediaCategory.Movie:
                {

                    AddMovieListCategory(
                        new MovieListCategory
                        {
                            Id = GetMovieListCategories().Count,
                            Title = "TODO",
                            Color = "#FFD03333"
                        },
                        loggedUser
                        );
                    AddMovieListCategory(
                        new MovieListCategory
                        {
                            Id = GetMovieListCategories().Count,
                            Title = "Watched",
                            Color = "#FFE2AF41"
                        },
                        loggedUser
                        );
                    AddMovieListCategory(
                        new MovieListCategory
                        {
                            Id = GetMovieListCategories().Count,
                            Title = "Favorites",
                            Color = "#FF4A7A25"
                        },
                        loggedUser
                        );

                    break;
                }
                case MediaCategory.Book:
                    {
                        AddBookListCategory(
                            new BookListCategory
                            {
                                Id = GetBookListCategories().Count,
                                Title = "TODO",
                                Color = "#FFD03333"
                            },
                            loggedUser
                            );
                        AddBookListCategory(
                            new BookListCategory
                            {
                                Id = GetBookListCategories().Count,
                                Title = "Read",
                                Color = "#FFE2AF41"
                            },
                            loggedUser
                            );
                        AddBookListCategory(
                            new BookListCategory
                            {
                                Id = GetBookListCategories().Count,
                                Title = "Favorites",
                                Color = "#FF4A7A25"
                            },
                            loggedUser
                            );
                        break;
                    }
                case MediaCategory.Game:
                    {
                        AddGameListCategory(
                            new GameListCategory
                            {
                                Id = GetGameListCategories().Count,
                                Title = "TODO",
                                Color = "#FFD03333"
                            },
                            loggedUser
                            );
                        AddGameListCategory(
                            new GameListCategory
                            {
                                Id = GetGameListCategories().Count,
                                Title = "Played",
                                Color = "#FFE2AF41"
                            },
                            loggedUser
                            );
                        AddGameListCategory(
                            new GameListCategory
                            {
                                Id = GetGameListCategories().Count,
                                Title = "Favorites",
                                Color = "#FF4A7A25"
                            },
                            loggedUser
                            );
                        break;
                    }
            }
        }

        public void DeleteListCategory(IListCategory listCategory, User loggedUser, MediaCategory currentMediaCategory)
        {
            switch (currentMediaCategory)
            {
                case MediaCategory.Movie:
                    Delete(listCategory as MovieListCategory, loggedUser);
                    break;
                case MediaCategory.Book:
                    Delete(listCategory as BookListCategory, loggedUser);
                    break;
                case MediaCategory.Game:
                    Delete(listCategory as GameListCategory, loggedUser);
                    break;
            }

        }

        private void Delete(MovieListCategory movieListCategory, User loggedUser)
        {
            using (var db = new MovieListCategoryRepository())
            {
                db.DeleteListForUser(movieListCategory, loggedUser);
            }
        }

        private void Delete(GameListCategory gameListCategory, User loggedUser)
        {
            using (var db = new GameListCategoryRepository())
            {
                db.DeleteListForUser(gameListCategory, loggedUser);
            }
        }

        private void Delete(BookListCategory bookListCategory, User loggedUser)
        {
            using (var db = new BookListCategoryRepository())
            {
                db.DeleteListForUser(bookListCategory, loggedUser);
            }
        }

        public void UpdateListCategory(IListCategory listCategory, User loggedUser, MediaCategory currentMediaCategory)
        {
            switch (currentMediaCategory)
            {
                case MediaCategory.Movie:
                    Update(listCategory as MovieListCategory, loggedUser);
                    break;
                case MediaCategory.Book:
                    Update(listCategory as BookListCategory, loggedUser);
                    break;
                case MediaCategory.Game:
                    Update(listCategory as GameListCategory, loggedUser);
                    break;
            }

        }

        private void Update(MovieListCategory movieListCategory, User loggedUser)
        {
            using (var db = new MovieListCategoryRepository())
            {
                db.UpdateListForUser(movieListCategory, loggedUser);
            }
        }

        private void Update(GameListCategory gameListCategory, User loggedUser)
        {
            using (var db = new GameListCategoryRepository())
            {
                db.UpdateListForUser(gameListCategory, loggedUser);
            }
        }

        private void Update(BookListCategory bookListCategory, User loggedUser)
        {
            using (var db = new BookListCategoryRepository())
            {
                db.UpdateListForUser(bookListCategory, loggedUser);
            }
        }
    }
    
}
