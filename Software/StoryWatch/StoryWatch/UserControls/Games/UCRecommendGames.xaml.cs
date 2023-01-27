using BusinessLayer;
using EntitiesLayer.Entities;
using Newtonsoft.Json.Linq;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

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

            foreach (var game in gameServices.GetAllGames())
            {
                var gameIgdb = await gameServices.GetGameInfoWithGenreIds(int.Parse(game.IGDB_Id));
                int[] ids = gameIgdb.Genres.Ids.Select(i => (int)i).ToArray();
                
                recommendedGames.AddRange(await recommendServices.RecommendGames(ids));
            }

            dgResults.ItemsSource = recommendedGames;
        }

        private async void Back_Clicked(object sender, RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UCMediaHome(EntitiesLayer.MediaCategory.Game));
        }

        private async void Get_Clicked(object sender, RoutedEventArgs e)
        {
            await PopulateDg();
        }
    }
}
