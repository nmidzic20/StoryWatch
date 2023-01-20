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

namespace StoryWatch.UserControls.Books
{
    /// <summary>
    /// Interaction logic for UCAddBook.xaml
    /// </summary>
    public partial class UCAddBook : UserControl
    {
        private ListCategoryServices listCategoryServices = new ListCategoryServices();
        private BookService bookServices = new BookService();

        private string placeholderTextKeyword = "Search movies by keyword";
        private string delimiter = " | ID: ";

        public UCAddBook()
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

            var movies = bookServices.GetBookTitleAsync(txtSearchKeyword.Text);

            if (movies == null)
            {
                return;
            }

            //foreach (var m in movies)
                lbResults.Items.Add(movies);

        }

        private async void lbResults_SelectionChangedAsync(object sender, SelectionChangedEventArgs e)
        {
            /*if (e.AddedItems.Count == 0) return;

            var item = e.AddedItems[0] as string;
            var id = item.Split(new string[] { delimiter }, StringSplitOptions.RemoveEmptyEntries)[1];

            int idInt;
            Int32.TryParse(id, out idInt);

            string movieInfo = "";
            TMDbLib.Objects.Movies.Movie movie = await movieServices.GetMovieInfoAsync(idInt);

            string urlYoutube = "https://www.youtube.com/watch?v=";
            string trailerURL = urlYoutube + movie.Videos.Results[0].Key;
            movieInfo += movie.Title + " " + movie.Homepage + " " + movie.Genres[0].Name + " "
                + movie.Runtime + " " + movie.BackdropPath + " " + trailerURL;

            MessageBox.Show("TODO add movie info into textboxes " + id + " " + movieInfo);*/
        }
    }
}
