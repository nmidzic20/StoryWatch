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
using TMDbLib.Objects.General;

namespace StoryWatch.UserControls
{
    /// <summary>
    /// Interaction logic for UCAddMedia.xaml
    /// </summary>
    public partial class UCAddMedia : UserControl
    {
        private ListCategoryServices listCategoryServices = new ListCategoryServices();
        private MovieServices movieServices = new MovieServices();

        private string placeholderTextCollection = "Search movies by franchise";
        private string placeholderTextKeyword = "Search movies by keyword";
        private string delimiter = " | ID: ";

        public UCAddMedia()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

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
            
            if (txtSearch.Name == "txtSearchKeyword")
                txtSearch.Text = placeholderTextKeyword;
            else if (txtSearch.Name == "txtSearchCollection")
                txtSearch.Text = placeholderTextCollection;

            txtSearch.FontStyle = FontStyles.Italic;
            txtSearch.FontWeight = FontWeights.Bold;
            txtSearch.Foreground = new SolidColorBrush(Colors.SlateGray);
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (lbResults == null || 
                string.IsNullOrEmpty(txtSearchKeyword.Text) || 
                txtSearchKeyword.Text == placeholderTextKeyword) 
                    
                return;

            lbResults.Items.Clear();

            var movies = movieServices.SearchMoviesByKeyword(txtSearchKeyword.Text);

            if (movies == null)
            {
                //txtResults.Text = "";
                return;
            }

            //var text = "";

            foreach (var m in movies)
                //text += r + "\n";
                lbResults.Items.Add(m.Title + delimiter + m.Id);

            //txtResults.Text = text;
        }

        private void txtSearchCollection_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (lbResults == null ||
                string.IsNullOrEmpty(txtSearchCollection.Text) ||
                txtSearchCollection.Text == placeholderTextCollection)

                return;

            lbResults.Items.Clear();

            var movies = movieServices.SearchMoviesByCollection(txtSearchCollection.Text);

            if (movies == null)
            {
                //txtResults.Text = "";
                return;
            }

            //var text = "";

            foreach (var m in movies)
                //text += r + "\n";
                lbResults.Items.Add(m.Title + delimiter + m.Id);

            //txtResults.Text = text;
        }

        private void lbResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;

            var item = e.AddedItems[0] as string;
            var id = item.Split(new string[] { delimiter }, StringSplitOptions.RemoveEmptyEntries)[1];
            
            int idInt;
            Int32.TryParse(id, out idInt);

            string movieInfo = "";
            TMDbLib.Objects.Movies.Movie movie = movieServices.GetMovieInfo(idInt);

            movieInfo += movie.Title + " " + movie.Homepage + " " + movie.Genres + " "
                + movie.Runtime + " " + movie.BackdropPath;

            MessageBox.Show("TODO add movie info into textboxes " + id + " " + movieInfo);
        }
    }
}
