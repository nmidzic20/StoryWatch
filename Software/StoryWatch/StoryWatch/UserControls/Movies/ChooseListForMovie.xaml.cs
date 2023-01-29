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
using TMDbLib.Objects.Search;

namespace StoryWatch.UserControls.Movies
{
    /// <summary>
    /// Interaction logic for ChooseListForMovie.xaml
    /// </summary>
    public partial class ChooseListForMovie : Window
    {
        private ListCategoryServices listServices = new ListCategoryServices();
        private MovieServices movieServices = new MovieServices();
        private SearchMovie movie = new SearchMovie();

        public ChooseListForMovie(SearchMovie movie)
        {
            InitializeComponent();

            this.movie = movie;
            FillComboBox();
        }

        private void FillComboBox()
        {
            var listsForUser = listServices.GetMovieListCategoriesForUser(StateManager.LoggedUser);
            cboListCategoriesForUser.ItemsSource = listsForUser;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var chosenList = cboListCategoriesForUser.SelectedItem as MovieListCategory;
            /*
            var allMovies = movieServices.GetAllMovies();
            var movieId = (allMovies.Count() != 0) ? allMovies.Last().Id + 1 : 0;

            Genre genre = null;
            //add genre, fetch that genre, add it to the movie below
            if (!string.IsNullOrEmpty(txtGenre.Text))
            {
                var genreServices = new GenreServices();
                int genreId = (genreServices.GetAllGenres().LastOrDefault() != null) ? genreServices.GetAllGenres().Last().Id + 1 : 0;
                genre = new Genre
                {
                    Id = genreId,
                    Name = txtGenre.Text
                };
                genreId = genreServices.AddGenre(genre).Id;
                genre = genreServices.GetGenreById(genreId);
            }

            bool isSuccessful = movieServices.AddMovie(new EntitiesLayer.Entities.Movie
            {
                Id = movieId,
                Title = txtTitle.Text,
                Description = txtOverview.Text,
                TMDB_ID = txtID.Text,
                Countries = txtCountry.Text,
                ReleaseDate = dtReleaseDate.DisplayDate.ToString(),
                Trailer_URL = txtTrailerURL.Text,
                Genre = genre

            });

            if (!isSuccessful)
            {
                //MessageBox.Show("This movie is already added from TMDB to database");
                movieId = movieServices.GetMovieByTMDBId(txtID.Text).Id;
            }

            MovieListItem movie = new MovieListItem
            {
                Id_Users = StateManager.LoggedUser.Id,
                Id_Movies = movieId,
                Id_MovieListCategories = this.listCategory.Id
            };
            bool movieAddedToList = movieServices.AddMovieToList(movie, listCategory as MovieListCategory, StateManager.LoggedUser);

            if (!movieAddedToList)
                MessageBox.Show("This movie is already added to this list");
            else
                GuiManager.OpenContent(new UCMediaHome(EntitiesLayer.MediaCategory.Movie));*/

            var movieId = 0;
            var movieListItem = new MovieListItem
            {
                Id_MovieListCategories = chosenList.Id,
                Id_Movies = movieId,
                Id_Users = StateManager.LoggedUser.Id
            };
            movieServices.AddMovieToList(movieListItem, chosenList,StateManager.LoggedUser);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
