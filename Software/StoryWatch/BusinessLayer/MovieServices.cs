using DataAccessLayer.Repositories;
using EntitiesLayer.Entities;
using EntitiesLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDbLib.Client;
using TMDbLib.Objects.Collections;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;
using Movie = EntitiesLayer.Entities.Movie;
using MovieTMDB = TMDbLib.Objects.Movies.Movie;
using Collection = TMDbLib.Objects.Collections.Collection;
using System.Security.AccessControl;

namespace BusinessLayer
{
    /// <summary>
    /// Function: movie management service
    /// Author: Noa Midžić
    /// </summary>
    public class MovieServices
    {
        private string apiKey = "61a62cd9363b4d557e04105a89889368";

        public List<Movie> GetAllMovies()
        {
            using (var repo = new MovieRepository())
            {
                return repo.GetAll().ToList();
            }
        }

        public Movie GetMovieByTitle(string title)
        {
            using (var repo = new MovieRepository())
            {
                return repo.GetMovieByTitle(title).FirstOrDefault();
            }
        }

        public Movie GetMovieById(int id)
        {
            using (var repo = new MovieRepository())
            {
                return repo.GetMovieById(id).FirstOrDefault();
            }
        }

        public List<Movie> GetMoviesForList(MovieListCategory movieListCategory, User loggedUser)
        {
            using (var repo = new MovieRepository())
            {
                return repo.GetMoviesForList(movieListCategory,loggedUser);
            }
        }

        public Movie GetMovieByTMDBId(string TMDB_ID)
        {
            using (var repo = new MovieRepository())
            {
                return repo.GetMovieByTMDBId(TMDB_ID).FirstOrDefault();
            }
        }

        public bool AddMovie(Movie movie)
        {
            bool isSuccessful = false;
            using (var repo = new MovieRepository())
            {
                //check if exists in Movies - if not, add to Movies, if yes, return false
                //important - this check must be performed only if movie was added from TMDb - if it was
                //added manually, there is no TMDB ID and nothing to compare
                Movie existingMovie = null;

                if (!string.IsNullOrEmpty(movie.TMDB_ID))
                    existingMovie = repo.GetMovieByTMDBId(movie.TMDB_ID).FirstOrDefault();

                if (existingMovie != null)
                {
                    isSuccessful = false;
                }
                else
                {
                    int affectedRows = repo.Add(movie);
                    isSuccessful = affectedRows > 0;
                }

            }

            return isSuccessful;

        }

        public bool AddMovieToList(MovieListItem movieListItem, MovieListCategory movieListCategory, User loggedUser)
        {
            bool isSuccessful = false;
            using (var repo = new MovieRepository())
            {
                //check if exists on that list already, if yes, return false, if no, add to list
                List<Movie> movies = repo.GetMoviesForList(movieListCategory,loggedUser).ToList();
                bool movieExistsOnList = movies.Exists(m => m.Id == movieListItem.Id_Movies);

                if (movieExistsOnList)
                {
                    isSuccessful = false;
                }
                else
                {
                    int affectedRows = repo.AddMovieToList(movieListItem);
                    isSuccessful = affectedRows > 0;
                }

            }

            return isSuccessful;
        }

        public int UpdateMovie(Movie movie)
        {
            using (var repo = new MovieRepository())
            {
                return repo.Update(movie);
            }
        }

        public bool UpdateMovieToAnotherList(MovieListItem movieListItem, MovieListCategory destMovieListCategory, User loggedUser)
        {
            bool isSuccessful = false;
            using (var repo = new MovieRepository())
            {
                //check if exists on destination list already, if yes, return false, if no, change to that list
                List<Movie> movies = repo.GetMoviesForList(destMovieListCategory, loggedUser).ToList();
                bool movieExistsOnList = movies.Exists(m => m.Id == movieListItem.Id_Movies);

                if (movieExistsOnList)
                {
                    isSuccessful = false;
                }
                else
                {
                    //fetch movieListItem for this movie, this list and this user
                    //change the list
                    int affectedRows = repo.UpdateMovieListItem(movieListItem, destMovieListCategory.Id);
                    isSuccessful = affectedRows > 0;
                }

            }

            return isSuccessful;
        }

        public bool DeleteMovieFromList(Movie movie, MovieListCategory movieListCategory, User loggedUser)
        {
            bool isSuccessful = false;

            var movieListItem = new MovieListItem
            {
                Id_MovieListCategories = movieListCategory.Id,
                Id_Movies = movie.Id,
                Id_Users = loggedUser.Id
            };

            using (var db = new GenreRepository())
            {
                var movieGenre = GetMovieById(movie.Id).Genre;
                db.DeleteGenreWithoutMedia(movieGenre);
            }

            using (var repo = new MovieRepository())
            {
                int affectedRows = repo.DeleteMovieFromList(movieListItem);
                isSuccessful = affectedRows > 0;

                return isSuccessful;
            }
        }

        public async Task<TMDbLib.Objects.Movies.Movie> GetMovieInfoAsync(int movieTMDBid)
        {
            TMDbClient client = new TMDbClient(apiKey);
            MovieTMDB movie = await client.GetMovieAsync(movieTMDBid, MovieMethods.Credits | MovieMethods.Videos);

            Console.WriteLine($"Movie title: {movie.Title}");
            foreach (Cast cast in movie.Credits.Cast)
                Console.WriteLine($"{cast.Name} - {cast.Character}");

            Console.WriteLine();
            foreach (Video video in movie.Videos.Results)
                Console.WriteLine($"Trailer: {video.Type} ({video.Site}), {video.Name}");

            return movie;
        }

        public List<SearchMovie> SearchMoviesByKeyword(string keyword)
        {
            TMDbClient client = new TMDbClient(apiKey);
            SearchContainer<SearchMovie> results = client.SearchMovieAsync(keyword).Result;

            Console.WriteLine($"Got {results.Results.Count:N0} of {results.TotalResults:N0} results");
            foreach (SearchMovie result in results.Results)
                Console.WriteLine(result.Title);

            return results.Results;
        }

        public List<SearchMovie> SearchMoviesByCollection(string keyword)
        {
            TMDbClient client = new TMDbClient(apiKey);
            SearchContainer<SearchCollection> collections = client.SearchCollectionAsync(keyword).Result;
            Console.WriteLine($"Got {collections.Results.Count:N0} collections");

            if (collections.Results.Count > 0)
            {
                Collection collection = client.GetCollectionAsync(collections.Results.First().Id).Result;
                Console.WriteLine($"Collection: {collection.Name}");
                Console.WriteLine();

                Console.WriteLine($"Got {collection.Parts.Count:N0} Movies");

                List<SearchMovie> searchMovies = new List<SearchMovie>();

                foreach (SearchMovie part in collection.Parts)
                {
                    Console.WriteLine(part.Title);
                    searchMovies.Add(part);
                }

                Console.WriteLine("collection");
                return searchMovies;
            }
            else
                return null;
        }

        public async Task<List<TMDbLib.Objects.General.Genre>> GetTMDBGenresAsync()
        {
            TMDbClient client = new TMDbClient(apiKey);
            var genresTMDB = await client.GetMovieGenresAsync();

            return genresTMDB;

        }

        [Obsolete]
        public async Task<List<SearchMovie>> GetMoviesByGenre(int genreId)
        {
            TMDbClient client = new TMDbClient(apiKey);
            var movies = await client.GetGenreMoviesAsync(genreId);

            return movies.Results;
        }

        public async Task<Credits> GetMovieCreditsAsync(int movieId)
        {
            TMDbClient client = new TMDbClient(apiKey);
            var credits = await client.GetMovieCreditsAsync(movieId);
            
            return credits;
        }
    }
}
