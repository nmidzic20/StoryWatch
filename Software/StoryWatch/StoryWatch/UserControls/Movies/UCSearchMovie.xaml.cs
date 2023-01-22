using BusinessLayer;
using EntitiesLayer.Entities;
using StoryWatch.UserControls.Movies;
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
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using static System.Net.WebRequestMethods;
using Movie = TMDbLib.Objects.Movies.Movie;

namespace StoryWatch.UserControls
{
    /// <summary>
    /// Interaction logic for UCAddMedia.xaml
    /// </summary>
    public partial class UCSearchMovie : UserControl
    {
        private Movie selectedMovie = null;

        private UCAddMovieToList ucAddMovieToList = null;

        private ListCategoryServices listCategoryServices = new ListCategoryServices();
        private MovieServices movieServices = new MovieServices();

        private string placeholderTextCollection = "Search movies by franchise";
        private string placeholderTextKeyword = "Search movies by keyword";
        private string delimiter = " | ID: ";

        public UCSearchMovie(UCAddMovieToList UCAddMovieToList)
        {
            InitializeComponent();

            ucAddMovieToList = UCAddMovieToList;

        }


        private void txtSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox txtSearch = sender as TextBox;
            txtSearch.Text = "";
            txtSearch.FontStyle = FontStyles.Normal;
            txtSearch.FontWeight = FontWeights.Normal;
            txtSearch.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void txtSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox txtSearch = sender as TextBox;
            
            if (txtSearch.Name == "txtSearchKeyword")
                txtSearch.Text = placeholderTextKeyword;
            else if (txtSearch.Name == "txtSearchCollection")
                txtSearch.Text = placeholderTextCollection;

            txtSearch.FontStyle = FontStyles.Italic;
            txtSearch.FontWeight = FontWeights.Bold;
            txtSearch.Foreground = new SolidColorBrush(Colors.SlateGray);
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (dgResults == null || 
                string.IsNullOrEmpty(txtSearchKeyword.Text) || 
                txtSearchKeyword.Text == placeholderTextKeyword) 
                    
                return;

            dgResults.ItemsSource = null;

            var movies = movieServices.SearchMoviesByKeyword(txtSearchKeyword.Text);

            if (movies == null)
            {
                return;
            }

            dgResults.ItemsSource = movies;

        }

        private void txtSearchCollection_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (dgResults == null ||
                string.IsNullOrEmpty(txtSearchCollection.Text) ||
                txtSearchCollection.Text == placeholderTextCollection)

                return;

            dgResults.ItemsSource = null;

            var movies = movieServices.SearchMoviesByCollection(txtSearchCollection.Text);

            if (movies == null)
            {
                return;
            }


            dgResults.ItemsSource = movies;

        }

        private async void lbResults_SelectionChangedAsync(object sender, SelectionChangedEventArgs e)
        {
            /*if (e.AddedItems.Count == 0) return;

            var item = e.AddedItems[0] as string;
            var id = item.Split(new string[] { delimiter }, StringSplitOptions.RemoveEmptyEntries)[1];
            
            int idInt;
            Int32.TryParse(id, out idInt);*/

            if (dgResults.SelectedItem == null) return;

            var item = dgResults.SelectedItem as TMDbLib.Objects.Search.SearchMovie;
            var movieTMDbId = item.Id;

            TMDbLib.Objects.Movies.Movie movie = await movieServices.GetMovieInfoAsync(movieTMDbId);

            selectedMovie = movie;

            /*important for trailers
             *             
             string movieInfo = "";

             * string urlYoutube = "https://www.youtube.com/watch?v=";
            string trailerURL = urlYoutube + movie.Videos.Results[0].Key;
            movieInfo += movie.Title + " " + movie.Homepage + " " + movie.Genres[0].Name + " "
                + movie.Runtime + " " + movie.BackdropPath + " " + trailerURL;*/


        }

        private void BtnSelectMovie(object sender, RoutedEventArgs e)
        {
            this.ucAddMovieToList.txtTitle.Text = selectedMovie.Title;
            this.ucAddMovieToList.txtGenre.Text = selectedMovie.Genres[0].Name;
            this.ucAddMovieToList.txtOverview.Text = selectedMovie.Overview;
            this.ucAddMovieToList.dtReleaseDate.Text = selectedMovie.ReleaseDate.ToString();
            foreach (var country in selectedMovie.ProductionCountries)
                this.ucAddMovieToList.txtCountry.Text += country.Name + " ";
            this.ucAddMovieToList.txtID.Text = selectedMovie.Id.ToString();

            GuiManager.CloseContent();
        }

        private void BtnCancel(object sender, RoutedEventArgs e)
        {
            GuiManager.CloseContent();
        }
    }
}
