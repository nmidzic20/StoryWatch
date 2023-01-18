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
        private string placeholderTextCollection = "Search movies by franchise";
        private string placeholderTextKeyword = "Search movies by keyword";

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
            if (txtResults == null || 
                string.IsNullOrEmpty(txtSearchKeyword.Text) || 
                txtSearchKeyword.Text == placeholderTextKeyword) 
                    
                return;

            var movieServices = new MovieServices();

            var movies = movieServices.SearchMoviesByKeyword(txtSearchKeyword.Text);

            if (movies == null)
            {
                txtResults.Text = "";
                return;
            }

            var results = movies.Select(r => r.Title);
            var text = "";

            foreach (var r in results)
                text += r + "\n";

            txtResults.Text = text;
        }

        private void txtSearchCollection_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtResults == null ||
                string.IsNullOrEmpty(txtSearchCollection.Text) ||
                txtSearchKeyword.Text == placeholderTextCollection)

                return;

            var movieServices = new MovieServices();

            var movies = movieServices.SearchMoviesByCollection(txtSearchCollection.Text);

            if (movies == null)
            {
                txtResults.Text = "";
                return;
            }

            var results = movies.Select(r => r.Title);
            var text = "";

            foreach (var r in results)
                text += r + "\n";

            txtResults.Text = text;
        }
    }
}
