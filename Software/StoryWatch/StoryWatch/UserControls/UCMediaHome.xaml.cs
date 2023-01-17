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
    /// Interaction logic for UCMediaHome.xaml
    /// </summary>
    public partial class UCMediaHome : UserControl
    {
        public UCMediaHome()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
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
            ListBox newCustomList = new ListBox();

            newCustomList.Items.Add("Mon");
            newCustomList.Items.Add("Tue");
            newCustomList.Items.Add("Wed");
            newCustomList.Items.Add("Thu");
            newCustomList.Items.Add("Fri");

            newCustomList.Width = 180;
            newCustomList.Height = 200;
            newCustomList.Name = "lbCustom"; //dati korisnički definirano ime
            newCustomList.Margin = new Thickness(20);
            ScrollViewer.SetVerticalScrollBarVisibility(newCustomList, ScrollBarVisibility.Visible);

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

            gridLists.Children.Add(newCustomList);
            Grid.SetRow(newCustomList, gridLists.RowDefinitions.Count - 1);
            Grid.SetColumn(newCustomList, (gridLists.Children.Count - 1) % columnCount);

        }

        private void ReturnToHome(object sender, RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UCHome());
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            //button.Background = new SolidColorBrush(Colors.BlanchedAlmond);
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            //button.Background = new SolidColorBrush(Colors.CornflowerBlue);
        }
    }
}
