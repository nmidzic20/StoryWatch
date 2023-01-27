using DataAccessLayer.Repositories;
using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TMDbLib.Objects.Movies;

namespace BusinessLayer
{
    public class RecommendServices
    {
        public List<TMDbLib.Objects.Search.SearchMovie> RecommendMovies(List<string> criteria)
        {
            var movieServices = new MovieServices();
            var movies = movieServices.SearchMoviesByKeyword(criteria[0]);

            return movies;

            /*using (var repo = new MovieRepository())
            {
                var movies = repo.
                return movies;
            }*/
        }
        
        public async Task<List<IGDB.Models.Game>> RecommendGames(int[] ids)
        {
            var gameServices = new GameServices();
            return (await gameServices.GetRecommendedGamesAsync(ids)).ToList();
        }
        
        public async Task<List<IGDB.Models.Game>> RecommendBestGames()
        {
            var gameServices = new GameServices();
            return (await gameServices.GetHighestRatedGames()).ToList();
        }
    }
}
