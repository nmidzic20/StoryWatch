using DataAccessLayer.Repositories;
using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Net.Http.Headers;
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
        public List<GenreTMDB> GenresTMDB { get; set; } = new List<GenreTMDB>();
        public List<GenreTMDB> GenresRelax { get; set; } = new List<GenreTMDB>();
        public List<GenreTMDB> GenresSocial { get; set; } = new List<GenreTMDB>();
        public List<GenreTMDB> GenresAdrenaline { get; set; } = new List<GenreTMDB>();
        public List<GenreTMDB> GenresFantasy { get; set; } = new List<GenreTMDB>();


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

        public async Task FillGenres()
        {
            await GetGenresTMDBAsync();
            List<string> genreNamesRelax = new List<string>();
            List<string> genreNamesSocial = new List<string>();
            List<string> genreNamesAdrenaline = new List<string>();
            List<string> genreNamesFantasy = new List<string>();


            genreNamesRelax.AddRange(new List<string>
            {
                "Animation", "Comedy", "Documentary", "Family", "Music", "Romance"
            });
            genreNamesSocial.AddRange(new List<string>
            {
                "Crime", "Documentary", "Drama", "Family", "TV Movie", "War"
            });
            genreNamesFantasy.AddRange(new List<string>
            {
                "Fantasy", "Science Fiction", "Western"
            });
            genreNamesAdrenaline.AddRange(new List<string>
            {
                "Action", "Adventure", "Crime", "Horror", "Thriller", "War"
            });

            GenresRelax = GenresTMDB.Where(g => genreNamesRelax.Exists(n => n == g.Name)).ToList();
            GenresAdrenaline = GenresTMDB.Where(g => genreNamesAdrenaline.Exists(n => n == g.Name)).ToList();
            GenresSocial = GenresTMDB.Where(g => genreNamesSocial.Exists(n => n == g.Name)).ToList();
            GenresFantasy = GenresTMDB.Where(g => genreNamesFantasy.Exists(n => n == g.Name)).ToList();

        }

        private async Task GetGenresTMDBAsync()
        {
            GenresTMDB = await movieServices.GetTMDBGenresAsync();
        }

        public async Task<List<SearchMovie>> RecommendMovies(List<GenreTMDB> preferredGenres, List<MovieListCategory> preferredListCategories)
        {
            recommendedMoviesPoints = new Dictionary<SearchMovie, double>();

            this.preferredListCategories = preferredListCategories;
            this.preferredGenres = preferredGenres;

            if (!preferredListCategories.Any(l => l.Title == "TODO")) //no TODO
                return await RecommendWatched();
            else
                return await RecommendUnwatched();

        }

        private async Task<List<SearchMovie>> RecommendWatched()
        {
            List<SearchMovie> moviesTMDB = await GetMoviesTMDBFromChosenGenres();

            AssignPoints(moviesFromOtherLists, 1);
            AssignPoints(moviesFromFavorites, 2);

            List<SearchMovie> recommendedMovies = RankMoviesByPoints();

            return recommendedMovies;

        }

        [Obsolete]
        private async Task<List<SearchMovie>> RecommendUnwatched()
        {
            List<SearchMovie> moviesTMDB = await GetMoviesTMDBFromChosenGenres();

            AssignPoints(moviesFromTODO, 0.3);
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
