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

namespace BusinessLayer
{
    public class MovieServices
    {
        private string apiKey = "61a62cd9363b4d557e04105a89889368";

        public async Task<TMDbLib.Objects.Movies.Movie> GetMovieInfoAsync(int movieTMDBid)
        {
            /*TMDbClient client = new TMDbClient(apiKey);
            TMDbLib.Objects.Movies.Movie movie = client.GetMovieAsync(movieTMDBid).Result;

            Console.WriteLine($"Movie name: {movie.Title}");*/
            
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
