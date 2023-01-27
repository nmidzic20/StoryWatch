using BusinessLayer;
using EntitiesLayer.Entities;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace StoryWatch.UserControls.Games
{
    /// <summary>
    /// Interaction logic for AddGame.xaml
    /// </summary>
    public partial class AddGame : Window
    {
        public IListCategory listCategory { get; set; }
        private GameServices gameServices;
        private Game selectedGame = null;
        private bool update = false;

        public AddGame(IListCategory lc, Game game, bool update = false, int id = 0)
        {
            InitializeComponent();

            gameServices = new GameServices();
            listCategory = lc;
            selectedGame = game;
            this.update = update;
            
            if (update)
            {
                btnSave.Content = "Update";
            }

            FillGameInfo();

            //TODO - when btn pressed, call movieServices.UpdateMovie -> repo.Update
        }

        private void FillGameInfo()
        {
            txtID.Text = selectedGame.IGDB_Id;
            txtTitle.Text = selectedGame.Title;
            txtGenres.Text = selectedGame.Genres;
            txtSummary.Text = selectedGame.Summary;
            datePicker.Text = selectedGame.Release_Date;
            txtDev.Text = selectedGame.Company;
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
            var game = new EntitiesLayer.Entities.Game
            {
                Id = selectedGame.Id,
                Title = txtTitle.Text,
                Summary = txtSummary.Text,
                Genres = txtGenres.Text,
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

        private void AddGameToList()
        {
            var allGames = gameServices.GetAllGames();
            var gameId = (allGames.Count() != 0) ? allGames.Last().Id + 1 : 0;

            bool isSuccessful = gameServices.AddGame(new EntitiesLayer.Entities.Game
            {
                Id = gameId,
                Title = txtTitle.Text,
                Summary = txtSummary.Text,
                IGDB_Id = txtID.Text,
                Company = txtDev.Text,
                Genres = txtGenres.Text,
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
