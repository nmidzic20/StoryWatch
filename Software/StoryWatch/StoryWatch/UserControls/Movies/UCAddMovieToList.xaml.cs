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

namespace StoryWatch.UserControls.Movies
{
    /// <summary>
    /// Interaction logic for UCAddMovieToList.xaml
    /// </summary>
    public partial class UCAddMovieToList : UserControl
    {
        public IListCategory listCategory { get; set; }
        public Movie SelectedMovieFromSearchTMDB { get; set; }

        private MovieServices movieServices = new MovieServices();

        public UCAddMovieToList(IListCategory listCategory)
        {
            InitializeComponent();
            this.listCategory = listCategory;
        }

        private void AddMovie(object sender, RoutedEventArgs e)
        {
            if (!ValidateMovieInfo()) return;

            var movieId = movieServices.GetAllMovies().Count;
            movieServices.AddMovie(new Movie
            {
                Id = movieId,
                Title = txtTitle.Text,
                Description = txtOverview.Text

            });

            MovieListItem movie = new MovieListItem
            {
                Id_Users = StateManager.LoggedUser.Id,
                Id_Movies = movieId,
                Id_MovieListCategories = this.listCategory.Id
            };
            movieServices.AddMovieToList(movie);

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
            //open search form, when search form closes, if any movie chosen, get info and fill textboxes
            //otherwise do nothing
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
