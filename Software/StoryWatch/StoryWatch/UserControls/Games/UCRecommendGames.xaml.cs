using BusinessLayer;
using EntitiesLayer.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace StoryWatch.UserControls.Games
{
    /// <summary>
    /// Interaction logic for UCRecommendGames.xaml
    /// </summary>
    public partial class UCRecommendGames : UserControl
    {
        private GameServices gameServices;
        private RecommendServices recommendServices;
        
        public UCRecommendGames()
        {
            InitializeComponent();
            gameServices = new GameServices();
            recommendServices = new RecommendServices();
        }
        
        private async Task PopulateDg()
        {
            List<IGDB.Models.Game> recommendedGames = new List<IGDB.Models.Game>();
            var allGames = gameServices.GetAllGames();
            recommendedGames.Clear();
            
            if (allGames.Count == 0)
            {
                dgResults.ItemsSource = await recommendServices.RecommendBestGames();
                return;
            }

            foreach (var game in gameServices.GetAllGames())
            {
                var gameIgdb = await gameServices.GetGameInfoAsync(int.Parse(game.IGDB_Id));
                int[] ids = gameIgdb.SimilarGames.Ids.Select(i => (int)i).ToArray();
                
                recommendedGames.AddRange(await recommendServices.RecommendGames(ids));
            }
            dgResults.ItemsSource = recommendedGames;
        }

        private void Back_Clicked(object sender, RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UCMediaHome(EntitiesLayer.MediaCategory.Game));
        }

        private async void Get_Clicked(object sender, RoutedEventArgs e)
        {
            await PopulateDg();
        }
    }
}
