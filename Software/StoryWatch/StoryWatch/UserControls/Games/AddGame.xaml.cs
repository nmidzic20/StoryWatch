using BusinessLayer;
using EntitiesLayer.Entities;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TMDbLib.Objects.Movies;

namespace StoryWatch.UserControls.Games
{
    /// <summary>
    /// Interaction logic for AddGame.xaml
    /// Author: Hrvoje Lukšić
    /// </summary>
    public partial class AddGame : Window
    {
        public IListCategory listCategory { get; set; }
        private GameServices gameServices;
        public GenreServices genreServices;
        private Game selectedGame = null;
        private bool update = false;
        private bool getIGDBInfo = false;

        public AddGame(IListCategory lc, Game game, bool update = false, bool getIGDBInfo = false)
        {
            InitializeComponent();

            gameServices = new GameServices();
            genreServices = new GenreServices();
            listCategory = lc;
            selectedGame = game;
            this.getIGDBInfo = getIGDBInfo;
            this.update = update;
            
            if (update)
            {
                btnSave.Content = "Update";
            }

            FormSetup();
        }

        private async void FormSetup()
        {
            if (selectedGame == null)
            {
                datePicker.IsEnabled = true;
                return;
            }
            
            if (getIGDBInfo)
            {
                var gameIGDB = await gameServices.GetGameInfoAsync(int.Parse(selectedGame.IGDB_Id));
                var companies = gameIGDB.InvolvedCompanies == null ? "Indie" : gameIGDB.InvolvedCompanies.Values
                    .Aggregate("", (current, company) => current + (company.Company.Value.Name + ", "));

                companies.Remove(companies.Length - 1, 1);

                txtID.Text = gameIGDB.Id.ToString();
                txtTitle.Text = gameIGDB.Name;
                txtSummary.Text = gameIGDB.Summary;
                datePicker.Text = gameIGDB.FirstReleaseDate.ToString();
                txtDev.Text = companies;
                txtGenres.Text = gameIGDB.Genres.Values.First().Name;

                txtID.IsEnabled = false;
                txtTitle.IsEnabled = false;
                txtSummary.IsEnabled = false;
                datePicker.IsEnabled = false;
                txtDev.IsEnabled = false;
                txtGenres.IsEnabled = false;

                btnSave.Visibility = Visibility.Hidden;

                return;
            }

            txtID.Text = selectedGame.IGDB_Id;
            txtTitle.Text = selectedGame.Title;
            txtSummary.Text = selectedGame.Summary;
            datePicker.Text = selectedGame.Release_Date;
            txtDev.Text = selectedGame.Company;

            if (update)
            {
                Game game = gameServices.GetGameByTitle(selectedGame.Title);
                txtGenres.Text = game.Genre.Name;
            }
        }

        private void AddGameToList(object sender, RoutedEventArgs e)
        {
            if (!InputValid()) return;

            if (update)
            {
                UpdateGame();
            }
            else
            {
                AddGameToList();
            }
        }

        private void UpdateGame()
        {
            var oldGenre = gameServices.GetGameByTitle(selectedGame.Title).Genre;
            var genre = UpdateGenre(oldGenre);

            var game = new EntitiesLayer.Entities.Game
            {
                Id = selectedGame.Id,
                Title = txtTitle.Text,
                Summary = txtSummary.Text,
                Genre = genre,
                Company = txtDev.Text,
                IGDB_Id = selectedGame.IGDB_Id,
            };

            int isSuccessful = gameServices.UpdateGame(game);
            
            if (isSuccessful != 0)
            {
                GuiManager.OpenContent(new UCMediaHome(EntitiesLayer.MediaCategory.Game));
                Close();
            }
            else
            {
                MessageBox.Show("Update failed");
            }
        }

        private Genre UpdateGenre(Genre oldGenre)
        {
            int genreId = (genreServices.GetAllGenres().LastOrDefault() != null) ? genreServices.GetAllGenres().Last().Id + 1 : 0;
            
            var newGenre = new Genre
            {
                Id = genreId,
                Name = txtGenres.Text
            };
            
            return genreServices.UpdateGameGenre(oldGenre, newGenre);
        }

        private void AddGameToList()
        {
            var allGames = gameServices.GetAllGames();
            var gameId = (allGames.Count() != 0) ? allGames.Last().Id + 1 : 0;

            Genre genre = null;
            
            if (!string.IsNullOrEmpty(txtGenres.Text))
            {
                var genreServices = new GenreServices();
                int genreId = (genreServices.GetAllGenres().LastOrDefault() != null) ? genreServices.GetAllGenres().Last().Id + 1 : 0;
                
                genre = new Genre
                {
                    Id = genreId,
                    Name = txtGenres.Text
                };
                
                genreId = genreServices.AddGenre(genre).Id;
                genre = genreServices.GetGenreById(genreId);
            }

            bool isSuccessful = gameServices.AddGame(new EntitiesLayer.Entities.Game
            {
                Id = gameId,
                Title = txtTitle.Text,
                Summary = txtSummary.Text,
                IGDB_Id = txtID.Text,
                Company = txtDev.Text,
                Genre = genre,
                Release_Date = datePicker.Text
            });

            if (!isSuccessful)
            {
                gameId = gameServices.GetGameByIGDBId(txtID.Text).Id;
            }

            GameListItem game = new GameListItem
            {
                Id_Users = StateManager.LoggedUser.Id,
                Id_Games = gameId,
                Id_GameListCategories = this.listCategory.Id
            };
            
            bool gameNotInList = gameServices.AddGameToList(game, listCategory as GameListCategory, StateManager.LoggedUser);

            if (!gameNotInList)
            {
                MessageBox.Show("This game is already added to this list");
            }
            else
            {
                GuiManager.OpenContent(new UCMediaHome(EntitiesLayer.MediaCategory.Game));
                Close();
            }
        }

        private bool InputValid()
        {
            return !string.IsNullOrEmpty(txtTitle.Text);
        }

        private void TextTitleChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTitle.Text))
            {
                btnSave.IsEnabled = true;
            }
        }

        private void BtnCancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void WebView2_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            if (e.IsSuccess)
            {
                ((WebView2)sender).ExecuteScriptAsync("document.querySelector('body').style.overflow='hidden'");
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IGDB.IdentitiesOrValues<IGDB.Models.GameVideo> videos = null;

            if (selectedGame != null && !string.IsNullOrEmpty(selectedGame.IGDB_Id))
            {
                videos = (await gameServices.GetGameInfoAsync(int.Parse(selectedGame.IGDB_Id))).Videos;
            }
            
            string url = "";

            if (videos != null)
            {
                url = videos.Values.First().VideoId;
            }


            string htmlBeginning = "<!DOCTYPE html>" +
                                    "<html>" +
                                    "<head>" +
                                        "<meta charset=\"utf-8\" />" +
                                        "<title>Test Title</title>" +
                                    "</head>" +
                                    "<body>";
            string trailer = "";

            if (!string.IsNullOrEmpty(url))
                trailer =
                    "<iframe width=\"560\" height=\"315\" src=\"https://www.youtube.com/embed/"
                    + url
                    + "\" title=\"YouTube video player\" frameborder=\"0\" allow=\"accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share\" allowfullscreen></iframe>";
            else
                trailer = "<div style='margin:20px; text-align:center'>No trailer URLs were provided for this game</div>";
            string htmlEnd = "</body>" +
                            "</html>";

            string html = htmlBeginning + trailer + htmlEnd;

            var env = await CoreWebView2Environment.CreateAsync();
            await webView2.EnsureCoreWebView2Async(env);
            webView2.CoreWebView2.NavigateToString(html);
        }
    }
}
