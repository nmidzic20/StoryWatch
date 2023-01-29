using BusinessLayer;
using EntitiesLayer;
using EntitiesLayer.Entities;
using StoryWatch.UserControls.Movies;
using StoryWatch.UserControls.Games;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using StoryWatch.UserControls.Books;
using System.Windows.Markup;
using System.Windows.Input;

namespace StoryWatch.UserControls
{
    /// <summary>
    /// Function: Početna stranica za medije
    /// Author: Noa Midžić
    /// </summary>
    public partial class UCMediaHome : UserControl
    {
        private ListCategoryServices listCategoryServices = new ListCategoryServices();
        private List<MediaListBox> allMediaListBoxes = new List<MediaListBox>();
        private bool initialLoadOfAllLists = true;
        private List<IListCategory> allLists = new List<IListCategory>();
        Window window;

        public UCMediaHome(MediaCategory mediaCategory)
        {
            InitializeComponent();

            StateManager.CurrentMediaCategory = mediaCategory;
            allLists = listCategoryServices.GetListCategories(StateManager.CurrentMediaCategory, StateManager.LoggedUser);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            gridLists.Children.Clear();

            LoadLists(allLists);
            initialLoadOfAllLists = false;

            window = Window.GetWindow(this);
            //window.KeyDown += new KeyEventHandler(UCMediaHomee_KeyDown);
        }

        /*private void UCMediaHomee_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1 && (GuiManager.currentContent.Name == "UCMediaHomee"))
            {
                MessageBox.Show("FNESTO TESTs");
                window.KeyDown -= UCMediaHomee_KeyDown;
            }
        }*/

        private void LoadLists(List<IListCategory> listCategories)
        {
            
            foreach (var lc in listCategories)
            {
                UserControl mediaListBox = new MediaListBox(lc);
                ContentControl control = new ContentControl
                {
                    Content = mediaListBox
                };

                AddListBoxToGrid(control);
            }
        }

        private void txtSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = "";
            txtSearch.FontStyle = FontStyles.Normal;
            txtSearch.FontWeight = FontWeights.Normal;
            txtSearch.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void txtSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = "Search";
            txtSearch.FontStyle = FontStyles.Italic;
            txtSearch.FontWeight = FontWeights.Bold;
            txtSearch.Foreground = new SolidColorBrush(Colors.SlateGray);
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text) || txtSearch.Text == "Search")
            {
                if (gridLists != null)
                {
                    gridLists.Children.Clear();
                    LoadLists(allLists);
                }

                return;
            }

            string keyword = txtSearch.Text.ToLower();
            ShowListsContainingMediaWithKeyword(keyword);

        }

        private void ShowListsContainingMediaWithKeyword(string keyword)
        {
            //empty gridLists, need to remove all rowdefinitions created up until now
            //so that results aren't being put in row definitions below those already created
            //and only one row definition is present at each reset (column definitions always fixed at 3 so no need to reset them)
            gridLists.Children.Clear();
            gridLists.RowDefinitions.Clear();
            gridLists.RowDefinitions.Add(new RowDefinition());

            List<UserControl> listsContainingKeyword = new List<UserControl>();

            foreach (MediaListBox listBox in allMediaListBoxes)
            {
                var mediaItems = listBox.MediaItems;
                List<string> mediaTitles = mediaItems.Select(m => m.Title.ToLower()).ToList();
                //include in results if any of media titles on the list contain keyword
                int count = mediaTitles.Count(m => m.Contains(keyword));
                //also include in the results if the list title itself contains keyword
                string listTitle = listBox.lblTitle.Content.ToString().ToLower();
                count += (listTitle.Contains(keyword)) ? 1 : 0;

                if (count != 0)
                {
                    AddListBoxToGrid(new ContentControl
                    {
                        Content = listBox
                    });
                }
      
            }

        }


        private void btnAddCustomList_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UCAddCustomList());           
        }

        private void AddListBoxToGrid(ContentControl list)
        {
            var columnCount = gridLists.ColumnDefinitions.Count;
            var rowCount = gridLists.RowDefinitions.Count;

            var lastColumnChild = gridLists.Children
                                    .Cast<UIElement>()
                                    .FirstOrDefault(c => Grid.GetRow(c) == rowCount - 1 &&
                                                        Grid.GetColumn(c) == columnCount - 1);

            //add new row in case that in the existing last row, the last column is taken
            if (lastColumnChild != null)
            {
                gridLists.RowDefinitions.Add(new RowDefinition());
            }

            gridLists.Children.Add(list);
            Grid.SetRow(list, gridLists.RowDefinitions.Count - 1);
            Grid.SetColumn(list, (gridLists.Children.Count - 1) % columnCount);

            if (initialLoadOfAllLists)
            {
                allMediaListBoxes.Add(list.Content as MediaListBox);
            }
        }

        private void ReturnToHome(object sender, RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UCHome());
        }

        private void LogOut(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you wish to log out?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }
            StateManager.LoggedUser = null;
            GuiManager.OpenContent(new UCLogin());
        }

        private void btnRecommend_Click(object sender, RoutedEventArgs e)
        {
            switch (StateManager.CurrentMediaCategory)
            {
                case MediaCategory.Movie:
                    GuiManager.OpenContent(new UCRecommendMovies());
                    break;
                case MediaCategory.Book:
                    GuiManager.OpenContent(new UCRecommendBooks());
                    break;
                case MediaCategory.Game:
                    GuiManager.OpenContent(new UCRecommendGames());
                    break;
                default:
                    break;
            }
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            switch (StateManager.CurrentMediaCategory)
            {
                case MediaCategory.Movie:
                    var movieReport = new MovieReport();
                    movieReport.Show();
                    break;
                case MediaCategory.Book:
                    BookReport bookReport = new BookReport();
                    bookReport.Show();
                    break;
                case MediaCategory.Game:
                    var gameReport = new GameReport();
                    gameReport.Show();
                    break;
                default:
                    break;
            }
            
        }
    }
}
