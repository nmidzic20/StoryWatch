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

namespace StoryWatch.UserControls
{
    /// <summary>
    /// Autor: Noa Midžić
    /// Namjena: Početna stranica za medije
    /// </summary>
    public partial class UCMediaHome : UserControl
    {
        private ListCategoryServices listCategoryServices = new ListCategoryServices();
        private List<MediaListBox> allMediaListBoxes = new List<MediaListBox>();
        private bool firstPass = true;
        private List<IListCategory> allLists = new List<IListCategory>();


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
            firstPass = false;

            //save all created listboxes for reference in local search
            //foreach (UIElement child in gridLists.Children)
                //allMediaListBoxes.Add(StateManager.GetChildOfType<MediaListBox>(child));

        }

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
                    //foreach (UIElement child in gridLists.Children)
                        //child.Visibility = Visibility.Visible;

                return;
            }

            string keyword = txtSearch.Text.ToLower();
            ShowListsContainingMediaWithKeyword(keyword);

        }

        private void ShowListsContainingMediaWithKeyword(string keyword)
        {
            gridLists.Children.Clear();
            List<UserControl> listsContainingKeyword = new List<UserControl>();
            //foreach (UIElement child in gridLists.Children)
            foreach (MediaListBox listBox in allMediaListBoxes)
            {
                //var mediaItems = StateManager.GetChildOfType<MediaListBox>(child).MediaItems;
                var mediaItems = listBox.MediaItems;
                List<string> mediaTitles = mediaItems.Select(m => m.Title.ToLower()).ToList();
                //include in results if any of media titles on the list contain keyword
                int count = mediaTitles.Count(m => m.Contains(keyword));
                //also include in the results if the list title itself contains keyword
                string listTitle = listBox.lblTitle.Content.ToString().ToLower();
                count += (listTitle.Contains(keyword)) ? 1 : 0;

                if (count != 0)
                    AddListBoxToGrid(new UserControl
                    {
                        Content = listBox
                    });
           
                //listsContainingKeyword.Add(listBox);
                //child.Visibility = Visibility.Visible;
                //else
                //child.Visibility = Visibility.Collapsed;
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

            //dodati novi redak u slučaju da je u već postojećem zadnjem retku zadnji stupac zauzet
            if (lastColumnChild != null)
            {
                gridLists.RowDefinitions.Add(new RowDefinition());
            }

            gridLists.Children.Add(list);
            Grid.SetRow(list, gridLists.RowDefinitions.Count - 1);
            Grid.SetColumn(list, (gridLists.Children.Count - 1) % columnCount);

            if (firstPass)
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
            if (MessageBox.Show("Želite li se odjaviti?", "Obavijest", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
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
                    break;
                case MediaCategory.Game:
                    break;
                default:
                    break;
            }
            
        }
    }
}
