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
    /// Interaction logic for UCRecommendMovies.xaml
    /// </summary>
    public partial class UCRecommendMovies : UserControl
    {
        public string MovieCriterion1 { get; set; }
        public string MovieCriterion2 { get; set; }

        private RecommendServices recommendServices = new RecommendServices();
        private MovieServices movieServices = new MovieServices();

        public UCRecommendMovies()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UCMediaHome(EntitiesLayer.MediaCategory.Movie));
        }

        private void btnRelax_Click(object sender, RoutedEventArgs e)
        {
            ShowNextQuestion(sender);
        }

        private void btnScary_Click(object sender, RoutedEventArgs e)
        {
            ShowNextQuestion(sender);

        }

        private void btnSocial_Click(object sender, RoutedEventArgs e)
        {
            ShowNextQuestion(sender);

        }

        private void btnFantasy_Click(object sender, RoutedEventArgs e)
        {
            ShowNextQuestion(sender);

        }

        private void ShowNextQuestion(object sender)
        {
            Button button = sender as Button;
            MovieCriterion1 = button.Name;

            btnRelax.Visibility = Visibility.Collapsed;
            btnScary.Visibility = Visibility.Collapsed;
            btnFantasy.Visibility = Visibility.Collapsed;
            btnSocial.Visibility = Visibility.Collapsed;

            btnOld.Visibility = Visibility.Visible;
            btnNew.Visibility = Visibility.Visible;
            btnEither.Visibility = Visibility.Visible;

        }

        private void btnOld_Click(object sender, RoutedEventArgs e)
        {
            HideButtons(sender);
            RecommendMovies();
        }

        private void btnnew_Click(object sender, RoutedEventArgs e)
        {
            HideButtons(sender);
            RecommendMovies();
        }

        private void btnEither_Click(object sender, RoutedEventArgs e)
        {
            HideButtons(sender);
            RecommendMovies();
        }

        private void HideButtons(object sender)
        {
            Button button = sender as Button;
            MovieCriterion2 = button.Name;

            btnOld.Visibility = Visibility.Collapsed;
            btnNew.Visibility = Visibility.Collapsed;
            btnEither.Visibility = Visibility.Collapsed;

        }

        private void RecommendMovies()
        {
            //MessageBox.Show(MovieCriterion1 + " " + MovieCriterion2);

            var criteria = new List<string> { "dragon", MovieCriterion2 };

            var movies = recommendServices.RecommendMovies(criteria);
            dgRecommendedMovies.ItemsSource = movies;
        }

        private async void lbResults_SelectionChangedAsync(object sender, SelectionChangedEventArgs e)
        {

            if (dgRecommendedMovies.SelectedItem == null) return;

            var item = dgRecommendedMovies.SelectedItem as TMDbLib.Objects.Search.SearchMovie;
            var movieTMDbId = item.Id;

            TMDbLib.Objects.Movies.Movie movie = await movieServices.GetMovieInfoAsync(movieTMDbId);

        }

    }
}
