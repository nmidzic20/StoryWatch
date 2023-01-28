﻿using BusinessLayer;
using EntitiesLayer.Entities;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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

        public AddGame(IListCategory lc, Game game, bool update = false, int id = 0)
        {
            InitializeComponent();

            gameServices = new GameServices();
            genreServices = new GenreServices();
            listCategory = lc;
            selectedGame = game;
            this.update = update;
            
            if (update)
            {
                btnSave.Content = "Update";
            }

            FillGameInfo();
        }

        private void FillGameInfo()
        {
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
    }
}
