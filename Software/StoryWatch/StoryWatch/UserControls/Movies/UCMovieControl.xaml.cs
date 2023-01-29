using System;
using System.Collections.Generic;
using System.Globalization;
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
using TMDbLib.Objects.Search;

namespace StoryWatch.UserControls.Movies
{
    /// <summary>
    /// Interaction logic for UCMovieControl.xaml
    /// </summary>
    public partial class UCMovieControl : UserControl
    {
        private SearchMovie movie;
        public UCMovieControl(SearchMovie movie)
        {
            InitializeComponent();
            this.movie = movie;
            this.DataContext = movie;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var windowChooseList = new ChooseListForMovie(movie);
            windowChooseList.ShowDialog();
        }
    }

  
}
