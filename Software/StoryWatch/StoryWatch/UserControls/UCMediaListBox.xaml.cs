using BusinessLayer;
using EntitiesLayer;
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
        public MediaListBox(string title, string colorString)
        {
            InitializeComponent();

            lblTitle.Content = title;

            Color color = (Color)ColorConverter.ConvertFromString(colorString);
            header.Background = new SolidColorBrush(color);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UCAddMedia());
        }

        private async void lbMedia_SelectionChangedAsync(object sender, SelectionChangedEventArgs e)
        {
            if (StateManager.CurrentMediaCategory == MediaCategory.Movie)
            {
                var movieServices = new MovieServices();
                var movie = await movieServices.GetMovieInfoAsync(0);
                MessageBox.Show(movie.Title + " " + movie.Tagline + " ");
            }
        }
    }
}
