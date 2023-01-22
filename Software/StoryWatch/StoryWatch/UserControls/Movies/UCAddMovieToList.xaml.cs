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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TMDbLib.Objects.Movies;

namespace StoryWatch.UserControls.Movies
{
    /// <summary>
    /// Interaction logic for UCAddMovieToList.xaml
    /// </summary>
    public partial class UCAddMovieToList : UserControl
    {
        public IListCategory listCategory { get; set; }
        private MovieServices movieServices = new MovieServices();
        private bool update = false;
        private EntitiesLayer.Entities.Movie movieToUpdate = null;

        public UCAddMovieToList(IListCategory listCategory)
        {
            InitializeComponent();
            this.listCategory = listCategory;
        }

        public UCAddMovieToList(IListCategory listCategory, EntitiesLayer.Entities.Movie movie)
        {
            InitializeComponent();
            this.listCategory = listCategory;

            btnAdd.Content = "Update";
            update = true;
            movieToUpdate = movie;

            txtTitle.Text = movieToUpdate.Title;
            //txtGenre.Text = movieToUpdate.Genres[0].Name;
            txtOverview.Text = movieToUpdate.Description;
            dtReleaseDate.Text = movieToUpdate.ReleaseDate;
            txtCountry.Text = movieToUpdate.Countries;
            txtID.Text = movieToUpdate.TMDB_ID;

            //TODO - when btn pressed, call movieServices.UpdateMovie -> repo.Update
        }

        private void AddMovie(object sender, RoutedEventArgs e)
        {
            if (!ValidateMovieInfo()) return;

            if (update)
            {
                UpdateMovie();
            }
            else
            {
                AddMovie();
            }
        }

        private void UpdateMovie()
        {
            var movie = new EntitiesLayer.Entities.Movie
            {
                Id = movieToUpdate.Id,
                Title = txtTitle.Text,
                Description = txtOverview.Text,
                TMDB_ID = movieToUpdate.TMDB_ID,
                Countries = txtCountry.Text,
                ReleaseDate = dtReleaseDate.DisplayDate.ToString()

            };

            int isSuccessful = movieServices.UpdateMovie(movie);
            if (isSuccessful != 0)
                GuiManager.OpenContent(new UCMediaHome(EntitiesLayer.MediaCategory.Movie));
            else
                MessageBox.Show("Update failed");

        }

        private void AddMovie()
        {
            var allMovies = movieServices.GetAllMovies();
            var movieId = (allMovies.Count() != 0) ? allMovies.Last().Id + 1 : 0;

            bool isSuccessful = movieServices.AddMovie(new EntitiesLayer.Entities.Movie
            {
                Id = movieId,
                Title = txtTitle.Text,
                Description = txtOverview.Text,
                TMDB_ID = txtID.Text,
                Countries = txtCountry.Text,
                ReleaseDate = dtReleaseDate.DisplayDate.ToString()

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
                GuiManager.OpenContent(new UCMediaHome(EntitiesLayer.MediaCategory.Movie));
        }

        private bool ValidateMovieInfo()
        {
            if (string.IsNullOrEmpty(txtTitle.Text))
                    return false;

            return true;
        }

        private void SearchTMDb(object sender, RoutedEventArgs e)
        {
            //open search form, when search form closes, if any movie selected, it will fill textboxes in this UC
            //otherwise if no movie selected, will do nothing
            GuiManager.OpenContent(new UCSearchMovie(this));
        }

        private void TextTitleChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTitle.Text)) 
                btnAdd.IsEnabled = true;
        }

        private void Return(object sender, RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UCMediaHome(EntitiesLayer.MediaCategory.Movie));
        }


    }
}
