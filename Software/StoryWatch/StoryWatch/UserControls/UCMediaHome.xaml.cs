using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
            if (gridLists.Children.Count != 0) gridLists.Children.Clear();

            var listCategories = listCategoryServices.GetListCategories(StateManager.CurrentMediaCategory);

            foreach (var lc in listCategories)
            {
                UserControl mediaListBox = new MediaListBox(lc.Title, lc.Color);
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

        private void btnAddCustomList_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UCAddCustomList());           
        }

        private void AddListBoxToGrid(ContentControl list)//ListBox list)
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

        
    }
}
