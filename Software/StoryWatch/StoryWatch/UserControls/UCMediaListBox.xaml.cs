using BusinessLayer;
using EntitiesLayer;
using EntitiesLayer.Entities;
using StoryWatch.UserControls.Books;
using StoryWatch.UserControls.Games;
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

namespace StoryWatch.UserControls
{
    /// <summary>
    /// Interaction logic for MediaListBox.xaml
    /// </summary>
    public partial class MediaListBox : UserControl
    {
        string Title;
        public MediaListBox(string title, string colorString)
        {
            InitializeComponent();

            lblTitle.Content = title;
            Title = title;

            Color color = (Color)ColorConverter.ConvertFromString(colorString);
            header.Background = new SolidColorBrush(color);
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            loadUserControl();
        }

        private void loadUserControl()
        {
            if (StateManager.CurrentMediaCategory == MediaCategory.Book)
            {
                ListCategoryServices listCategoryServices = new ListCategoryServices();
                List<Book> book = listCategoryServices.GetAllBooksForCurrentCategory(Title);
                foreach (var b in book)
                {
                    lbMedia.Items.Add(b.Title);
                }
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (StateManager.CurrentMediaCategory == MediaCategory.Movie)
            {
                GuiManager.OpenContent(new UCAddMedia());
            }
            else if (StateManager.CurrentMediaCategory == MediaCategory.Book)
            {
                GuiManager.OpenContent(new UCAddBook(Title));
            }
            else
            {
                GuiManager.OpenContent(new UCAddGame());
            }

        }

        private async void lbMedia_SelectionChangedAsync(object sender, SelectionChangedEventArgs e)
        {
            if (StateManager.CurrentMediaCategory == MediaCategory.Movie)
            {
                /*var movieServices = new MovieServices();
                var movie = await movieServices.GetMovieInfoAsync(0);
                MessageBox.Show(movie.Title + " " + movie.Tagline + " ");*/
            }
           else if(StateManager.CurrentMediaCategory == MediaCategory.Book)
            {

            }
        }
    }
}
