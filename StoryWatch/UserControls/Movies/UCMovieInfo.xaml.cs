using BusinessLayer;
using EntitiesLayer.Entities;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
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
    /// Interaction logic for MovieInfo.xaml
    /// Author: Noa Midžić
    /// </summary>
    public partial class MovieInfo : UserControl
    {

        public Movie movie { get; set; }
        public MovieInfo(Movie movie)
        {
            InitializeComponent();

            this.movie = movie;

            var movieServices = new MovieServices();
            movie.Genre = movieServices.GetMovieById(movie.Id).Genre;

            this.DataContext = movie;

            if (string.IsNullOrEmpty(movie.Countries)) txtCountries.Visibility = Visibility.Hidden;
            if (string.IsNullOrEmpty(movie.ReleaseDate)) txtReleaseDate.Visibility = Visibility.Hidden;
            if (string.IsNullOrEmpty(movie.Genre.Name)) txtGenre.Visibility = Visibility.Hidden;

        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string htmlBeginning = "<!DOCTYPE html>" +
                                    "<html>" +
                                    "<head>" +
                                        "<meta charset=\"utf-8\" />" +
                                        "<title>Test Title</title>" +
                                    "</head>" +
                                    "<body>";

            string trailer = "";

            if (!string.IsNullOrEmpty(movie.Trailer_URL))
                trailer =
                    "<iframe width=\"560\" height=\"315\" src=\"https://www.youtube.com/embed/"
                    + movie.Trailer_URL
                    + "\" title=\"YouTube video player\" frameborder=\"0\" allow=\"accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share\" allowfullscreen></iframe>";
            else
                trailer = "<div style='margin:20px'>No trailer URLs were provided for this movie - try updating this movie by searching for it on TMDb or manually provide trailer URL</div>";
            string htmlEnd = "</body>" +
                            "</html>";

            string html = htmlBeginning + trailer + htmlEnd;

            try
            {
                //custom defined user data folder - if left out, WebView2 will attempt to create
                //default user data folder in ProgramFiles folder, which will result in error due to
                //this folder requiring elevated privileges
                string userDataFolder = "C:\\StoryWatchUserDataFolder";
                var env = await CoreWebView2Environment.CreateAsync(null, userDataFolder, null);
                await webView2.EnsureCoreWebView2Async(env);
                webView2.CoreWebView2.NavigateToString(html);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.Source + "\n\n" + ex.StackTrace);
            }
        }

        private void WebView2_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            if (e.IsSuccess)
            {
                ((WebView2)sender).ExecuteScriptAsync("document.querySelector('body').style.overflow='hidden'");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.CloseContent();
        }
    }
}
