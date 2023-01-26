using BusinessLayer;
using EntitiesLayer.Entities;
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
        private IGDB.Models.Game selectedGame = null;
        private int idToUpdate;
        private bool update = false;

        public AddGame(IListCategory lc, IGDB.Models.Game game, bool update = false, int id = 0)
        {
            InitializeComponent();

            gameServices = new GameServices();
            listCategory = lc;
            selectedGame = game;
            idToUpdate = id;
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
            txtTitle.Text = selectedGame.Name;
            //txtGenre.Text = movieToUpdate.Genres[0].Name;
            txtSummary.Text = selectedGame.Summary;
            //dtReleaseDate.Text = gameToUpdate.ReleaseDate;
            //txtCountry.Text = gameToUpdate.Countries;
            txtID.Text = selectedGame.Id.ToString();
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
                Id = idToUpdate,
                Title = txtTitle.Text,
                Summary = txtSummary.Text,
                IGDB_Id = selectedGame.Id.ToString(),
                Company = ""
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
                Company = txtDev.Text
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
