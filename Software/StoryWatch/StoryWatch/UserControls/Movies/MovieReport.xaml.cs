using BusinessLayer;
using EntitiesLayer.Entities;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StoryWatch.UserControls.Movies
{
    /// <summary>
    /// Interaction logic for MovieReport.xaml
    /// </summary>
    public partial class MovieReport : Window
    {
        public MovieReport()
        {
            InitializeComponent();

            reportViewer.Load += ReportViewer_Load;

        }
        private bool _isReportViewerLoaded;

        private void ReportViewer_Load(object sender, EventArgs e)
        {
            if (!_isReportViewerLoaded)
            {
                List<Movie> favouriteMovies, allUserMovies;
                List<Genre> genres;

                FetchDataForReportDatasets(out favouriteMovies, out allUserMovies, out genres);
                SetReportDataSources(favouriteMovies, allUserMovies, genres);

                _isReportViewerLoaded = true;
            }
        }

        private static void FetchDataForReportDatasets(out List<Movie> favouriteMovies, out List<Movie> allUserMovies, out List<Genre> genres)
        {
            var movieServices = new MovieServices();
            var listCategoryServices = new ListCategoryServices();
            var genreServices = new GenreServices();

            var userLists = listCategoryServices.GetMovieListCategoriesForUser(StateManager.LoggedUser);

            favouriteMovies = new List<Movie>();
            allUserMovies = new List<Movie>();

            foreach (var list in userLists)
            {
                allUserMovies.AddRange(movieServices.GetMoviesForList(list, StateManager.LoggedUser));

                if (list.Title == "Favorites")
                    favouriteMovies = movieServices.GetMoviesForList(list, StateManager.LoggedUser);
            }

            genres = genreServices.GetGenresForUser(StateManager.LoggedUser).ToList();
        }

        private void SetReportDataSources(List<Movie> favouriteMovies, List<Movie> allUserMovies, List<Genre> genres)
        {
            reportViewer.LocalReport.DataSources.Clear();
            var dataSource = new ReportDataSource() { Name = "MyFavouriteMovies", Value = favouriteMovies };
            reportViewer.LocalReport.DataSources.Add(dataSource);
            var dataSource2 = new ReportDataSource() { Name = "Genres", Value = genres };
            reportViewer.LocalReport.DataSources.Add(dataSource2);
            var dataSource3 = new ReportDataSource() { Name = "UserMovies", Value = allUserMovies };
            reportViewer.LocalReport.DataSources.Add(dataSource3);
            string path = "UserControls/Movies/Reports/ReportMovies.rdlc";
            reportViewer.LocalReport.ReportPath = path;
            reportViewer.Refresh();
            reportViewer.RefreshReport();
        }
    }
    
}
