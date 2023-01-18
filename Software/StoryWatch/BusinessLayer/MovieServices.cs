using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDbLib.Client;

namespace BusinessLayer
{
    public class MovieServices
    {
        public void GetMovieTMDB()
        {
            TMDbClient client = new TMDbClient("APIKey");
            TMDbLib.Objects.Movies.Movie movie = client.GetMovieAsync(47964).Result;

            Console.WriteLine($"Movie name: {movie.Title}");
        }

    }
}
