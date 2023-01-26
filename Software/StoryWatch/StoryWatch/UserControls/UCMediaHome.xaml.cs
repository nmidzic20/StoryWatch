using BusinessLayer;
using EntitiesLayer;
using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity.Core.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Autor: Noa Midžić
    /// Namjena: Početna stranica za medije
    /// </summary>
    public partial class UCMediaHome : UserControl
    {
        private ListCategoryServices listCategoryServices = new ListCategoryServices();

        public UCMediaHome(MediaCategory mediaCategory)
        {
            InitializeComponent();

            StateManager.CurrentMediaCategory = mediaCategory;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            gridLists.Children.Clear();

            var allLists = listCategoryServices.GetListCategories(StateManager.CurrentMediaCategory, StateManager.LoggedUser);

            LoadLists(allLists);
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
                return;

            List<MediaListBox> allMediaListBoxes = new List<MediaListBox>();

            var contentControls = new List<DependencyObject>();
            for (int i = 0; i < gridLists.Children.Count; i++)
                contentControls.Add(VisualTreeHelper.GetChild(gridLists, i));

            foreach (var contentControl in contentControls)
                allMediaListBoxes.Add(StateManager.GetChildOfType<MediaListBox>(contentControl));

            //var lists = StateManager.GetChildOfType<MediaListBox>(gridLists);
            string movies = "";
            foreach (var list in allMediaListBoxes)
                movies += list.MediaItems.Count != 0 ? " " + list.MediaItems[0].ToString() + " " : " nema ";
            MessageBox.Show(movies);

            foreach (UIElement child in gridLists.Children)
                child.Visibility = Visibility.Collapsed;
            //gridLists.Children.Clear();

            //List<IListCategory> listsContainingSearchedMedia = new List<IListCategory>();
            //listsContainingSearchedMedia.Add(allMediaListBoxes[0].listCategory);
            //ShowLists(listsContainingSearchedMedia);
        

            foreach (UIElement child in gridLists.Children)
                if (StateManager.GetChildOfType<MediaListBox>(child).MediaItems
                    .Count(m => m.Title.Contains("hinin")) != 0)
                    
                    child.Visibility = Visibility.Visible;

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
    }
}
