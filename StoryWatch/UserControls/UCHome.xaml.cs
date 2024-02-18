using BusinessLayer;
using EntitiesLayer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for UCHome.xaml
    /// Author: Noa Midžić
    /// </summary>
    public partial class UCHome : UserControl
    {
        Window window;
        public UCHome()
        {
            InitializeComponent();
        }

        private void btnBooks_Click(object sender, RoutedEventArgs e)
        {
           GuiManager.OpenContent(new UCMediaHome(MediaCategory.Book));
        }

        private void btnMovies_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UCMediaHome(MediaCategory.Movie));
        }

        private void btnGames_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UCMediaHome(MediaCategory.Game));
        }

        /*private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            window = Window.GetWindow(this);
            window.KeyDown += new KeyEventHandler(UCHomee_KeyDown);
        }

        private void UCHomee_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1 && (GuiManager.currentContent.Name == "UCHomee"))
            {
                MessageBox.Show(this.GetType().Name);
                //System.Diagnostics.Process.Start(@"PDF\\UCHome.pdf");
                window.KeyDown -= UCHomee_KeyDown;
            }
        }*/
    }
}
