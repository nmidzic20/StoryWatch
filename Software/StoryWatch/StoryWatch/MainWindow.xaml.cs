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

namespace StoryWatch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            stackPanelButtons.Visibility = Visibility.Visible;
            stackPanelTitle.Visibility = Visibility.Visible;
            contentPanel.Visibility = Visibility.Collapsed;
        }
        private void btnBooks_Click(object sender, RoutedEventArgs e)
        {
            stackPanelTitle.Visibility = Visibility.Collapsed;
            stackPanelButtons.Visibility = Visibility.Collapsed;
            contentPanel.Visibility = Visibility.Visible;
            contentPanel.Content = new UCBooksCRUD();
            /*var booksCRUD = new BooksCRUD();
            booksCRUD.Show();
            Close();*/
        }

        private void btnMovies_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnGames_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
