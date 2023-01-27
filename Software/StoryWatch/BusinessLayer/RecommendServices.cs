using DataAccessLayer.Repositories;
using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDbLib.Objects.Movies;
using GenreTMDB = TMDbLib.Objects.General.Genre;

namespace BusinessLayer
{
    public class RecommendServices
    {
        [Obsolete]
        public async Task<List<TMDbLib.Objects.Search.SearchMovie>> RecommendMovies(List<GenreTMDB> preferredGenres, List<MovieListCategory> preferredListCategories)
        {
            var movieServices = new MovieServices();
            var movies = await movieServices.GetMoviesByGenre(preferredGenres[0].Id);


            return movies;

            /*using (var repo = new MovieRepository())
            {
                var movies = repo.
                return movies;
            }*/
        }
    }
}
