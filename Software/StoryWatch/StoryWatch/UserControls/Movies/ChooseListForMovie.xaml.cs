using BusinessLayer;
using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace StoryWatch.UserControls.Movies
{
    /// <summary>
    /// Author: Noa Midžić
    /// Choose any of the user's list to add the recommended movie to
    /// </summary>
    public partial class ChooseListForMovie : Window
    {
        private ListCategoryServices listServices = new ListCategoryServices();
        private MovieServices movieServices = new MovieServices();
        private SearchMovie searchMovie = new SearchMovie();

        public ChooseListForMovie(SearchMovie movie)
        {
            InitializeComponent();

            this.searchMovie = movie;
            FillComboBox();
        }

        private void FillComboBox()
        {
            var listsForUser = listServices.GetMovieListCategoriesForUser(StateManager.LoggedUser);
            cboListCategoriesForUser.ItemsSource = listsForUser;
        }

        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var chosenList = cboListCategoriesForUser.SelectedItem as MovieListCategory;

            var movieId = await AddMovie();
            AddMovieToChosenList(chosenList, movieId);

            Close();
        }

        private void AddMovieToChosenList(MovieListCategory chosenList, int movieId)
        {
            var movieListItem = new MovieListItem
            {
                Id_MovieListCategories = chosenList.Id,
                Id_Movies = movieId,
                Id_Users = StateManager.LoggedUser.Id
            };
            bool movieAddedToList = movieServices.AddMovieToList(movieListItem, chosenList, StateManager.LoggedUser);

            if (!movieAddedToList)
                MessageBox.Show("This movie is already added to this list");
            else
                GuiManager.OpenContent(new UCMediaHome(EntitiesLayer.MediaCategory.Movie));
        }

        private async Task<int> AddMovie()
        {    
            //there can be recommended a movie which does not have TMDB link
            //if a movie which does not have TMDB link is recommended, it means it is a movie
            //that was already in DB, manually added by user
            if (searchMovie.Id == 0)
            {
                //MessageBox.Show("This movie is already added from TMDB to database");
                var id = movieServices.GetMovieByTitle(searchMovie.Title).Id;
                return id;
            }

            var allMovies = movieServices.GetAllMovies();
            var movieId = (allMovies.Count() != 0) ? allMovies.Last().Id + 1 : 0;

            var movieTMDB = await movieServices.GetMovieInfoAsync(searchMovie.Id);

            string countries = "";
            foreach (var country in movieTMDB.ProductionCountries)
            {
                countries += country.Name;
                countries += (country == movieTMDB.ProductionCountries.Last()) ? " " : ", ";
            }

            bool isSuccessful = movieServices.AddMovie(new EntitiesLayer.Entities.Movie
            {
                Id = movieId,
                Title = movieTMDB.Title,
                Description = movieTMDB.Overview,
                TMDB_ID = movieTMDB.Id.ToString(),
                Countries = countries,
                ReleaseDate = movieTMDB.ReleaseDate.ToString(),
                Trailer_URL = (movieTMDB.Videos.Results.Count != 0) ? movieTMDB.Videos.Results[0].Key : null,
                Genre = null

            });

            if (!isSuccessful)
            {
                //MessageBox.Show("This movie is already added from TMDB to database");
                movieId = movieServices.GetMovieByTMDBId(movieTMDB.Id.ToString()).Id;
            }

            return movieId;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ChooseList_KeyDown(object sender, KeyEventArgs e)
        {
            /*if (e.Key == Key.F1)
                MessageBox.Show("AddBook " + GuiManager.currentContent.GetType().Name);*/

            if (e.Key == Key.F1)
            {
                System.Diagnostics.Process.Start(@"PDF\\ChooseListForMovie.pdf");
            }
        }
            
    }
}
