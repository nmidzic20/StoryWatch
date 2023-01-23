using BusinessLayer;
using EntitiesLayer;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace StoryWatch.UserControls.Games
{
    /// <summary>
    /// Interaction logic for UCAddGame.xaml
    /// </summary>
    public partial class UCAddGame : UserControl
    {
        private GameServices gameServices;
        
        private readonly string placeholderTextKeyword = "Search games by keyword";

        public UCAddGame()
        {
            InitializeComponent();
            gameServices = new GameServices();
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

            //foreach (var game in games)
            //{
            //    lbResults.Items.Add(game.Name + delimiter + game.Id);
            //}
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

        private void btnBooksHome_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UCMediaHome(MediaCategory.Game));
        }
    }
}
