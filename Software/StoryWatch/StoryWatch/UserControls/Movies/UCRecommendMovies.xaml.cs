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

        private List<GenreTMDB> genresTMDB = new List<GenreTMDB>();

        private List<string> genreNamesRelax = new List<string>();
        private List<string> genreNamesSocial = new List<string>();
        private List<string> genreNamesAdrenaline = new List<string>();
        private List<string> genreNamesFantasy = new List<string>();


        private Dictionary<string, List<GenreTMDB>> genresDict;

        private RecommendServices recommendServices = new RecommendServices();
        private MovieServices movieServices = new MovieServices();
        private ListCategoryServices listCategoryServices = new ListCategoryServices();


        public UCRecommendMovies()
        {
            InitializeComponent();
            FillGenres();
        }

        private async void FillGenres()
        {
            await GetGenresTMDBAsync();

            genreNamesRelax.AddRange(new List<string>
            {
                "Animation", "Comedy", "Documentary", "Family", "History", "Music", "Romance"
            });
            genreNamesSocial.AddRange(new List<string>
            {
                "Crime", "Documentary", "Drama", "Family", "TV Movie", "War", "Mystery"
            });
            genreNamesFantasy.AddRange(new List<string>
            {
                "Fantasy", "Science Fiction", "Adventure", "Western"
            });
            genreNamesAdrenaline.AddRange(new List<string>
            {
                "Action", "Adventure", "Crime", "Horror", "Thriller", "War"
            });

            var genresRelax = genresTMDB.Where(g => genreNamesRelax.Exists(n => n == g.Name)).ToList();
            var genresAdrenaline = genresTMDB.Where(g => genreNamesAdrenaline.Exists(n => n == g.Name)).ToList();
            var genresSocial = genresTMDB.Where(g => genreNamesSocial.Exists(n => n == g.Name)).ToList();
            var genresFantasy = genresTMDB.Where(g => genreNamesFantasy.Exists(n => n == g.Name)).ToList();

            genresDict = new Dictionary<string, List<GenreTMDB>>()
            {
                { btnRelax.Name, genresRelax },
                { btnSocial.Name, genresSocial },
                { btnAdrenaline.Name, genresAdrenaline },
                { btnFantasy.Name, genresFantasy }
            };

        }

        private async Task GetGenresTMDBAsync()
        {
            genresTMDB = await movieServices.GetTMDBGenresAsync();
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
            btnEither.Visibility = Visibility.Visible;

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
            btnEither.Visibility = Visibility.Collapsed;

        }

        private void SavePreferredListsChoice(string btnName)
        {
            if (btnName == btnOld.Name)
            {
                preferredListCategories = listCategoryServices.GetMovieListCategories().Where(l => l.Title == "TODO").ToList();
            }
            else if (btnName == btnNew.Name)
            {
                preferredListCategories = listCategoryServices.GetMovieListCategories().Where(l => l.Title != "TODO").ToList();
            }
            else if (btnName == btnEither.Name)
            {
                preferredListCategories = listCategoryServices.GetMovieListCategories();
            }
        }

        [Obsolete]
        private async void RecommendMovies()
        {
            /*string msg = "";
            foreach (var list in preferredListCategories) msg += list.Title + " ";
            foreach (var g in preferredGenres) msg += g.Name + " ";
            MessageBox.Show(msg);*/


            var movies = await recommendServices.RecommendMovies(preferredGenres, preferredListCategories);
            dgRecommendedMovies.ItemsSource = movies;

            var credit = await movieServices.GetMovieCreditsAsync(movies[0].Id);
            var director = credit.Crew.Where(c => c.Job == "Director").Select(c => c.Name).SingleOrDefault();
            var mainCast = credit.Cast.Take(credit.Cast.Count < 5 ? credit.Cast.Count : 5).Select(c => c.Name).ToList();
            string cast = "";
            foreach (var c in mainCast) cast += c + " ";
            MessageBox.Show("DIRECTOR: " + director + " CAST:" + cast);

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
