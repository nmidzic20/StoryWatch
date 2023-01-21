using BusinessLayer;
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
    /// Interaction logic for UCAddGame.xaml
    /// </summary>
    public partial class UCAddGame : UserControl
    {
        private GameServices gameServices;
        private readonly string delimiter = " | ID: ";

        public UCAddGame()
        {
            gameServices = new GameServices();
        }

        private async void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (lbResults == null ||
                string.IsNullOrEmpty(txtSearchKeyword.Text)/* ||
                txtSearchKeyword.Text == placeholderTextKeyword*/)

                return;

            lbResults.Items.Clear();

            var games = await gameServices.SearchGamesAsync(txtSearchKeyword.Text);

            if (games == null)
            {
                //txtResults.Text = "";
                return;
            }

            //var text = "";

            foreach (var game in games)
                //text += r + "\n";
                lbResults.Items.Add(game.Name + delimiter + game.Id);

            //txtResults.Text = text;
        }

        //private async void lbResults_SelectionChangedAsync(object sender, SelectionChangedEventArgs e)
        //{
        //    if (e.AddedItems.Count == 0) return;

        //    var item = e.AddedItems[0] as string;
        //    var id = item.Split(new string[] { delimiter }, StringSplitOptions.RemoveEmptyEntries)[1];

        //    int idInt;
        //    Int32.TryParse(id, out idInt);

        //    string movieInfo = "";
        //    TMDbLib.Objects.Movies.Movie movie = await movieServices.GetMovieInfoAsync(idInt);

        //    string urlYoutube = "https://www.youtube.com/watch?v=";
        //    string trailerURL = urlYoutube + movie.Videos.Results[0].Key;
        //    movieInfo += movie.Title + " " + movie.Homepage + " " + movie.Genres[0].Name + " "
        //        + movie.Runtime + " " + movie.BackdropPath + " " + trailerURL;

        //    MessageBox.Show("TODO add movie info into textboxes " + id + " " + movieInfo);
        //}
    }
}
