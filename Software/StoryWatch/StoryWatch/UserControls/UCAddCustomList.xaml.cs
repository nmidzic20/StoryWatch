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
using EntitiesLayer;
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
            bool isSuccessful;
            string color = clpck.SelectedColor.ToString();

            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("List name cannot be empty!");
                return;
            }

            if (String.IsNullOrEmpty(color))
                color = "#FFFFFF";

            switch (StateManager.CurrentMediaCategory)
            {
                case MediaCategory.Movie:

                    var allMovieLists = listCategoryServices.GetMovieListCategories();
                    var listMovieId = (allMovieLists.Count() != 0) ? allMovieLists.Last().Id + 1 : 0;

                    isSuccessful = listCategoryServices.AddMovieListCategory(
                        new MovieListCategory
                        {
                            Id = listMovieId,
                            Title = txtName.Text,
                            Color = color
                        },
                        StateManager.LoggedUser
                        );

                    if (isSuccessful == false)
                    {
                        MessageBox.Show("Custom list was not added!");
                    }

                    GuiManager.OpenContent(new UCMediaHome(MediaCategory.Movie));
                    break;

                case MediaCategory.Book:

                    var allBookLists = listCategoryServices.GetMovieListCategories();
                    var listBookId = (allBookLists.Count() != 0) ? allBookLists.Last().Id + 1 : 0;

                    isSuccessful = listCategoryServices.AddBookListCategory(
                        new BookListCategory
                        {
                            Id = listBookId,
                            Title = txtName.Text,
                            Color = color
                        },
                        StateManager.LoggedUser
                        );

                    if (isSuccessful == false)
                    {
                        MessageBox.Show("Custom list was not added!");
                    }

                    GuiManager.OpenContent(new UCMediaHome(MediaCategory.Book));
                    break;

                case MediaCategory.Game:

                    var allGameLists = listCategoryServices.GetMovieListCategories();
                    var gameListId = (allGameLists.Count() != 0) ? allGameLists.Last().Id + 1 : 0;

                    isSuccessful = listCategoryServices.AddGameListCategory(
                        new GameListCategory
                        {
                            Id = gameListId,
                            Title = txtName.Text,
                            Color = color
                        },
                        StateManager.LoggedUser
                        );

                    if (isSuccessful == false)
                    {
                        MessageBox.Show("Custom list was not added!");
                    }

                    GuiManager.OpenContent(new UCMediaHome(MediaCategory.Game));
                    break;

            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.CloseContent();
        }

    }
}
