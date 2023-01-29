﻿using BusinessLayer;
using EntitiesLayer;
using EntitiesLayer.Entities;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TMDbLib.Utilities;

namespace StoryWatch.UserControls.Games
{
    /// <summary>
    /// Interaction logic for UCAddGame.xaml
    /// Author: Hrvoje Lukšić
    /// </summary>
    public partial class UCAddGame : UserControl
    {
        private GameServices gameServices;
        private IListCategory listCategory;
        private readonly string placeholderTextKeyword = "Search games by keyword";

        public UCAddGame(IListCategory lc)
        {
            InitializeComponent();
            gameServices = new GameServices();
            listCategory = lc;
        }

        private async void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (dgResults == null ||
                string.IsNullOrEmpty(txtSearchKeyword.Text) ||
                txtSearchKeyword.Text == placeholderTextKeyword)
            {
                return;
            }

            var games = await gameServices.SearchGamesAsync(txtSearchKeyword.Text);

            games.ToList().Clear();

            if (games == null)
            {
                return;
            }

            dgResults.ItemsSource = games;
        }

        private void txtSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox txtSearch = sender as TextBox;
            txtSearch.Text = "";
            txtSearch.FontStyle = FontStyles.Normal;
            txtSearch.FontWeight = FontWeights.Normal;
            txtSearch.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void txtSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox txtSearch = sender as TextBox;

            txtSearch.FontStyle = FontStyles.Italic;
            txtSearch.FontWeight = FontWeights.Bold;
            txtSearch.Foreground = new SolidColorBrush(Colors.SlateGray);
        }

        private void Back_Clicked(object sender, RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UCMediaHome(MediaCategory.Game));
        }

        public async void AddClicked(object sender, RoutedEventArgs e)
        {
            if (dgResults.SelectedItem == null)
            {
                return;
            }

            IGDB.Models.Game game = await gameServices.GetGameInfoAsync((int)(dgResults.SelectedItem as IGDB.Models.Game).Id);

            Genre selectedGameGenre = new Genre()
            {
                Name = game.Genres.Values.First().Name,
                Id = 0
            };

            var genres = game.InvolvedCompanies == null ? "Indie" : game.InvolvedCompanies.Values
                    .Aggregate("", (current, company) => current + (company.Company.Value.Name + ", "));

            genres.Remove(genres.Length - 1, 1);

            Game selectedGame = new Game()
            {
                IGDB_Id = game.Id.ToString(),
                Title = game.Name,
                Summary = game.Summary,
                Release_Date = game.FirstReleaseDate.ToString(),
                Company = genres,
                Genre = selectedGameGenre
            };

            var winAddGame = new AddGame(listCategory, selectedGame);
            winAddGame.txtGenres.Text = selectedGameGenre.Name;
            winAddGame.ShowDialog();
        }
    }
}
