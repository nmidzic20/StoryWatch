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
using BusinessLayer;
using EntitiesLayer.Entities;

namespace StoryWatch.UserControls
{
    /// <summary>
    /// Interaction logic for UCAddCustomList.xaml
    /// </summary>
    public partial class UCAddCustomList : UserControl
    {
        private ListCategoryServices listCategoryServices = new ListCategoryServices();
        public UCAddCustomList()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            switch (StateManager.CurrentMediaCategory)
            {
                case MediaCategory.Movie:

                    bool isSuccessful = listCategoryServices.AddMovieListCategory(
                        new MovieListCategory
                        {
                            Id = listCategoryServices.GetMovieListCategories().Count,
                            Title = txtName.Text,
                            Color = Colors.BlanchedAlmond.ToString()
                        }
                        );

                    if (isSuccessful == false)
                    {
                        MessageBox.Show("Custom list was not added!");
                    }

                    GuiManager.CloseContent();
                    break;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
