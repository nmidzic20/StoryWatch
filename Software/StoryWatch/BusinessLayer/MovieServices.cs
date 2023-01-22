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
using Movie = TMDbLib.Objects.Movies.Movie;

namespace BusinessLayer
{
    public class MovieServices
    {
        private string apiKey = "61a62cd9363b4d557e04105a89889368";

        public List<EntitiesLayer.Entities.Movie> GetAllMovies()
        {
            using (var repo = new MovieRepository())
            {
                return repo.GetAll().ToList();
            }
        }

        public EntitiesLayer.Entities.Movie GetMovieByTitle(string title)
        {
            using (var repo = new MovieRepository())
            {
                return repo.GetMovieByTitle(title).Single();
            }
        }

        public List<EntitiesLayer.Entities.Movie> GetMoviesForList(MovieListCategory movieListCategory, User loggedUser)
        {
            using (var repo = new MovieRepository())
            {
                return repo.GetMoviesForList(movieListCategory,loggedUser);
            }
        }

        public EntitiesLayer.Entities.Movie GetMovieByTMDBId(string TMDB_ID)
        {
            using (var repo = new MovieRepository())
            {
                return repo.GetMovieByTMDBId(TMDB_ID).Single();
            }
        }

        public bool AddMovie(EntitiesLayer.Entities.Movie movie)
        {
            bool isSuccessful = false;
            using (var repo = new MovieRepository())
            {
                //check if exists in Movies - if not, add to Movies, if yes, return false
                EntitiesLayer.Entities.Movie existingMovie = repo.GetMovieByTMDBId(movie.TMDB_ID).Single();

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

        public bool AddMovieToList(MovieListItem movieListItem)
        {
            bool isSuccessful = false;
            using (var repo = new MovieRepository())
            {
                //check if exists on that list already, if yes, return false, if no, add to list
                MovieListItem ml = null;//repo.GetMovieFromList();

                if (ml != null)
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

        public async Task<TMDbLib.Objects.Movies.Movie> GetMovieInfoAsync(int movieTMDBid)
        {
            TMDbClient client = new TMDbClient(apiKey);
            Movie movie = await client.GetMovieAsync(movieTMDBid, MovieMethods.Credits | MovieMethods.Videos);

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

    }
}
