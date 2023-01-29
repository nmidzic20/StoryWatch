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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StoryWatch.UserControls.Games
{
    /// <summary>
    /// Interaction logic for GameReport.xaml
    /// Author: Hrvoje Lukšić
    /// </summary>
    public partial class GameReport : Window
    {
        private bool _isReportViewerLoaded;

        public GameReport()
        {
            InitializeComponent();
            reportViewer.Load += ReportViewer_Load;
        }

        private void ReportViewer_Load(object sender, EventArgs e)
        {
            if (!_isReportViewerLoaded)
            {
                List<Game> favoriteGames, allUserGames;
                List<Genre> genres;
                List<Genre> gameLists = new List<Genre>();


                FetchDataForReportDatasets(out favoriteGames, out allUserGames, out genres, out gameLists);
                SetReportDataSources(favoriteGames, allUserGames, genres, gameLists);

                _isReportViewerLoaded = true;
            }
        }

        private static void FetchDataForReportDatasets(out List<Game> favoriteGames, out List<Game> allUserGames, out List<Genre> genres, out List<Genre> gameLists)
        {
            var gameServices = new GameServices();
            var listCategoryServices = new ListCategoryServices();
            var genreServices = new GenreServices();

            var userLists = listCategoryServices.GetGameListCategoriesForUser(StateManager.LoggedUser);

            favoriteGames = new List<Game>();
            allUserGames = new List<Game>();

            foreach (var list in userLists)
            {
                allUserGames.AddRange(gameServices.GetGamesForList(list, StateManager.LoggedUser));

                if (list.Title == "Favorites")
                    favoriteGames = gameServices.GetGamesForList(list, StateManager.LoggedUser);
            }

            // using the Genre class here because it's convenient for storing the number of games and name of list
            List <Genre> lists = new List<Genre>();
            
            lists.AddRange(listCategoryServices.GetGameListCategoriesForUser(StateManager.LoggedUser)
                .ToList()
                .Where(x => x.GameListItems.Count() > 0)
                .Select(x => new Genre() 
                { 
                    Name = x.Title, 
                    Id = x.GameListItems.Count() 
                }));
            
            gameLists = lists;
            genres = genreServices.GetGameGenresForUser(StateManager.LoggedUser).ToList();
        }

        private void SetReportDataSources(List<Game> favoriteGames, List<Game> allUserGames, List<Genre> genres, List<Genre> gameLists)
        {
            reportViewer.LocalReport.DataSources.Clear();
            var dataSource = new ReportDataSource() { Name = "MyFavoriteGames", Value = favoriteGames };
            
            reportViewer.LocalReport.DataSources.Add(dataSource);
            var dataSource2 = new ReportDataSource() { Name = "Genres", Value = genres };
            
            reportViewer.LocalReport.DataSources.Add(dataSource2);
            var dataSource3 = new ReportDataSource() { Name = "UserGames", Value = allUserGames };
            
            reportViewer.LocalReport.DataSources.Add(dataSource3);
            var dataSource4 = new ReportDataSource() { Name = "GameLists", Value = gameLists };
            
            reportViewer.LocalReport.DataSources.Add(dataSource4);
            string path = "UserControls/Games/Reports/ReportGames.rdlc";
            
            reportViewer.LocalReport.ReportPath = path;
            reportViewer.Refresh();
            reportViewer.RefreshReport();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                System.Diagnostics.Process.Start(@"PDF\\GameReport.pdf");
            }
        }
    }
}
