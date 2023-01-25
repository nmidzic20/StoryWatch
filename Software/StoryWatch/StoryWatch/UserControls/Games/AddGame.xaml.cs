using BusinessLayer;
using EntitiesLayer.Entities;
using IGDB.Models;
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
    /// Interaction logic for AddGame.xaml
    /// </summary>
    public partial class AddGame : Window
    {
        public IListCategory listCategory { get; set; }
        private GameServices gameServices;
        private EntitiesLayer.Entities.Game gameToUpdate = null;
        private bool update = false;

        public AddGame(IListCategory lc, EntitiesLayer.Entities.Game game, bool update = false)
        {
            InitializeComponent();
            gameServices = new GameServices();
            listCategory = lc;
            btnSave.Content = "Update";
            gameToUpdate = game;
            this.update = update;

            txtTitle.Text = gameToUpdate.Title;
            //txtGenre.Text = movieToUpdate.Genres[0].Name;
            txtSummary.Text = gameToUpdate.Summary;
            //dtReleaseDate.Text = gameToUpdate.ReleaseDate;
            //txtCountry.Text = gameToUpdate.Countries;
            txtID.Text = gameToUpdate.IGDB_Id;

            //TODO - when btn pressed, call movieServices.UpdateMovie -> repo.Update
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
                Id = gameToUpdate.Id,
                Title = txtTitle.Text,
                Summary = txtSummary.Text,
                IGDB_Id = gameToUpdate.IGDB_Id,
                Company = gameToUpdate.Company,
                Indie = gameToUpdate.Indie
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
                Indie = 0
            });

            if (!isSuccessful)
            {
                //MessageBox.Show("This movie is already added from TMDB to database");
                gameId = gameServices.GetGameByIGDBId(txtID.Text).Id;
            }

            GameListItem game = new GameListItem
            {
                Id_Users = StateManager.LoggedUser.Id,
                Id_Games = gameId,
                Id_GameListCategories = this.listCategory.Id
            };
            
            bool gameAddedToList = gameServices.AddGameToList(game, listCategory as GameListCategory, StateManager.LoggedUser);

            if (!gameAddedToList)
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

        //private void SearchTMDb(object sender, RoutedEventArgs e)
        //{
        //    //open search form, when search form closes, if any movie selected, it will fill textboxes in this UC
        //    //otherwise if no movie selected, will do nothing
        //    GuiManager.OpenContent(new UCSearchMovie(this));
        //}

        private void TextTitleChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTitle.Text))
            {
                btnSave.IsEnabled = true;
            }
        }

        private void Return(object sender, RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UCMediaHome(EntitiesLayer.MediaCategory.Game));
            Close();
        }
    }
}
