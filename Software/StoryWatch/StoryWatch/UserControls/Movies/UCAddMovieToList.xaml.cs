using EntitiesLayer.Entities;
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

namespace StoryWatch.UserControls.Movies
{
    /// <summary>
    /// Interaction logic for UCAddMovieToList.xaml
    /// </summary>
    public partial class UCAddMovieToList : UserControl
    {
        public IListCategory listCategory { get; set; }
        public Movie SelectedMovieFromSearchTMDB { get; set; }

        public UCAddMovieToList(IListCategory listCategory)
        {
            InitializeComponent();
            this.listCategory = listCategory;
        }

        private void AddMovie(object sender, RoutedEventArgs e)
        {
            //validacija txtboxova


            //sastavi i posalji movieServices.AddMovieToList:

            /*new MovieListItem
            {
                Id_Users = StateManager.LoggedUser.Id,
                Id_Movies = ,
                Id_MovieListCategories = this.listCategory.Id
            };*/
        }

        private void SearchTMDb(object sender, RoutedEventArgs e)
        {
            //open search form, when search form closes, if any movie chosen, get info and fill textboxes
            //otherwise do nothing
            GuiManager.OpenContent(new UCSearchMovie(this));
        }


    }
}
