using DataAccessLayer.Repositories;
using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;
using GenreTMDB = TMDbLib.Objects.General.Genre;
using Movie = EntitiesLayer.Entities.Movie;

namespace BusinessLayer
{
    public class RecommendServices
    {
        private List<GenreTMDB> preferredGenres;
        private List<MovieListCategory> preferredListCategories;
        private User loggedUser;
        private HashSet<Movie> moviesFromOtherLists = new HashSet<Movie>();
        private HashSet<Movie> moviesFromTODO = new HashSet<Movie>();
        private HashSet<Movie> moviesFromFavorites = new HashSet<Movie>();

        private MovieServices movieServices = new MovieServices();

        private Dictionary<SearchMovie, double> recommendedMoviesPoints;



        public RecommendServices(User loggedUser)
        {
            this.loggedUser = loggedUser;

            var listServices = new ListCategoryServices();
            var lists = listServices.GetMovieListCategoriesForUser(loggedUser);

            using (var repo = new MovieRepository())
            {
                foreach (var list in lists)
                {
                    var movies = repo.GetMoviesForList(list, loggedUser);
                    if (list.Title != "TODO")
                    {
                        if (list.Title == "Favorites")
                            moviesFromFavorites.UnionWith(movies);
                        else
                            moviesFromOtherLists.UnionWith(movies);
                    }
                    else
                    {
                        moviesFromTODO.UnionWith(movies);
                    }
                    
                }

            }

        }

        public async Task<List<SearchMovie>> RecommendMovies(List<GenreTMDB> preferredGenres, List<MovieListCategory> preferredListCategories)
        {
            /*
             * if lists only favorites, watched... -> take into account those movies, if possible match with preferred genres (db info only)
             * give 3 points to favorited movies, and 1 point to rest
             * and return ordered by points, with message warning of too little info if few movies
             * 
             * if lists are either or only TODO, then involve TMDB:
             - take into account only movies within those genres -> GetMovieByGenre for all preferred genres
            - from chosen lists (all including TODO, or just TODO) get movies, iterate through them
            get director+main cast info via TMDB for each, then select the TMDB movies taken into account above that include
            that cast/director info,
            and give each movies points according to their credit info:
            3 points for same director
            0.5 point for each cast member that appears in those as well
            in case of either list, additionally give 3 points to fav, 2 points to all watched lists, and 1 point to todo
                then join that list with tmdb movies
            order by points and return
             */

            recommendedMoviesPoints = new Dictionary<SearchMovie, double>();

            this.preferredListCategories = preferredListCategories;
            this.preferredGenres = preferredGenres;

            if (!preferredListCategories.Any(l => l.Title == "TODO")) //no TODO
                return await RecommendWatched();
            else
                return await RecommendUnwatched();

        }

        [Obsolete]
        private async Task<List<SearchMovie>> RecommendUnwatched()
        {
            List<SearchMovie> moviesTMDB = await GetMoviesTMDBFromChosenGenres();

            AssignPoints(moviesFromTODO, 1);
            await FindMoviesTMDBByFavouriteGenres(moviesTMDB);

            List<SearchMovie> recommendedMovies = RankMoviesByPoints();

            return recommendedMovies;

        }

        private async Task FindMoviesTMDBByFavouriteGenres(List<SearchMovie> moviesTMDB)
        {
            foreach (var movie in moviesFromFavorites)
            {
                if (string.IsNullOrEmpty(movie.TMDB_ID)) continue;

                var credits = await GetCreditsDataForMovie(movie);
                var director = credits.Director;
                var mainCast = credits.MainCast;

                foreach (var movieTMDB in moviesTMDB)
                {
                    var credits2 = await GetCreditsDataForMovie(movie);
                    var director2 = credits2.Director;
                    var mainCast2 = credits2.MainCast;

                    double points = 0;

                    if (director2 == director) points += 3;

                    foreach (var castMember in mainCast2)
                        if (mainCast.Exists(c => c == castMember))
                            points += 0.5;

                    if (recommendedMoviesPoints.ContainsKey(movieTMDB))
                        recommendedMoviesPoints[movieTMDB] += points;
                    else
                        recommendedMoviesPoints.Add(movieTMDB, points);
                }

            }
        }

        private List<SearchMovie> RankMoviesByPoints()
        {
            var rankedMoviesDict = from entry in recommendedMoviesPoints orderby entry.Value ascending select entry.Key;
            List<SearchMovie> recommendedMovies = new List<SearchMovie>();
            foreach (var m in rankedMoviesDict)
                if (!recommendedMovies.Exists(r => r.Title == m.Title))
                    recommendedMovies.Add(m);
            return recommendedMovies;
        }

        private async Task<List<SearchMovie>> GetMoviesTMDBFromChosenGenres()
        {
            List<SearchMovie> moviesTMDB = new List<SearchMovie>();

            foreach (GenreTMDB genreTMDB in preferredGenres)
            {
                //take 2 movies from each preferred genre
                var results = (await movieServices.GetMoviesByGenre(genreTMDB.Id)).Take(2).ToList();
                moviesTMDB.AddRange(results);
            }

            return moviesTMDB;
        }

        class CreditsData
        {
            public string Director { get; set; }
            public List<string> MainCast { get; set; }
        }

        private async Task<CreditsData> GetCreditsDataForMovie(Movie movie)
        {
            var credits = await movieServices.GetMovieCreditsAsync(int.Parse(movie.TMDB_ID));
            var director = credits.Crew.Where(c => c.Job == "Director").Select(c => c.Name).SingleOrDefault();
            var mainCast = credits.Cast.Take(credits.Cast.Count < 5 ? credits.Cast.Count : 5).Select(c => c.Name).ToList();

            return new CreditsData
            { 
                Director = director,
                MainCast = mainCast
            };
        }

        private async Task<List<SearchMovie>> RecommendWatched()
        {
            List<SearchMovie> moviesTMDB = await GetMoviesTMDBFromChosenGenres();
            
            AssignPoints(moviesFromOtherLists, 1);
            AssignPoints(moviesFromFavorites, 2);

            List<SearchMovie> recommendedMovies = RankMoviesByPoints();

            return recommendedMovies;

        }

        private void AssignPoints(HashSet<Movie> movies, double points)
        {
            foreach (var movie in movies)
            {
                SearchMovie searchMovie = new SearchMovie
                {
                    Title = movie.Title,
                    Overview = movie.Description
                };

                recommendedMoviesPoints.Add(searchMovie, points);
            }
        }
    }
}
