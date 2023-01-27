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
using GenreTMDB = TMDbLib.Objects.General.Genre;
using Genre = EntitiesLayer.Entities.Genre;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace StoryWatch.UserControls.Movies
{
    /// <summary>
    /// Interaction logic for UCRecommendMovies.xaml
    /// </summary>
    public partial class UCRecommendMovies : UserControl
    {
        private List<GenreTMDB> preferredGenres = new List<GenreTMDB>();
        private List<MovieListCategory> preferredListCategories = new List<MovieListCategory>();

        private Dictionary<string, List<GenreTMDB>> genresDict;

        private RecommendServices recommendServices;
        private MovieServices movieServices = new MovieServices();
        private ListCategoryServices listCategoryServices = new ListCategoryServices();


        public UCRecommendMovies()
        {
            InitializeComponent();
            recommendServices = new RecommendServices(StateManager.LoggedUser);
            FillGenres();
        }

        private async Task FillGenres()
        {
            await recommendServices.FillGenres();
            FillGenreDict();
        }

        private void FillGenreDict()
        {
            List<GenreTMDB> genresRelax = new List<GenreTMDB>();
            genresRelax.AddRange(recommendServices.GenresRelax);
            List<GenreTMDB> genresSocial = new List<GenreTMDB>();
            genresSocial.AddRange(recommendServices.GenresSocial);
            List<GenreTMDB> genresAdrenaline = new List<GenreTMDB>();
            genresAdrenaline.AddRange(recommendServices.GenresAdrenaline);
            List<GenreTMDB> genresFantasy = new List<GenreTMDB>();
            genresFantasy.AddRange(recommendServices.GenresFantasy);

            genresDict = new Dictionary<string, List<GenreTMDB>>()
            {
                { btnRelax.Name, genresRelax },
                { btnSocial.Name, genresSocial },
                { btnAdrenaline.Name, genresAdrenaline },
                { btnFantasy.Name, genresFantasy }
            };
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UCMediaHome(EntitiesLayer.MediaCategory.Movie));
        }

        private void btnRelax_Click(object sender, RoutedEventArgs e)
        {
            ShowNextQuestion(sender);
        }

        private void btnAdrenaline_Click(object sender, RoutedEventArgs e)
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
            SavePreferredGenreChoice(button.Name);

            btnRelax.Visibility = Visibility.Collapsed;
            btnAdrenaline.Visibility = Visibility.Collapsed;
            btnFantasy.Visibility = Visibility.Collapsed;
            btnSocial.Visibility = Visibility.Collapsed;

            btnOld.Visibility = Visibility.Visible;
            btnNew.Visibility = Visibility.Visible;

        }

        private void SavePreferredGenreChoice(string btnName)
        {
            preferredGenres.AddRange(genresDict[btnName]);
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
            SavePreferredListsChoice(button.Name);

            btnOld.Visibility = Visibility.Collapsed;
            btnNew.Visibility = Visibility.Collapsed;

        }

        private void SavePreferredListsChoice(string btnName)
        {
            if (btnName == btnNew.Name)
            {
                preferredListCategories = listCategoryServices.GetMovieListCategories().Where(l => l.Title == "TODO").ToList();
            }
            else if (btnName == btnOld.Name)
            {
                preferredListCategories = listCategoryServices.GetMovieListCategories().Where(l => l.Title != "TODO").ToList();
            }
            
        }

        private async void RecommendMovies()
        {
            var movies = await recommendServices.RecommendMovies(preferredGenres, preferredListCategories);
            dgRecommendedMovies.ItemsSource = movies;

            if (movies.Count < 3)
                MessageBox.Show("Too few movies watched to give more recommendations");

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
